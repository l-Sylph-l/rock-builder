using UnityEngine;
using System.Collections.Generic;


namespace RockBuilder
{
    public class CrystalMeshGenerator
    {
        private static CrystalMeshGenerator instance = null;
        private static readonly object padlock = new object();

        CrystalMeshGenerator()
        {
        }

        public static CrystalMeshGenerator Instance
        {
            get
            {
                lock (padlock)
                {
                    if (instance == null)
                    {
                        instance = new CrystalMeshGenerator();
                    }
                    return instance;
                }
            }
        }

        public List<Vector3> CreateVertexPositions(Crystal crystal)
        {
            List<Vector3> spawnPoints = new List<Vector3>();

            // Get the points for the bottom circle
            for (int loopCount = 0; crystal.edges > loopCount; loopCount++)
            {
                Vector3 spawnPoint = DrawCircularVertices(crystal, -crystal.height, loopCount);
                spawnPoints.Add(spawnPoint);
            }

            // Get the points for the upper circle
            for (int loopCount = 0; crystal.edges > loopCount; loopCount++)
            {
                Vector3 spawnPoint = DrawCircularVertices(crystal, crystal.height, loopCount);
                spawnPoints.Add(spawnPoint);
            }

            // Get the points for the upper and bottom peak
            spawnPoints.Add(crystal.transform.position + (Vector3.up * (crystal.height / 2 + crystal.heightPeak)));
            spawnPoints.Add(crystal.transform.position - (Vector3.up * (crystal.height / 2 + crystal.heightPeak)));

            return spawnPoints;
        }

        private Vector3 DrawCircularVertices(Crystal crystal, float height, int loopCount)
        {
            Vector3 spawnPoint;
            float degree = (360f / crystal.edges) * loopCount;
            float radian = degree * Mathf.Deg2Rad;
            float x = Mathf.Cos(radian);
            float z = Mathf.Sin(radian);
            spawnPoint = new Vector3(x, 0, z) * crystal.radius;
            spawnPoint.y = height / 2;
            spawnPoint += crystal.transform.position;
            return spawnPoint;
        }

        public Mesh CreateMesh(Crystal crystal)
        {
            if (crystal.smooth)
            {
                return CreateSmoothMesh(crystal);
            }
            else
            {
                return CreateHardMesh(crystal);
            }

            //CreateLods(crystal);
        }

