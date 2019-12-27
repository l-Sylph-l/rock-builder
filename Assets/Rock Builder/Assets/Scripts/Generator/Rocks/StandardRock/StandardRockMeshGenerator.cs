using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace RockBuilder
{
    public class StandardRockMeshGenerator
    {
        private static StandardRockMeshGenerator instance = null;
        private static readonly object padlock = new object();

        StandardRockMeshGenerator()
        {
        }

        public static StandardRockMeshGenerator Instance
        {
            get
            {
                lock (padlock)
                {
                    if (instance == null)
                    {
                        instance = new StandardRockMeshGenerator();
                    }
                    return instance;
                }
            }
        }

        public Mesh CreateRockMesh(StandardRock standardRock)
        {
            return CreateSmoothMesh(standardRock);
        }

        private Mesh CreateSmoothMesh(StandardRock rockBuildData)
        {

            //List<Vector3> vertexPositions = rockBuildData.GetVertexCount();

            int vrticesCount = rockBuildData.GetVertexCount();
            Vector3[] vertices = new Vector3[vrticesCount];
            Vector2[] uv = new Vector2[vrticesCount];
            int vertexLoop = 0;

            //    for (int loopCount = 0; rockBuildData.sortedVertices.Count > loopCount; loopCount++)
            //{
            //    foreach(Vector3 vertex in rockBuildData.sortedVertices[loopCount].GetSortedVertexList())
            //    {
            //        vertices[vertexLoop] = vertex;
            //        vertexLoop++;
            //    }
            //}

            int[] triangles = new int[rockBuildData.GetVertexCount() * 6];
            int verticesCount = 0;
            int triangleVerticesCount = 0;
            int vertexCountPerIteration = rockBuildData.sortedVertices[0].GetVertexCount();

            for (int literationCount = 0; literationCount < rockBuildData.sortedVertices.Count; literationCount++)
            {
                int finalIteration = rockBuildData.sortedVertices.Count-1;
                if (literationCount != finalIteration)
                {
                    for (int vertexCount = 0; vertexCount < vertexCountPerIteration; vertexCount++)
                    {
                        int finalLoop = vertexCountPerIteration - 1;
                        int nextVertex = verticesCount + 1;
                        int firstVertexFromNextIteration = verticesCount + vertexCountPerIteration;
                        int secondVertexFromNextIteration = verticesCount + vertexCountPerIteration + 1;

                        triangles[triangleVerticesCount] = verticesCount;
                        triangles[triangleVerticesCount + 1] = firstVertexFromNextIteration;
                        triangles[triangleVerticesCount + 2] = nextVertex;
                        triangles[triangleVerticesCount + 3] = firstVertexFromNextIteration;
                        triangles[triangleVerticesCount + 4] = secondVertexFromNextIteration;
                        triangles[triangleVerticesCount + 5] = nextVertex;

                        if (vertexCount == finalLoop)
                        {
                            triangles[triangleVerticesCount + 2] = nextVertex - vertexCountPerIteration;
                            triangles[triangleVerticesCount + 4] = secondVertexFromNextIteration - vertexCountPerIteration;
                            triangles[triangleVerticesCount + 5] = nextVertex - vertexCountPerIteration;
                        }

                        verticesCount += 1;
                        triangleVerticesCount += 6;
                    }
                }
            }

         

            Mesh mesh = new Mesh();
            mesh.Clear();
            mesh.vertices = vertices;
            mesh.triangles = triangles;
            mesh.uv = uv;
            mesh.name = "generated diamond mesh";
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
    }
}