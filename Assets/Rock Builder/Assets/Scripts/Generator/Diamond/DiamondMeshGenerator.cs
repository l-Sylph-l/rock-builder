using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RockBuilder
{
    public class DiamondMeshGenerator
    {
        private static DiamondMeshGenerator instance = null;
        private static readonly object padlock = new object();

        DiamondMeshGenerator()
        {
        }

        public static DiamondMeshGenerator Instance
        {
            get
            {
                lock (padlock)
                {
                    if (instance == null)
                    {
                        instance = new DiamondMeshGenerator();
                    }
                    return instance;
                }
            }
        }

        public List<Vector3> CreateVertexPositions(Diamond diamond)
        {
            List<Vector3> spawnPoints = new List<Vector3>();

            // Get the point for the bottom peak
            spawnPoints.Add(diamond.transform.position - (Vector3.up * (diamond.pavillonHeight)));

            // Get the points of the middle pavillon circle
            for (int loopCount = 0; diamond.edges / 2 > loopCount; loopCount++)
            {
                float height = diamond.pavillonHeight * diamond.bottomRadiusPosition;
                float radius = diamond.radius * diamond.bottomRadiusPosition;
                Vector3 spawnPoint = DrawCircularVertices(diamond, radius, -height, diamond.edges / 2, loopCount, false);
                spawnPoints.Add(spawnPoint);
            }

            // Get the points of the upper pavillon circle
            for (int loopCount = 0; diamond.edges > loopCount; loopCount++)
            {
                Vector3 spawnPoint = DrawCircularVertices(diamond, diamond.radius, 0f, diamond.edges, loopCount, false);
                spawnPoints.Add(spawnPoint);
            }

            // Get the points for the upper circle
            for (int loopCount = 0; diamond.edges / 2 > loopCount; loopCount++)
            {
                Vector3 spawnPoint = DrawCircularVertices(diamond, diamond.radius * 0.75f, diamond.crownHeight / 2, diamond.edges / 2, loopCount, false);
                spawnPoints.Add(spawnPoint);
            }

            // Get the points for the upper plane
            for (int loopCount = 0; diamond.edges / 2 > loopCount; loopCount++)
            {
                Vector3 spawnPoint = DrawCircularVertices(diamond, diamond.radius / 2, diamond.crownHeight, diamond.edges / 2, loopCount, true);
                spawnPoints.Add(spawnPoint);
            }

            // Get the vertex position in the middle from the upper plane
            spawnPoints.Add(diamond.transform.position + (Vector3.up * (diamond.crownHeight)));

            return spawnPoints;
        }

        private Vector3 DrawCircularVertices(Diamond diamond, float radius, float height, int edges, int loopCount, bool offset)
        {
            Vector3 spawnPoint;
            float degree = (360f / edges) * loopCount;
            if (offset)
            {
                degree += (360f / edges) / 2;
            }
            float radian = degree * Mathf.Deg2Rad;
            float x = Mathf.Cos(radian);
            float z = Mathf.Sin(radian);
            spawnPoint = new Vector3(x, 0, z) * radius;
            spawnPoint.y = height;
            spawnPoint += diamond.transform.position;
            return spawnPoint;
        }

        public Mesh CreateMesh(Diamond diamond)
        {
            if (diamond.smooth)
            {
                //return CreateSmoothMesh(diamond);
            }
            else
            {
                return CreateHardMesh(diamond);
            }
            return null;
            //CreateLods(Diamond);
        }

        private Mesh CreateHardMesh(Diamond diamond)
        {
            List<Vector3> vertexPositions = diamond.vertexPositions;
            int halfAmountOfEdges = diamond.edges / 2;

            // Initialize variables for vertices logic
            int initialVerticesCount = diamond.edges * 13;
            Vector3[] vertices = new Vector3[initialVerticesCount];
            int vertexLoop = 0;

            // Initialize variables for uv logic
            Vector2[] uv = new Vector2[initialVerticesCount];
            float circumference = Vector3.Distance(vertexPositions[0], vertexPositions[1]) * diamond.edges;
            float uvHeightBody = ((diamond.pavillonHeight / circumference) * 2);
            float uvHeightTotal = (((diamond.crownHeight + diamond.pavillonHeight) / circumference) * 2);

            // Initialize variables for triangle logic
            int[] triangles = new int[diamond.edges * 15];
            int verticesCount = 0;
            int triangleVerticesCount = 0;

            // Calculate vertices, uv and triangles for the first part of the pavillon
            for (int loopCount = 0; halfAmountOfEdges > loopCount; loopCount++)
            {
                int bootomPeakStartIndex = 0;
                int pavillonStartIndex = 1;
                int upperPavillonStartIndex = halfAmountOfEdges + 2;

                vertices[vertexLoop] = vertexPositions[bootomPeakStartIndex] - diamond.transform.position;
                vertices[vertexLoop + 1] = vertexPositions[pavillonStartIndex + loopCount] - diamond.transform.position;
                vertices[vertexLoop + 2] = vertexPositions[upperPavillonStartIndex + loopCount * 2] - diamond.transform.position;

                if (halfAmountOfEdges - 1 != loopCount)
                {
                    vertices[vertexLoop + 3] = vertexPositions[pavillonStartIndex + loopCount + 1] - diamond.transform.position;
                }
                else
                {
                    vertices[vertexLoop + 3] = vertexPositions[pavillonStartIndex] - diamond.transform.position;
                }

                uv[vertexLoop] = new Vector2(((float)loopCount + 0.5f) / (float)halfAmountOfEdges, 0f);
                uv[vertexLoop + 1] = new Vector2((float)loopCount / (float)halfAmountOfEdges, uvHeightBody / 2);
                uv[vertexLoop + 2] = new Vector2(((float)loopCount + 0.5f) / (float)halfAmountOfEdges, uvHeightBody);
                uv[vertexLoop + 3] = new Vector2(((float)loopCount + 1f) / (float)halfAmountOfEdges, uvHeightBody / 2);

                triangles[triangleVerticesCount] = verticesCount;
                triangles[triangleVerticesCount + 1] = verticesCount + 1;
                triangles[triangleVerticesCount + 2] = verticesCount + 2;
                triangles[triangleVerticesCount + 3] = verticesCount;
                triangles[triangleVerticesCount + 4] = verticesCount + 2;
                triangles[triangleVerticesCount + 5] = verticesCount + 3;

                triangleVerticesCount += 6;
                verticesCount += 4;

                vertexLoop = vertexLoop + 4;
            }

            // Calculate vertices, uv and triangles for the second part of the pavillon
            for (int loopCount = 0; halfAmountOfEdges > loopCount; loopCount++)
            {

                int pavillonStartIndex = 1;
                int upperPavillonStartIndex = halfAmountOfEdges + 1 + (loopCount * 2);

                vertices[vertexLoop] = vertexPositions[pavillonStartIndex + loopCount] - diamond.transform.position;
                vertices[vertexLoop + 1] = vertexPositions[upperPavillonStartIndex] - diamond.transform.position;
                vertices[vertexLoop + 2] = vertexPositions[upperPavillonStartIndex + 1] - diamond.transform.position;
                vertices[vertexLoop + 4] = vertexPositions[upperPavillonStartIndex + 1] - diamond.transform.position;

                if (halfAmountOfEdges - 1 != loopCount)
                {
                    vertices[vertexLoop + 3] = vertexPositions[pavillonStartIndex + loopCount + 1] - diamond.transform.position;
                    vertices[vertexLoop + 5] = vertexPositions[upperPavillonStartIndex + 2] - diamond.transform.position;
                }
                else
                {
                    vertices[vertexLoop + 3] = vertexPositions[pavillonStartIndex] - diamond.transform.position;
                    vertices[vertexLoop + 5] = vertexPositions[halfAmountOfEdges + 1] - diamond.transform.position;
                }

                uv[vertexLoop] = new Vector2(((float)loopCount + 1f) / (float)halfAmountOfEdges, uvHeightBody / 2);
                uv[vertexLoop + 1] = new Vector2(((float)loopCount + 0.5f) / (float)halfAmountOfEdges, uvHeightBody);
                uv[vertexLoop + 2] = new Vector2(((float)loopCount + 1f) / (float)halfAmountOfEdges, uvHeightBody);
                uv[vertexLoop + 3] = new Vector2(((float)loopCount + 1f) / (float)halfAmountOfEdges, uvHeightBody / 2);
                uv[vertexLoop + 4] = new Vector2(((float)loopCount + 1f) / (float)halfAmountOfEdges, uvHeightBody);
                uv[vertexLoop + 5] = new Vector2(((float)loopCount + 1.5f) / (float)halfAmountOfEdges, uvHeightBody);

                triangles[triangleVerticesCount] = verticesCount;
                triangles[triangleVerticesCount + 1] = verticesCount + 1;
                triangles[triangleVerticesCount + 2] = verticesCount + 2;
                triangles[triangleVerticesCount + 3] = verticesCount + 3;
                triangles[triangleVerticesCount + 4] = verticesCount + 4;
                triangles[triangleVerticesCount + 5] = verticesCount + 5;

                triangleVerticesCount += 6;
                verticesCount += 6;

                vertexLoop = vertexLoop + 6;
            }

            // Calculate vertices, uv and triangles for the first part of the crown
            for (int loopCount = 0; halfAmountOfEdges > loopCount; loopCount++)
            {

                int crownStartIndex = (halfAmountOfEdges * 3) + 1;
                int upperPavillonStartIndex = halfAmountOfEdges + 1 + (loopCount * 2);

                vertices[vertexLoop] = vertexPositions[upperPavillonStartIndex] - diamond.transform.position;
                vertices[vertexLoop + 1] = vertexPositions[crownStartIndex + loopCount] - diamond.transform.position;
                vertices[vertexLoop + 2] = vertexPositions[upperPavillonStartIndex + 1] - diamond.transform.position;
                vertices[vertexLoop + 5] = vertexPositions[upperPavillonStartIndex + 1] - diamond.transform.position;

                if (halfAmountOfEdges - 1 != loopCount)
                {
                    vertices[vertexLoop + 3] = vertexPositions[crownStartIndex + loopCount + 1] - diamond.transform.position;
                    vertices[vertexLoop + 4] = vertexPositions[upperPavillonStartIndex + 2] - diamond.transform.position;
                }
                else
                {
                    vertices[vertexLoop + 3] = vertexPositions[crownStartIndex] - diamond.transform.position;
                    vertices[vertexLoop + 4] = vertexPositions[halfAmountOfEdges + 1] - diamond.transform.position;
                }

                triangles[triangleVerticesCount] = verticesCount;
                triangles[triangleVerticesCount + 1] = verticesCount + 1;
                triangles[triangleVerticesCount + 2] = verticesCount + 2;
                triangles[triangleVerticesCount + 3] = verticesCount + 3;
                triangles[triangleVerticesCount + 4] = verticesCount + 4;
                triangles[triangleVerticesCount + 5] = verticesCount + 5;

                triangleVerticesCount += 6;
                verticesCount += 6;

                vertexLoop = vertexLoop + 6;
            }

            // Calculate vertices, uv and triangles for the second part of the crown
            for (int loopCount = 0; halfAmountOfEdges > loopCount; loopCount++)
            {

                int crownStartIndex = (halfAmountOfEdges * 3) + 1;
                int upperCrownStartIndex = (halfAmountOfEdges * 4) + 1;
                int upperPavillonStartIndex = halfAmountOfEdges + (2 + (loopCount * 2));

                vertices[vertexLoop] = vertexPositions[upperPavillonStartIndex] - diamond.transform.position;
                vertices[vertexLoop + 1] = vertexPositions[crownStartIndex + loopCount] - diamond.transform.position;
                vertices[vertexLoop + 2] = vertexPositions[upperCrownStartIndex + loopCount] - diamond.transform.position;

                if (halfAmountOfEdges - 1 != loopCount)
                {
                    vertices[vertexLoop + 3] = vertexPositions[crownStartIndex + loopCount + 1] - diamond.transform.position;
                }
                else
                {
                    vertices[vertexLoop + 3] = vertexPositions[crownStartIndex] - diamond.transform.position;
                }

                triangles[triangleVerticesCount] = verticesCount;
                triangles[triangleVerticesCount + 1] = verticesCount + 1;
                triangles[triangleVerticesCount + 2] = verticesCount + 2;
                triangles[triangleVerticesCount + 3] = verticesCount;
                triangles[triangleVerticesCount + 4] = verticesCount + 2;
                triangles[triangleVerticesCount + 5] = verticesCount + 3;

                triangleVerticesCount += 6;
                verticesCount += 4;

                vertexLoop = vertexLoop + 4;
            }

            // Calculate vertices, uv and triangles for the third part of the crown
            for (int loopCount = 0; halfAmountOfEdges > loopCount; loopCount++)
            {

                int crownStartIndex = (halfAmountOfEdges * 3) + 1 + loopCount;
                int upperCrownStartIndex = (halfAmountOfEdges * 4) + 1 + loopCount;

                vertices[vertexLoop + 1] = vertexPositions[upperCrownStartIndex] - diamond.transform.position;

                if (halfAmountOfEdges - 1 != loopCount)
                {
                    vertices[vertexLoop] = vertexPositions[crownStartIndex + 1] - diamond.transform.position;
                    vertices[vertexLoop + 2] = vertexPositions[upperCrownStartIndex + 1] - diamond.transform.position;
                }
                else
                {
                    vertices[vertexLoop] = vertexPositions[(halfAmountOfEdges * 3) + 1] - diamond.transform.position;
                    vertices[vertexLoop + 2] = vertexPositions[(halfAmountOfEdges * 4) + 1] - diamond.transform.position;
                }

                triangles[triangleVerticesCount] = verticesCount;
                triangles[triangleVerticesCount + 1] = verticesCount + 1;
                triangles[triangleVerticesCount + 2] = verticesCount + 2;

                triangleVerticesCount += 3;
                verticesCount += 3;

                vertexLoop = vertexLoop + 3;
            }

            // Calculate vertices, uv and triangles for the upper plane
            for (int loopCount = 0; halfAmountOfEdges > loopCount; loopCount++)
            {
                int upperMiddle = (halfAmountOfEdges * 5) + 1;
                int upperCrownStartIndex = (halfAmountOfEdges * 4) + 1 + loopCount;

                vertices[vertexLoop] = vertexPositions[upperCrownStartIndex] - diamond.transform.position;
                vertices[vertexLoop + 1] = vertexPositions[upperMiddle] - diamond.transform.position;

                if (halfAmountOfEdges - 1 != loopCount)
                {
                    vertices[vertexLoop + 2] = vertexPositions[upperCrownStartIndex + 1] - diamond.transform.position;
                }
                else
                {
                    vertices[vertexLoop + 2] = vertexPositions[(halfAmountOfEdges * 4) + 1] - diamond.transform.position;
                }

                triangles[triangleVerticesCount] = verticesCount;
                triangles[triangleVerticesCount + 1] = verticesCount + 1;
                triangles[triangleVerticesCount + 2] = verticesCount + 2;

                triangleVerticesCount += 3;
                verticesCount += 3;

                vertexLoop = vertexLoop + 3;
            }

            Mesh mesh = new Mesh();
            mesh.Clear();
            mesh.vertices = vertices;
            mesh.triangles = triangles;
            mesh.uv = uv;
            mesh.name = "generated diamond mesh";
            mesh.Optimize();
            mesh.RecalculateNormals();
            return mesh;
        }

        //private Mesh CreateSmoothMesh(Diamond diamond)
        //{

        //    List<Vector3> vertexPositions = diamond.vertexPositions;

        //    int vrticesCount = (diamond.edges * 4) + 2;
        //    Vector3[] vertices = new Vector3[vrticesCount];
        //    Vector2[] uv = new Vector2[vrticesCount];
        //    int vertexLoop = 0;
        //    float circumference = Vector3.Distance(vertexPositions[0], vertexPositions[1]) * diamond.edges;
        //    float uvHeightBody = (1f - (diamond.height / circumference)) / 2;
        //    float uvHeightTotal = (1f - ((diamond.heightPeak * 2 + diamond.height) / circumference)) / 4;

        //    for (int loopCount = 0; diamond.edges > loopCount; loopCount++)
        //    {
        //        vertices[vertexLoop] = vertexPositions[diamond.edges + loopCount] - diamond.transform.position;
        //        vertices[vertexLoop + 1] = vertexPositions[loopCount] - diamond.transform.position;

        //        uv[vertexLoop] = new Vector2((float)loopCount / (float)diamond.edges, 1f - uvHeightBody);
        //        uv[vertexLoop + 1] = new Vector2(((float)loopCount) / (float)diamond.edges, uvHeightBody);

        //        vertexLoop = vertexLoop + 2;
        //    }

        //    vertices[vertexLoop] = vertexPositions[diamond.edges] - diamond.transform.position;
        //    vertices[vertexLoop + 1] = vertexPositions[0] - diamond.transform.position;
        //    uv[vertexLoop] = new Vector2(1f, 1f - uvHeightBody);
        //    uv[vertexLoop + 1] = new Vector2(1f, uvHeightBody);
        //    vertexLoop = vertexLoop + 2;

        //    // Get the vertices for both peaks
        //    for (int loopCount = 0; diamond.edges > loopCount; loopCount++)
        //    {
        //        vertices[vertexLoop] = vertexPositions[(diamond.edges * 2) + 1] - diamond.transform.position;
        //        uv[vertexLoop] = new Vector2(((float)loopCount / (float)diamond.edges) + 1f / (float)diamond.edges / 2f, uvHeightTotal);
        //        vertexLoop++;
        //    }

        //    for (int loopCount = 0; diamond.edges > loopCount; loopCount++)
        //    {
        //        vertices[vertexLoop] = vertexPositions[(diamond.edges * 2)] - diamond.transform.position;
        //        uv[vertexLoop] = new Vector2(((float)loopCount / (float)diamond.edges) + 1f / (float)diamond.edges / 2f, 1f - uvHeightTotal);
        //        vertexLoop++;
        //    }

        //    #region Draw triangles 
        //    int[] triangles = new int[(diamond.edges * 12) + 6];
        //    int loopCountBody = (diamond.edges * 2) + 2;
        //    int loopCountPeak = diamond.edges * 2;
        //    int verticesCount = 0;
        //    int triangleVerticesCount = 0;

        //    for (int i = 0; verticesCount < loopCountBody; i = i - 1)
        //    {
        //        triangles[triangleVerticesCount] = i;
        //        triangles[triangleVerticesCount + 1] = i = i + 3;
        //        triangles[triangleVerticesCount + 2] = i = i - 2;
        //        triangles[triangleVerticesCount + 3] = i = i - 1;
        //        triangles[triangleVerticesCount + 4] = i = i + 2;
        //        triangles[triangleVerticesCount + 5] = i = i + 1;

        //        if (verticesCount == loopCountBody - 2)
        //        {
        //            triangles[triangleVerticesCount + 1] = 1;
        //            triangles[triangleVerticesCount + 4] = 0;
        //            triangles[triangleVerticesCount + 5] = 1;
        //        }

        //        triangleVerticesCount += 6;
        //        verticesCount += 2;
        //    }

        //    for (int i = 0; 0 < loopCountPeak; i = i - 1)
        //    {
        //        triangles[triangleVerticesCount] = verticesCount + diamond.edges;
        //        triangles[triangleVerticesCount + 1] = i = i + 2;
        //        triangles[triangleVerticesCount + 2] = i = i - 2;
        //        triangles[triangleVerticesCount + 3] = verticesCount;
        //        triangles[triangleVerticesCount + 4] = i = i + 1;
        //        triangles[triangleVerticesCount + 5] = i = i + 2;

        //        if (loopCountPeak == 2)
        //        {
        //            triangles[triangleVerticesCount + 1] = diamond.edges * 2;
        //            triangles[triangleVerticesCount + 5] = (diamond.edges * 2) + 1;
        //        }

        //        triangleVerticesCount += 6;
        //        verticesCount++;
        //        loopCountPeak -= 2;
        //    }
        //    #endregion

        //    Mesh mesh = new Mesh();
        //    mesh.Clear();
        //    mesh.vertices = vertices;
        //    mesh.triangles = triangles;
        //    mesh.uv = uv;
        //    mesh.name = "generated diamond mesh";
        //    mesh.RecalculateNormals();


        //    #region Recalculate some normals manually for smoother shading. 
        //    Vector3[] normals = mesh.normals;

        //    Vector3 averageNormal1 = (normals[0] + normals[(diamond.edges * 2)]) / 2;
        //    normals[0] = averageNormal1;
        //    normals[(diamond.edges * 2)] = averageNormal1;

        //    Vector3 averageNormal2 = (normals[1] + normals[(diamond.edges * 2) + 1]) / 2;
        //    normals[1] = averageNormal2;
        //    normals[(diamond.edges * 2) + 1] = averageNormal2;

        //    for (int i = 1; i < diamond.edges + 1; i++)
        //    {
        //        normals[normals.Length - i] = new Vector3(0f, 1f, 0f);
        //        normals[normals.Length - i - diamond.edges] = new Vector3(0f, -1f, 0f);
        //    }

        //    mesh.normals = normals;
        //    #endregion
        //    mesh.Optimize();
        //    return mesh;
        //}
    }
}
