using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace RockBuilder
{
    public class SphereRockMeshGenerator
    {
        private static SphereRockMeshGenerator instance = null;
        private static readonly object padlock = new object();

        private int triangleVerticesCount;
        private int vertexLoop;
        Vector3[] vertices;
        Vector2[] uv;
        int[] triangles;
        float noiseFactor;
        SphereRockMeshGenerator()
        {
        }

        public static SphereRockMeshGenerator Instance
        {
            get
            {
                lock (padlock)
                {
                    if (instance == null)
                    {
                        instance = new SphereRockMeshGenerator();
                    }
                    return instance;
                }
            }
        }

        public List<List<Vector3>> CreateVertexPositions(SphereRock sphereRock)
        {
            List<List<Vector3>> circularIerationList = new List<List<Vector3>>();

            float startPositionY = -sphereRock.height / 2;
            int edges = 6;


            float radiusX = sphereRock.width;
            float radiusY = sphereRock.height;
            float radiusZ = sphereRock.depth;


            float positionZ = radiusZ / 2;


            List<Vector3> circularIeration = new List<Vector3>();
            // Get the vertices for the body
            for (int loopCount = 0; edges > loopCount; loopCount++)
            {
                int halfCircleFactor = (edges * 2) - 2;
                Vector3 spawnPoint = DrawCircularVerticesXY(sphereRock, radiusX, radiusY, positionZ, halfCircleFactor, loopCount);
                circularIeration.Add(spawnPoint);
            }

            foreach (Vector3 vertex in circularIeration)
            {
                List<Vector3> circularIerationY = new List<Vector3>();
                for (int loopCount = 0; edges > loopCount; loopCount++)
                {
                    float multiplyFactor = vertex.x / radiusX;
                    float newRadiusX = radiusX * multiplyFactor;
                    float newRadiusZ = radiusZ * multiplyFactor;
                    float positionY = vertex.y;
                    Vector3 spawnPoint = DrawCircularVerticesXZ(sphereRock, newRadiusX, newRadiusZ, positionY, edges, loopCount);
                    circularIerationY.Add(spawnPoint);
                }
                circularIerationList.Add(circularIerationY);
            }

            return circularIerationList;
        }

        private Vector3 DrawCircularVerticesXZ(SphereRock sphereRock, float radiusX, float radiusZ, float positionY, int edges, int loopCount)
        {
            Vector3 spawnPoint;
            float degree = (360f / edges) * loopCount;
            // if (offset)
            // {
            //     degree += (360f / edges) / 2;
            // }
            float radian = degree * Mathf.Deg2Rad;
            float x = Mathf.Cos(radian) * radiusX;
            float z = Mathf.Sin(radian) * radiusZ;
            spawnPoint = new Vector3(x, 0, z);
            spawnPoint.y = positionY;
            //spawnPoint += sphereRock.transform.position;
            return spawnPoint;
        }

        private Vector3 DrawCircularVerticesXY(SphereRock sphereRock, float radiusX, float radiusY, float positionZ, int edges, int loopCount)
        {
            Vector3 spawnPoint;
            float degree = (360f / edges) * loopCount + 90f;
            // if (offset)
            // {
            //     degree += (360f / edges) / 2;
            // }
            float radian = degree * Mathf.Deg2Rad;
            float x = Mathf.Cos(radian) * radiusX;
            float y = Mathf.Sin(radian) * radiusY;
            spawnPoint = new Vector3(x, y, 0);
            spawnPoint.z = positionZ;

            return spawnPoint;
        }


        public Mesh CreateRockMesh(SphereRock sphereRock)
        {
            return CreateSmoothMesh(sphereRock);
        }

        private Mesh CreateSmoothMesh(SphereRock sphereRock)
        {
            List<List<Vector3>> circularIterations = sphereRock.vertexPositions;
            int verticesPerIteration = circularIterations.Count;
            vertexLoop = 0;
            triangleVerticesCount = 0;
            int verticesCount = verticesPerIteration * verticesPerIteration;
            vertices = new Vector3[verticesCount];
            uv = new Vector2[verticesCount];
            triangles = new int[verticesCount * 5];
            noiseFactor = sphereRock.noise;

            int iterationCount = 0;

            foreach (List<Vector3> iteration in circularIterations)
            {
                iterationCount++;
                float uvHeightIteration = (1f / circularIterations.Count) * iterationCount;
                int vertexCount = 1;
                foreach (Vector3 vertex in iteration)
                {
                    float uvWidthIteration = (1f / circularIterations.Count) * vertexCount / 1;
                    vertices[vertexLoop] = vertex;
                    uv[vertexLoop] = new Vector2(uvWidthIteration, uvHeightIteration);

                    if (iterationCount != circularIterations.Count)
                    {
                        triangles[triangleVerticesCount] = vertexLoop;
                        triangles[triangleVerticesCount + 1] = vertexLoop + 1;
                        triangles[triangleVerticesCount + 2] = vertexLoop + verticesPerIteration;
                        triangles[triangleVerticesCount + 3] = vertexLoop + 1;
                        triangles[triangleVerticesCount + 4] = vertexLoop + verticesPerIteration + 1;
                        triangles[triangleVerticesCount + 5] = vertexLoop + verticesPerIteration;

                        if (vertexCount == verticesPerIteration)
                        {
                            triangles[triangleVerticesCount + 1] = vertexLoop + 1 - verticesPerIteration;
                            triangles[triangleVerticesCount + 3] = vertexLoop + 1 - verticesPerIteration;
                            triangles[triangleVerticesCount + 4] = vertexLoop + 1;
                        }

                        triangleVerticesCount += 6;

                    }

                    vertexLoop++;
                    vertexCount++;
                }
            }

            Mesh mesh = new Mesh();
            mesh.Clear();
            mesh.vertices = vertices;
            mesh.triangles = triangles;
            mesh.uv = uv;
            mesh.name = "generated sphere mesh";
            mesh.RecalculateNormals();

            //#region Recalculate some normals manually for smoother shading. 
            //Vector3[] normals = mesh.normals;

            //Vector3 averageNormal1 = (normals[0] + normals[(crystal.edges * 2)]) / 2;
            //normals[0] = averageNormal1;
            //normals[(crystal.edges * 2)] = averageNormal1;

            //Vector3 averageNormal2 = (normals[1] + normals[(crystal.edges * 2) + 1]) / 2;
            //normals[1] = averageNormal2;
            //normals[(crystal.edges * 2) + 1] = averageNormal2;

            //for (int i = 1; i < crystal.edges + 1; i++)
            //{
            //    normals[normals.Length - i] = new Vector3(0f, 1f, 0f);
            //    normals[normals.Length - i - crystal.edges] = new Vector3(0f, -1f, 0f);
            //}

            //mesh.normals = normals;
            //#endregion


            mesh.Optimize();
            return mesh;
        }

        private Vector3 GetMiddlePoint(SphereRock sphereRock, float positionY)
        {
            return sphereRock.transform.position + (Vector3.up * positionY);
        }
    }
}