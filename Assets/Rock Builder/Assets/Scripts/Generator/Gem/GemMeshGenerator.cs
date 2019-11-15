using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RockBuilder
{
    public class GemMeshGenerator
    {
        private static GemMeshGenerator instance = null;
        private static readonly object padlock = new object();

        GemMeshGenerator()
        {
        }

        public static GemMeshGenerator Instance
        {
            get
            {
                lock (padlock)
                {
                    if (instance == null)
                    {
                        instance = new GemMeshGenerator();
                    }
                    return instance;
                }
            }
        }

        public List<Vector3> CreateVertexPositions(Gem gem)
        {
            List<Vector3> spawnPoints = new List<Vector3>();

            float startPositionZ = -gem.width / 2;
            // Get the vertex for the bottom middlepoint
            spawnPoints.Add(gem.transform.position + (Vector3.forward * startPositionZ));

            for (int loopCountZ = 0; 5 > loopCountZ; loopCountZ++)
            {
                float positionZ = startPositionZ + (gem.width / 4) * loopCountZ;
                float radiusX;
                float radiusY;
                if (loopCountZ < 3)
                {
                    radiusX = (gem.radiusX / 3) * (loopCountZ + 1);
                    radiusY = (gem.radiusY / 3) * (loopCountZ + 1);
                }
                else
                {
                    radiusX = gem.radiusX - ((gem.radiusX / 3) * (loopCountZ-2));
                    radiusY = gem.radiusY - ((gem.radiusY / 3) * (loopCountZ-2));
                }

                int edges;
                if (loopCountZ == 2)
                {
                    edges = gem.edges;
                } else
                {
                    edges = gem.edges/2;
                }

                bool offset;

                if (loopCountZ == 1 || loopCountZ == 3)
                {
                    offset = true;
                }
                else
                {
                    offset = false;
                }
           
                // Get the vertices for the body
                for (int loopCount = 0; edges > loopCount; loopCount++)
                {
                    Vector3 spawnPoint = DrawCircularVertices(gem, radiusX, radiusY, positionZ, edges, loopCount, offset);
                    spawnPoints.Add(spawnPoint);
                }
            }

            // Get the vertex for the upper middlepoint
            float endPositionZ = gem.width / 2;
            spawnPoints.Add(gem.transform.position + (Vector3.forward * endPositionZ));

            return spawnPoints;
        }

        private Vector3 DrawCircularVertices(Gem gem, float radiusX, float radiusY, float positionZ, int edges, int loopCount, bool offset)
        {
            Vector3 spawnPoint;
            float degree = (360f / edges) * loopCount;
            if (offset)
            {
                degree += (360f / edges) / 2;
            }
            float radian = degree * Mathf.Deg2Rad;
            float x = Mathf.Cos(radian) * radiusX;
            float y = Mathf.Sin(radian) * radiusY;
            spawnPoint = new Vector3(x, y, 0);
            spawnPoint.z = positionZ;
            spawnPoint += gem.transform.position;
            return spawnPoint;
        }

        public Mesh CreateMesh(Gem gem)
        {
            if (gem.smoothFlag)
            {
                return CreateSmoothMesh(gem);
            }
            else
            {
                return CreateHardMesh(gem);
            }
            //CreateLods(Gem);
        }

        private Mesh CreateHardMesh(Gem gem)
        {
            List<Vector3> vertexPositions = gem.vertexPositions;
            int halfAmountOfEdges = gem.edges / 2;

            // Initialize variables for vertices logic
            int initialVerticesCount = gem.edges * 13;
            Vector3[] vertices = new Vector3[initialVerticesCount];
            int vertexLoop = 0;

            // Initialize variables for uv logic
            Vector2[] uv = new Vector2[initialVerticesCount];

            // Initialize variables for triangle logic
            int[] triangles = new int[gem.edges * 15];
            int verticesCount = 0;
            int triangleVerticesCount = 0;

            // Calculate vertices, uv and triangles for the first part of the pavillon
            for (int loopCount = 0; halfAmountOfEdges > loopCount; loopCount++)
            {
                int bootomPeakStartIndex = 0;
                int pavillonStartIndex = 1;
                int upperPavillonStartIndex = halfAmountOfEdges + 2;

                vertices[vertexLoop] = vertexPositions[bootomPeakStartIndex] - gem.transform.position;
                vertices[vertexLoop + 1] = vertexPositions[pavillonStartIndex + loopCount] - gem.transform.position;
                vertices[vertexLoop + 2] = vertexPositions[upperPavillonStartIndex + loopCount * 2] - gem.transform.position;

                if (halfAmountOfEdges - 1 != loopCount)
                {
                    vertices[vertexLoop + 3] = vertexPositions[pavillonStartIndex + loopCount + 1] - gem.transform.position;
                }
                else
                {
                    vertices[vertexLoop + 3] = vertexPositions[pavillonStartIndex] - gem.transform.position;
                }

                uv[vertexLoop] = new Vector2(.5f, .5f);
                uv[vertexLoop + 1] = DrawCircularVerticesForUv(halfAmountOfEdges, .25f, .5f + loopCount * -1f);
                uv[vertexLoop + 2] = DrawCircularVerticesForUv(halfAmountOfEdges, .5f, 0f + loopCount * -1f);
                uv[vertexLoop + 3] = DrawCircularVerticesForUv(halfAmountOfEdges, .25f, -.5f + loopCount * -1f);

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

                vertices[vertexLoop] = vertexPositions[pavillonStartIndex + loopCount] - gem.transform.position;
                vertices[vertexLoop + 1] = vertexPositions[upperPavillonStartIndex] - gem.transform.position;
                vertices[vertexLoop + 2] = vertexPositions[upperPavillonStartIndex + 1] - gem.transform.position;
                vertices[vertexLoop + 4] = vertexPositions[upperPavillonStartIndex + 1] - gem.transform.position;

                if (halfAmountOfEdges - 1 != loopCount)
                {
                    vertices[vertexLoop + 3] = vertexPositions[pavillonStartIndex + loopCount + 1] - gem.transform.position;
                    vertices[vertexLoop + 5] = vertexPositions[upperPavillonStartIndex + 2] - gem.transform.position;
                }
                else
                {
                    vertices[vertexLoop + 3] = vertexPositions[pavillonStartIndex] - gem.transform.position;
                    vertices[vertexLoop + 5] = vertexPositions[halfAmountOfEdges + 1] - gem.transform.position;
                }

                uv[vertexLoop] = DrawCircularVerticesForUv(halfAmountOfEdges, .25f, -.5f - loopCount + 1);
                uv[vertexLoop + 1] = DrawCircularVerticesForUv(halfAmountOfEdges, .5f, -.5f - loopCount + 1);
                uv[vertexLoop + 2] = DrawCircularVerticesForUv(halfAmountOfEdges, .5f, -1f - loopCount + 1);
                uv[vertexLoop + 3] = DrawCircularVerticesForUv(halfAmountOfEdges, .25f, -1.5f - loopCount + 1);
                uv[vertexLoop + 4] = DrawCircularVerticesForUv(halfAmountOfEdges, .5f, -1f - loopCount + 1);
                uv[vertexLoop + 5] = DrawCircularVerticesForUv(halfAmountOfEdges, .5f, -1.5f - loopCount + 1);


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

                vertices[vertexLoop] = vertexPositions[upperPavillonStartIndex] - gem.transform.position;
                vertices[vertexLoop + 1] = vertexPositions[crownStartIndex + loopCount] - gem.transform.position;
                vertices[vertexLoop + 2] = vertexPositions[upperPavillonStartIndex + 1] - gem.transform.position;
                vertices[vertexLoop + 5] = vertexPositions[upperPavillonStartIndex + 1] - gem.transform.position;

                if (halfAmountOfEdges - 1 != loopCount)
                {
                    vertices[vertexLoop + 3] = vertexPositions[crownStartIndex + loopCount + 1] - gem.transform.position;
                    vertices[vertexLoop + 4] = vertexPositions[upperPavillonStartIndex + 2] - gem.transform.position;
                }
                else
                {
                    vertices[vertexLoop + 3] = vertexPositions[crownStartIndex] - gem.transform.position;
                    vertices[vertexLoop + 4] = vertexPositions[halfAmountOfEdges + 1] - gem.transform.position;
                }

                uv[vertexLoop] = DrawCircularVerticesForUv(halfAmountOfEdges, .5f, 1f + loopCount + 1);
                uv[vertexLoop + 1] = DrawCircularVerticesForUv(halfAmountOfEdges, .375f, 1f + loopCount + 1);
                uv[vertexLoop + 2] = DrawCircularVerticesForUv(halfAmountOfEdges, .5f, 1.5f + loopCount + 1);
                uv[vertexLoop + 3] = DrawCircularVerticesForUv(halfAmountOfEdges, .375f, 2f + loopCount + 1);
                uv[vertexLoop + 4] = DrawCircularVerticesForUv(halfAmountOfEdges, .5f, 2f + loopCount + 1);
                uv[vertexLoop + 5] = DrawCircularVerticesForUv(halfAmountOfEdges, .5f, 1.5f + loopCount + 1);

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

                vertices[vertexLoop] = vertexPositions[upperPavillonStartIndex] - gem.transform.position;
                vertices[vertexLoop + 1] = vertexPositions[crownStartIndex + loopCount] - gem.transform.position;
                vertices[vertexLoop + 2] = vertexPositions[upperCrownStartIndex + loopCount] - gem.transform.position;

                if (halfAmountOfEdges - 1 != loopCount)
                {
                    vertices[vertexLoop + 3] = vertexPositions[crownStartIndex + loopCount + 1] - gem.transform.position;
                }
                else
                {
                    vertices[vertexLoop + 3] = vertexPositions[crownStartIndex] - gem.transform.position;
                }

                uv[vertexLoop] = DrawCircularVerticesForUv(halfAmountOfEdges, .5f, 1.5f + loopCount + 1);
                uv[vertexLoop + 1] = DrawCircularVerticesForUv(halfAmountOfEdges, .375f, 1f + loopCount + 1);
                uv[vertexLoop + 2] = DrawCircularVerticesForUv(halfAmountOfEdges, .25f, 1.5f + loopCount + 1);
                uv[vertexLoop + 3] = DrawCircularVerticesForUv(halfAmountOfEdges, .375f, 2f + loopCount + 1);

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

                vertices[vertexLoop + 1] = vertexPositions[upperCrownStartIndex] - gem.transform.position;

                if (halfAmountOfEdges - 1 != loopCount)
                {
                    vertices[vertexLoop] = vertexPositions[crownStartIndex + 1] - gem.transform.position;
                    vertices[vertexLoop + 2] = vertexPositions[upperCrownStartIndex + 1] - gem.transform.position;
                }
                else
                {
                    vertices[vertexLoop] = vertexPositions[(halfAmountOfEdges * 3) + 1] - gem.transform.position;
                    vertices[vertexLoop + 2] = vertexPositions[(halfAmountOfEdges * 4) + 1] - gem.transform.position;
                }

                uv[vertexLoop] = DrawCircularVerticesForUv(halfAmountOfEdges, .375f, 2f + loopCount + 1);
                uv[vertexLoop + 1] = DrawCircularVerticesForUv(halfAmountOfEdges, .25f, 1.5f + loopCount + 1);
                uv[vertexLoop + 2] = DrawCircularVerticesForUv(halfAmountOfEdges, .25f, 2.5f + loopCount + 1);

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

                vertices[vertexLoop] = vertexPositions[upperCrownStartIndex] - gem.transform.position;
                vertices[vertexLoop + 1] = vertexPositions[upperMiddle] - gem.transform.position;

                if (halfAmountOfEdges - 1 != loopCount)
                {
                    vertices[vertexLoop + 2] = vertexPositions[upperCrownStartIndex + 1] - gem.transform.position;
                }
                else
                {
                    vertices[vertexLoop + 2] = vertexPositions[(halfAmountOfEdges * 4) + 1] - gem.transform.position;
                }

                uv[vertexLoop] = DrawCircularVerticesForUv(halfAmountOfEdges, .25f, 1.5f + loopCount + 1);
                uv[vertexLoop + 1] = new Vector2(0.5f, 0.5f);
                uv[vertexLoop + 2] = DrawCircularVerticesForUv(halfAmountOfEdges, .25f, 2.5f + loopCount + 1);

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
            mesh.name = "generated gem mesh";
            mesh.Optimize();
            mesh.RecalculateNormals();
            return mesh;
        }

        private Mesh CreateSmoothMesh(Gem gem)
        {
            List<Vector3> vertexPositions = gem.vertexPositions;
            int halfAmountOfEdges = gem.edges / 2;

            // Initialize variables for vertices logic
            int initialVerticesCount = gem.edges * 3 + 2;
            Vector3[] vertices = new Vector3[initialVerticesCount];
            int vertexLoop = 0;

            // Initialize variables for uv logic
            Vector2[] uv = new Vector2[initialVerticesCount];

            // Initialize variables for triangle logic
            int[] triangles = new int[gem.edges * 12];
            int verticesCount = 0;
            int triangleVerticesCount = 0;

            // Set the vertex of the bottom peak
            vertices[vertexLoop] = vertexPositions[0] - gem.transform.position;
            vertexLoop++;

            // Calculate vertices, uv and triangles for the first part of the pavillon
            for (int loopCount = 0; gem.edges > loopCount; loopCount++)
            {
                int bootomPeakStartIndex = 0;
                int upperPavillonStartIndex = halfAmountOfEdges + 1;

                if (loopCount < halfAmountOfEdges)
                {
                    vertices[vertexLoop + 1] = vertexPositions[upperPavillonStartIndex + loopCount * 2 + 1] - gem.transform.position;
                    vertices[vertexLoop] = vertexPositions[upperPavillonStartIndex + loopCount * 2] - gem.transform.position;


                    uv[bootomPeakStartIndex] = new Vector2(.5f, .5f);
                    uv[vertexLoop] = DrawCircularVerticesForUv(halfAmountOfEdges, .5f, .5f + loopCount * -1f);
                    uv[vertexLoop + 1] = DrawCircularVerticesForUv(halfAmountOfEdges, .5f, 0f + loopCount * -1f);

                    vertexLoop = vertexLoop + 2;
                }

                triangles[triangleVerticesCount] = bootomPeakStartIndex;
                triangles[triangleVerticesCount + 1] = verticesCount + 1;
                triangles[triangleVerticesCount + 2] = verticesCount + 2;
                if (gem.edges - 1 == loopCount)
                {
                    triangles[triangleVerticesCount + 2] = bootomPeakStartIndex + 1;
                }

                triangleVerticesCount += 3;
                verticesCount += 1;
            }

            // Calculate vertices, uv and triangles for the first part of the pavillon
            for (int loopCount = 0; gem.edges > loopCount; loopCount++)
            {
                int crownStartIndex = (halfAmountOfEdges * 3) + 1;
                int upperPavillonStartIndex = halfAmountOfEdges + 1;

                if (loopCount < halfAmountOfEdges)
                {
                    vertices[vertexLoop] = vertexPositions[upperPavillonStartIndex + loopCount * 2] - gem.transform.position;
                    vertices[vertexLoop + 1] = vertexPositions[upperPavillonStartIndex + loopCount * 2 + 1] - gem.transform.position;
                    vertices[gem.edges * 2 + 1 + loopCount] = vertexPositions[crownStartIndex + loopCount] - gem.transform.position;

                    uv[gem.edges * 2 + 1 + loopCount] = DrawCircularVerticesForUv(halfAmountOfEdges, 0.375f, 0.5f + loopCount);
                    uv[vertexLoop] = DrawCircularVerticesForUv(halfAmountOfEdges, .5f, 0.5f + loopCount);
                    uv[vertexLoop + 1] = DrawCircularVerticesForUv(halfAmountOfEdges, .5f, 1f + loopCount);

                    vertexLoop = vertexLoop + 2;
                }

                triangles[triangleVerticesCount] = (gem.edges * 2) + 1 + loopCount / 2;
                triangles[triangleVerticesCount + 2] = verticesCount;
                triangles[triangleVerticesCount + 1] = verticesCount + 1;

                triangleVerticesCount += 3;
                verticesCount += 1;
            }

            vertexLoop = vertexLoop + halfAmountOfEdges;
            verticesCount = vertexLoop;


            // Calculate vertices, uv and triangles for the first part of the pavillon
            for (int loopCount = 0; halfAmountOfEdges > loopCount; loopCount++)
            {
                int upperCrownStartIndex = (halfAmountOfEdges * 4) + 1;
                int crownStartIndex = (halfAmountOfEdges * 3) + 1;
                int upperPavillonStartIndex = halfAmountOfEdges + 1 + loopCount * 2;

                vertices[vertexLoop] = vertexPositions[upperCrownStartIndex + loopCount] - gem.transform.position;
                uv[vertexLoop] = DrawCircularVerticesForUv(halfAmountOfEdges, .25f, 1f + loopCount);
                vertexLoop = vertexLoop + 1;

                triangles[triangleVerticesCount] = gem.edges + 2 + loopCount * 2;
                triangles[triangleVerticesCount + 1] = (gem.edges * 2) + 1 + loopCount;
                triangles[triangleVerticesCount + 2] = verticesCount;
                triangles[triangleVerticesCount + 3] = gem.edges + 2 + loopCount * 2;
                triangles[triangleVerticesCount + 4] = verticesCount;
                triangles[triangleVerticesCount + 5] = (gem.edges * 2) + 2 + loopCount;

                if (halfAmountOfEdges - 1 == loopCount)
                {
                    triangles[triangleVerticesCount + 5] = (gem.edges * 2) + 1;
                }

                triangleVerticesCount += 6;
                verticesCount += 1;
            }

            // Calculate vertices, uv and triangles for the first part of the pavillon
            for (int loopCount = 0; halfAmountOfEdges > loopCount; loopCount++)
            {
                int upperCrownStartIndex = (halfAmountOfEdges * 5) + 1;
                int crownStartIndex = (gem.edges * 2) + 1;

                triangles[triangleVerticesCount] = crownStartIndex + loopCount + 1;
                triangles[triangleVerticesCount + 1] = upperCrownStartIndex + loopCount;
                triangles[triangleVerticesCount + 2] = upperCrownStartIndex + loopCount + 1;


                if (halfAmountOfEdges - 1 == loopCount)
                {
                    triangles[triangleVerticesCount] = crownStartIndex;
                    triangles[triangleVerticesCount + 2] = upperCrownStartIndex;
                }

                triangleVerticesCount += 3;
            }


            // Get the vertex in the middle of the upper plane
            int upperMiddlePlaneStartIndex = (halfAmountOfEdges * 5 + 1);
            vertices[vertexLoop] = vertexPositions[upperMiddlePlaneStartIndex] - gem.transform.position;
            uv[vertexLoop] = DrawCircularVerticesForUv(halfAmountOfEdges, 0f, 0f);

            // Calculate vertices, uv and triangles for the first part of the pavillon
            for (int loopCount = 0; halfAmountOfEdges > loopCount; loopCount++)
            {

                int upperCrownStartIndex = (halfAmountOfEdges * 5) + 1;

                triangles[triangleVerticesCount] = upperCrownStartIndex + loopCount;
                triangles[triangleVerticesCount + 1] = verticesCount;
                triangles[triangleVerticesCount + 2] = upperCrownStartIndex + loopCount + 1;


                if (halfAmountOfEdges - 1 == loopCount)
                {
                    triangles[triangleVerticesCount + 2] = upperCrownStartIndex;
                }

                triangleVerticesCount += 3;
            }



            Mesh mesh = new Mesh();
            mesh.Clear();
            mesh.vertices = vertices;
            mesh.triangles = triangles;
            mesh.uv = uv;
            mesh.name = "generated gem mesh";
            mesh.RecalculateNormals();

            #region Recalculate some normals manually for smoother shading. 
            Vector3[] normals = mesh.normals;

            for (int i = 1; i < gem.edges + 1; i++)
            {
                Vector3 averageNormal = (normals[i] + normals[i + gem.edges]) / 2;
                normals[i] = averageNormal;
                normals[i + gem.edges] = averageNormal;
            }

            mesh.normals = normals;
            #endregion

            mesh.Optimize();
            return mesh;
        }

        private Vector3 DrawCircularVerticesForUv(int edges, float radius, float offset)
        {
            Vector2 uvPosition;
            float degree = (360f / edges);
            degree += (360f / edges) * offset;
            float radian = degree * Mathf.Deg2Rad;
            float x = Mathf.Cos(radian);
            float y = Mathf.Sin(radian);
            uvPosition = new Vector2(.5f, .5f);
            uvPosition = uvPosition + new Vector2(x, y) * radius;
            return uvPosition;
        }
    }
}
