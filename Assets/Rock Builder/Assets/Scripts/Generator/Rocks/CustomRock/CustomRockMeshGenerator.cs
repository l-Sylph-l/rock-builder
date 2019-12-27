using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace RockBuilder
{
    public class CustomRockMeshGenerator
    {
        private static CustomRockMeshGenerator instance = null;
        private static readonly object padlock = new object();

        CustomRockMeshGenerator()
        {
        }

        public static CustomRockMeshGenerator Instance
        {
            get
            {
                lock (padlock)
                {
                    if (instance == null)
                    {
                        instance = new CustomRockMeshGenerator();
                    }
                    return instance;
                }
            }
        }

        public Mesh CreateRockMesh(CustomRock customRock)
        {
            List<List<Vector3>> vertexIteratios = CreateIterations(customRock.rockBuildPoints);
            customRock.sortedVertices = SortVerticesClockwise(vertexIteratios);
            return CreateSmoothMesh(customRock);
        }

        private List<List<Vector3>> CreateIterations(List<Vector3> buildPoints)
        {
            List<List<Vector3>> iterationList = new List<List<Vector3>>();

            float highestBuildPoint = buildPoints[0].y;
            float lowestBuildPoint = buildPoints[0].y;

            foreach (Vector3 buildPoint in buildPoints)
            {
                if (buildPoint.y > highestBuildPoint)
                {
                    highestBuildPoint = buildPoint.y;
                }

                if (buildPoint.y < lowestBuildPoint)
                {
                    lowestBuildPoint = buildPoint.y;
                }
            }

            float meshHeight = Mathf.Abs(highestBuildPoint - lowestBuildPoint);
            float iterationScanHeight = meshHeight / 6;


            for (int loopCount = 0; loopCount < 6; loopCount++)
            {
                List<Vector3> iteration = new List<Vector3>();
                float startHieght = lowestBuildPoint + iterationScanHeight * loopCount;
                float endHeight = lowestBuildPoint + iterationScanHeight * (loopCount + 1);

                foreach (Vector3 buildPoint in buildPoints)
                {
                    if (buildPoint.y >= startHieght && buildPoint.y <= endHeight)
                    {
                        iteration.Add(buildPoint);
                    }
                }

                if (iteration.Count != 0)
                {
                    iterationList.Add(iteration);
                }

            }

            return iterationList;
        }

        private List<CustomRockListIteration> SortVerticesClockwise(List<List<Vector3>> unsortedVertexPositions)
        {
            List<CustomRockListIteration> sortedVertexPositions = new List<CustomRockListIteration>();

            foreach (List<Vector3> iteration in unsortedVertexPositions)
            {
                CustomRockListIteration sortedIteration = new CustomRockListIteration(iteration);

                Vector3 centerPoint = sortedIteration.GetCenterPoint();

                foreach (Vector3 vertexPosition in iteration)
                {
                    // from 0 to 3 o'clock
                    if (vertexPosition.x > centerPoint.x && vertexPosition.z > centerPoint.z)
                    {
                        sortedIteration.firstQuarter.Add(vertexPosition);
                    }
                    // from 3 to 6 o'clock
                    if (vertexPosition.x > centerPoint.x && vertexPosition.z < centerPoint.z)
                    {
                        sortedIteration.secondQuarter.Add(vertexPosition);
                    }
                    // from 6 to 9 o'clock
                    if (vertexPosition.x < centerPoint.x && vertexPosition.z < centerPoint.z)
                    {
                        sortedIteration.thirdQuarter.Add(vertexPosition);
                    }
                    // from 9 to 12 o'clock
                    if (vertexPosition.x < centerPoint.x && vertexPosition.z > centerPoint.z)
                    {
                        sortedIteration.fourthQuarter.Add(vertexPosition);
                    }
                }

                sortedIteration.Sort();

                sortedVertexPositions.Add(sortedIteration);
            }

            return sortedVertexPositions;
        }

        private void equalizeVertexCount(List<CustomRockListIteration> sortedVertexPositions)
        {
            int highestCount = 0;
            foreach (CustomRockListIteration iteration in sortedVertexPositions)
            {
                highestCount = iteration.GetHighestCount();
            }

            foreach (CustomRockListIteration iteration in sortedVertexPositions)
            {
                iteration.EqualizeVertexCount(highestCount);
            }
        }

        private Mesh CreateSmoothMesh(CustomRock rockBuildData)
        {

            //List<Vector3> vertexPositions = rockBuildData.GetVertexCount();

            int vrticesCount = rockBuildData.GetVertexCount();
            Vector3[] vertices = new Vector3[vrticesCount];
            Vector2[] uv = new Vector2[vrticesCount];
            int vertexLoop = 0;

            foreach (CustomRockListIteration iterationList in rockBuildData.sortedVertices)
            {
                foreach (Vector3 vertex in iterationList.GetSortedVertexList())
                {
                    vertices[vertexLoop] = vertex;
                    vertexLoop++;
                }
            }

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