        private Mesh CreateHardMesh(Crystal crystal)
        {
            List<Vector3> vertexPositions = crystal.vertexPositions;

            Vector3[] vertices = new Vector3[crystal.edges * 6];
            Vector2[] uv = new Vector2[crystal.edges * 6];
            int vertexLoop = 0;
            float circumference = Vector3.Distance(vertexPositions[0], vertexPositions[1]) * crystal.edges;
            float uvHeightBody = (1f - (crystal.height / circumference)) / 2;
            float uvHeightTotal = (1f - ((crystal.heightPeak * 2 + crystal.height) / circumference)) / 4;

            for (int loopCount = 0; crystal.edges > loopCount; loopCount++)
            {
                if (crystal.edges - 1 != loopCount)
                {
                    vertices[vertexLoop] = vertexPositions[crystal.edges + loopCount] - crystal.transform.position;
                    vertices[vertexLoop + 1] = vertexPositions[loopCount] - crystal.transform.position;
                    vertices[vertexLoop + 2] = vertexPositions[loopCount + 1] - crystal.transform.position; ;
                    vertices[vertexLoop + 3] = vertexPositions[crystal.edges + loopCount + 1] - crystal.transform.position;

                    if (loopCount == 0)
                    {
                        uv[vertexLoop] = new Vector2(0f, 1f - uvHeightBody);
                        uv[vertexLoop + 1] = new Vector2(0f, uvHeightBody);
                        uv[vertexLoop + 2] = new Vector2(((float)loopCount + 1f) / (float)crystal.edges, uvHeightBody);
                        uv[vertexLoop + 3] = new Vector2(((float)loopCount + 1f) / (float)crystal.edges, 1f - uvHeightBody);
                    }
                    else
                    {
                        uv[vertexLoop] = new Vector2((float)loopCount / (float)crystal.edges, 1f - uvHeightBody);
                        uv[vertexLoop + 1] = new Vector2((float)loopCount / (float)crystal.edges, uvHeightBody);
                        uv[vertexLoop + 2] = new Vector2(((float)loopCount + 1f) / (float)crystal.edges, uvHeightBody);
                        uv[vertexLoop + 3] = new Vector2(((float)loopCount + 1f) / (float)crystal.edges, 1f - uvHeightBody);
                    }

                    vertexLoop = vertexLoop + 4;

                }
                else
                {
                    vertices[vertexLoop] = vertexPositions[crystal.edges + loopCount] - crystal.transform.position;
                    vertices[vertexLoop + 1] = vertexPositions[loopCount] - crystal.transform.position;
                    vertices[vertexLoop + 2] = vertexPositions[0] - crystal.transform.position;
                    vertices[vertexLoop + 3] = vertexPositions[crystal.edges] - crystal.transform.position;

                    uv[vertexLoop] = new Vector2((float)loopCount / (float)crystal.edges, 1f - uvHeightBody);
                    uv[vertexLoop + 1] = new Vector2((float)loopCount / (float)crystal.edges, uvHeightBody);
                    uv[vertexLoop + 2] = new Vector2(((float)loopCount + 1f) / (float)crystal.edges, uvHeightBody);
                    uv[vertexLoop + 3] = new Vector2(((float)loopCount + 1f) / (float)crystal.edges, 1f - uvHeightBody);

                    vertexLoop = vertexLoop + 4;
                }
            }

            // Get the vertices for both peaks
            for (int loopCount = 0; crystal.edges > loopCount; loopCount++)
            {
                vertices[vertexLoop] = vertexPositions[(crystal.edges * 2) + 1] - crystal.transform.position;
                uv[vertexLoop] = new Vector2(((float)loopCount / (float)crystal.edges) + 1f / (float)crystal.edges / 2f, uvHeightTotal);
                vertexLoop++;
            }

            for (int loopCount = 0; crystal.edges > loopCount; loopCount++)
            {
                vertices[vertexLoop] = vertexPositions[(crystal.edges * 2)] - crystal.transform.position;
                uv[vertexLoop] = new Vector2(((float)loopCount / (float)crystal.edges) + 1f / (float)crystal.edges / 2f, 1f - uvHeightTotal);
                vertexLoop++;
            }

            int[] triangles = new int[crystal.edges * 12];
            int loopCountBody = crystal.edges * 4;
            int loopCountPeak = crystal.edges * 2;
            int verticesCount = 0;
            int triangleVerticesCount = 0;

            for (int i = 0; verticesCount < loopCountBody; i = i + 2)
            {
                triangles[triangleVerticesCount] = i;
                triangles[triangleVerticesCount + 1] = i = i + 2;
                triangles[triangleVerticesCount + 2] = i = i - 1;
                triangles[triangleVerticesCount + 3] = i = i - 1;
                triangles[triangleVerticesCount + 4] = i = i + 3;
                triangles[triangleVerticesCount + 5] = i = i - 1;

                triangleVerticesCount += 6;
                verticesCount += 4;
            }

            for (int i = 0; 0 < loopCountPeak; i = i + 2)
            {
                triangles[triangleVerticesCount] = verticesCount + crystal.edges;
                triangles[triangleVerticesCount + 1] = i = i + 3;
                triangles[triangleVerticesCount + 2] = i = i - 3;
                triangles[triangleVerticesCount + 3] = verticesCount;
                triangles[triangleVerticesCount + 4] = i = i + 1;
                triangles[triangleVerticesCount + 5] = i = i + 1;

                triangleVerticesCount += 6;
                verticesCount++;
                loopCountPeak -= 2;
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

        private Mesh CreateSmoothMesh(Crystal crystal)
        {

            List<Vector3> vertexPositions = crystal.vertexPositions;

            int vrticesCount = (crystal.edges * 4) + 2;
            Vector3[] vertices = new Vector3[vrticesCount];
            Vector2[] uv = new Vector2[vrticesCount];
            int vertexLoop = 0;
            float circumference = Vector3.Distance(vertexPositions[0], vertexPositions[1]) * crystal.edges;
            float uvHeightBody = (1f - (crystal.height / circumference)) / 2;
            float uvHeightTotal = (1f - ((crystal.heightPeak * 2 + crystal.height) / circumference)) / 4;

            for (int loopCount = 0; crystal.edges > loopCount; loopCount++)
            {
                vertices[vertexLoop] = vertexPositions[crystal.edges + loopCount] - crystal.transform.position;
                vertices[vertexLoop + 1] = vertexPositions[loopCount] - crystal.transform.position;

                uv[vertexLoop] = new Vector2((float)loopCount / (float)crystal.edges, 1f - uvHeightBody);
                uv[vertexLoop + 1] = new Vector2(((float)loopCount) / (float)crystal.edges, uvHeightBody);

                vertexLoop = vertexLoop + 2;
            }

            vertices[vertexLoop] = vertexPositions[crystal.edges] - crystal.transform.position;
            vertices[vertexLoop + 1] = vertexPositions[0] - crystal.transform.position;
            uv[vertexLoop] = new Vector2(1f, 1f - uvHeightBody);
            uv[vertexLoop + 1] = new Vector2(1f, uvHeightBody);
            vertexLoop = vertexLoop + 2;

            // Get the vertices for both peaks
            for (int loopCount = 0; crystal.edges > loopCount; loopCount++)
            {
                vertices[vertexLoop] = vertexPositions[(crystal.edges * 2) + 1] - crystal.transform.position;
                uv[vertexLoop] = new Vector2(((float)loopCount / (float)crystal.edges) + 1f / (float)crystal.edges / 2f, uvHeightTotal);
                vertexLoop++;
            }

            for (int loopCount = 0; crystal.edges > loopCount; loopCount++)
            {
                vertices[vertexLoop] = vertexPositions[(crystal.edges * 2)] - crystal.transform.position;
                uv[vertexLoop] = new Vector2(((float)loopCount / (float)crystal.edges) + 1f / (float)crystal.edges / 2f, 1f - uvHeightTotal);
                vertexLoop++;
            }

            #region Draw triangles 
            int[] triangles = new int[(crystal.edges * 12) + 6];
            int loopCountBody = (crystal.edges * 2) + 2;
            int loopCountPeak = crystal.edges * 2;
            int verticesCount = 0;
            int triangleVerticesCount = 0;

            for (int i = 0; verticesCount < loopCountBody; i = i - 1)
            {
                triangles[triangleVerticesCount] = i;
                triangles[triangleVerticesCount + 1] = i = i + 3;
                triangles[triangleVerticesCount + 2] = i = i - 2;
                triangles[triangleVerticesCount + 3] = i = i - 1;
                triangles[triangleVerticesCount + 4] = i = i + 2;
                triangles[triangleVerticesCount + 5] = i = i + 1;

                if (verticesCount == loopCountBody - 2)
                {
                    triangles[triangleVerticesCount + 1] = 1;
                    triangles[triangleVerticesCount + 4] = 0;
                    triangles[triangleVerticesCount + 5] = 1;
                }

                triangleVerticesCount += 6;
                verticesCount += 2;
            }

            for (int i = 0; 0 < loopCountPeak; i = i - 1)
            {
                triangles[triangleVerticesCount] = verticesCount + crystal.edges;
                triangles[triangleVerticesCount + 1] = i = i + 2;
                triangles[triangleVerticesCount + 2] = i = i - 2;
                triangles[triangleVerticesCount + 3] = verticesCount;
                triangles[triangleVerticesCount + 4] = i = i + 1;
                triangles[triangleVerticesCount + 5] = i = i + 2;

                if (loopCountPeak == 2)
                {
                    triangles[triangleVerticesCount + 1] = crystal.edges * 2;
                    triangles[triangleVerticesCount + 5] = (crystal.edges * 2) + 1;
                }

                triangleVerticesCount += 6;
                verticesCount++;
                loopCountPeak -= 2;
            }
            #endregion

            Mesh mesh = new Mesh();
            mesh.Clear();
            mesh.vertices = vertices;
            mesh.triangles = triangles;
            mesh.uv = uv;
            mesh.name = "generated diamond mesh";
            mesh.RecalculateNormals();


            #region Recalculate some normals manually for smoother shading. 
            Vector3[] normals = mesh.normals;

            Vector3 averageNormal1 = (normals[0] + normals[(crystal.edges * 2)]) / 2;
            normals[0] = averageNormal1;
            normals[(crystal.edges * 2)] = averageNormal1;

            Vector3 averageNormal2 = (normals[1] + normals[(crystal.edges * 2) + 1]) / 2;
            normals[1] = averageNormal2;
            normals[(crystal.edges * 2) + 1] = averageNormal2;

            for (int i = 1; i < crystal.edges + 1; i++)
            {
                normals[normals.Length - i] = new Vector3(0f, 1f, 0f);
                normals[normals.Length - i - crystal.edges] = new Vector3(0f, -1f, 0f);
            }

            mesh.normals = normals;
            #endregion
            mesh.Optimize();
            return mesh;
        }

        //public void CreateLods(Crystal crystal)
        //{
        //    if (crystal.childrens != null)
        //    {
        //        crystal.RemoveLOD();
        //    }

        //    int lodCounter = crystal.lodCount;

        //    if (lodCounter != 0 && 3 <= crystal.edges / crystal.lodCount)
        //    {
        //        // Programmatically create a LOD group and add LOD levels.
        //        // Create a GUI that allows for forcing a specific LOD level.
        //        lodCounter += 1;
        //        LODGroup group = crystal.gameObject.AddComponent<LODGroup>();
        //        childrens = new Transform[lodCounter - 1];

        //        // Add 4 LOD levels
        //        LOD[] lods = new LOD[lodCounter];
        //        for (int i = 0; i < lodCounter; i++)
        //        {

        //            Renderer[] renderers;
        //            Crystal childCrystal;

        //            if (i != 0)
        //            {
        //                childCrystal = new GameObject().AddComponent(typeof(Crystal)) as Crystal;
        //                if (crystal.smooth)
        //                {
        //                    childCrystal.edges = crystal.edges / (i + 1);
        //                    CreateSmoothMesh(crystal);
        //                }
        //                else
        //                {
        //                    CreateHardMesh(crystal);
        //                }

        //                childCrystal.name = crystal.name + "_LOD_0" + i;
        //                childCrystal.transform.parent = crystal.transform;
        //                childCrystal.transform.localPosition = new Vector3(0f, 0f, 0f);
        //                renderers = new Renderer[1];
        //                renderers[0] = crystal.GetComponent<Renderer>();
        //                childrens[i - 1] = crystal.transform;
        //                DestroyImmediate(crystal.GetComponent<CrystalMeshGenerator>());
        //            }
        //            else
        //            {
        //                renderers = new Renderer[1];
        //                renderers[0] = GetComponent<Renderer>();
        //            }

        //            if (i != lodCount - 1)
        //            {
        //                lods[i] = new LOD((1f / lodCount) * (lodCount - i - 1) / 2, renderers);
        //            }
        //            else
        //            {
        //                lods[i] = new LOD(0f, renderers);

        //            }

        //        }
        //        group.SetLODs(lods);
        //        group.RecalculateBounds();
        //    }
        //}

        //private void DestroyLOD()
        //{
        //    if (GetComponent<LODGroup>() != null)
        //    {
        //        DestroyImmediate(GetComponent<LODGroup>());
        //    }

        //    foreach (Transform child in childrens)
        //    {
        //        if (child != null)
        //        {
        //            DestroyImmediate(child.gameObject);
        //        }
        //    }
        //}

        public MeshCollider CreateCollider(Mesh mesh)
        {
            MeshCollider meshCollider = new MeshCollider();
            if (mesh == null)
            {
                meshCollider.sharedMesh = mesh;
                meshCollider.convex = true;
            }
            return meshCollider;
        }
    }
}

