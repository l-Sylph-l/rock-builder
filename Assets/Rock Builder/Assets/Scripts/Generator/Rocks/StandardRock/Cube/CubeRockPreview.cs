using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RockBuilder
{
    public class CubeRockPreview
    {
        private static CubeRockPreview instance = null;
        private static readonly object padlock = new object();

        CubeRockPreview()
        {
        }

        public static CubeRockPreview Instance
        {
            get
            {
                lock (padlock)
                {
                    if (instance == null)
                    {
                        instance = new CubeRockPreview();
                    }
                    return instance;
                }
            }
        }

        public void DrawLines(CubeRock cubeRock)
        {
            Gizmos.color = Color.blue;
            Gizmos.matrix = cubeRock.transform.localToWorldMatrix;

            int verticalBezelLoopCount = 0;
            int bottomLoopCount = 1;
            int linkLoopCount = 0;
            foreach (var vertex in cubeRock.bottomCornerVertices)
            {
                Vector3 vertexFrom = vertex;
                Vector3 vertexTo;
                Vector3 firstVerticalBezelTo = cubeRock.bottomVerticalBezelsVertices[verticalBezelLoopCount];
                Vector3 secondVerticalBezelTo;

                if (verticalBezelLoopCount == 0)
                {
                    secondVerticalBezelTo = cubeRock.bottomVerticalBezelsVertices[7];
                } else {
                    secondVerticalBezelTo = cubeRock.bottomVerticalBezelsVertices[verticalBezelLoopCount - 1];
                }

                if (bottomLoopCount == 4)
                {
                    vertexTo = cubeRock.bottomCornerVertices[0];
                }
                else
                {
                    vertexTo = cubeRock.bottomCornerVertices[bottomLoopCount];
                }

                Gizmos.DrawLine(vertexFrom, vertexTo);
                Gizmos.DrawLine(vertexFrom, firstVerticalBezelTo);
                Gizmos.DrawLine(vertexFrom, secondVerticalBezelTo);

                bottomLoopCount++;
                verticalBezelLoopCount += 2;
            }

            verticalBezelLoopCount = 0;
            int upperLoopCount = 1;
            foreach (var vertex in cubeRock.upperCornerVertices)
            {
                Vector3 vertexFrom = vertex;
                Vector3 vertexTo;
                Vector3 firstVerticalBezelTo = cubeRock.upperVerticalBezelsVertices[verticalBezelLoopCount];
                Vector3 secondVerticalBezelTo;

                if (verticalBezelLoopCount == 0)
                {
                    secondVerticalBezelTo = cubeRock.upperVerticalBezelsVertices[7];
                } else {
                    secondVerticalBezelTo = cubeRock.upperVerticalBezelsVertices[verticalBezelLoopCount - 1];
                }

                if (upperLoopCount == 4)
                {
                    vertexTo = cubeRock.upperCornerVertices[0];
                }
                else
                {
                    vertexTo = cubeRock.upperCornerVertices[upperLoopCount];
                }

                Gizmos.DrawLine(vertexFrom, vertexTo);
                Gizmos.DrawLine(vertexFrom, firstVerticalBezelTo);
                Gizmos.DrawLine(vertexFrom, secondVerticalBezelTo);

                upperLoopCount++;
                verticalBezelLoopCount += 2;
            }


            if (cubeRock.bezelSize == 0f)
            {
                bottomLoopCount = 1;
                linkLoopCount = 0;
                foreach (var vertex in cubeRock.bottomBezelsVertices)
                {
                    Vector3 vertexFrom = vertex;

                    Vector3 upperVertexTo = cubeRock.upperBezelsVertices[linkLoopCount];

                    Gizmos.DrawLine(vertexFrom, upperVertexTo);

                    linkLoopCount++;
                    bottomLoopCount++;
                }
            }

            verticalBezelLoopCount = 0;
            foreach (var vertex in cubeRock.upperVerticalBezelsVertices)
            {
                Vector3 vertexFrom = vertex;
                Vector3 vertexTo = cubeRock.bottomVerticalBezelsVertices[verticalBezelLoopCount];

                Gizmos.DrawLine(vertexFrom, vertexTo);

                verticalBezelLoopCount++;
            }

            verticalBezelLoopCount = 1;
            foreach (var vertex in cubeRock.upperVerticalBezelsVertices)
            {
                Vector3 vertexFrom = vertex;
                Vector3 vertexTo;

                if (verticalBezelLoopCount == 8)
                {
                    vertexTo = cubeRock.upperVerticalBezelsVertices[0];
                }
                else
                {
                    vertexTo = cubeRock.upperVerticalBezelsVertices[verticalBezelLoopCount];
                }


                Gizmos.DrawLine(vertexFrom, vertexTo);

                verticalBezelLoopCount++;
            }

            verticalBezelLoopCount = 1;
            foreach (var vertex in cubeRock.bottomVerticalBezelsVertices)
            {
                Vector3 vertexFrom = vertex;
                Vector3 vertexTo;

                if (verticalBezelLoopCount == 8)
                {
                    vertexTo = cubeRock.bottomVerticalBezelsVertices[0];
                }
                else
                {
                    vertexTo = cubeRock.bottomVerticalBezelsVertices[verticalBezelLoopCount];
                }

                Gizmos.DrawLine(vertexFrom, vertexTo);

                verticalBezelLoopCount++;
            }
        }

        public void DrawGizmo(CubeRock cubeRock)
        {

            DrawLines(cubeRock);

            // Draw black cubes on every vertex position of the cubeRock
            foreach (Vector3 spawnPosition in cubeRock.bottomCornerVertices)
            {
                VisualizeVertex(spawnPosition, cubeRock);
            }

            foreach (Vector3 spawnPosition in cubeRock.bottomVerticalBezelsVertices)
            {
                VisualizeVertex(spawnPosition, cubeRock);
            }

            foreach (Vector3 spawnPosition in cubeRock.upperCornerVertices)
            {
                VisualizeVertex(spawnPosition, cubeRock);
            }

            foreach (Vector3 spawnPosition in cubeRock.upperVerticalBezelsVertices)
            {
                VisualizeVertex(spawnPosition, cubeRock);
            }

            if (cubeRock.bezelSize == 0f)
            {
                foreach (Vector3 spawnPosition in cubeRock.bottomBezelsVertices)
                {
                    VisualizeVertex(spawnPosition, cubeRock);
                }

                foreach (Vector3 spawnPosition in cubeRock.upperBezelsVertices)
                {
                    VisualizeVertex(spawnPosition, cubeRock);
                }
            }
        }

        private void VisualizeVertex(Vector3 vertex, CubeRock cubeRock)
        {
            Gizmos.color = Color.black;
            float scaleModeModifier = 10f;
            float cubeSize = Mathf.Clamp(0.05f / scaleModeModifier, 0.025f, 0.3f);
            Gizmos.DrawCube(vertex, new Vector3(cubeSize, cubeSize, cubeSize));
            Gizmos.color = Color.blue;
        }


    }
}
