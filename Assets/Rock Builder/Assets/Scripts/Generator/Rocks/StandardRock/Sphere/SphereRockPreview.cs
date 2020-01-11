using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RockBuilder
{
    public class SphereRockPreview
    {
        private static SphereRockPreview instance = null;
        private static readonly object padlock = new object();

        SphereRockPreview()
        {
        }

        public static SphereRockPreview Instance
        {
            get
            {
                lock (padlock)
                {
                    if (instance == null)
                    {
                        instance = new SphereRockPreview();
                    }
                    return instance;
                }
            }
        }

        public void DrawLines(SphereRock sphereRock)
        {
            Gizmos.color = Color.blue;

            Vector3 vertexFrom = Vector3.zero;

            foreach (List<Vector3> iteration in sphereRock.vertexPositions)
            {

                bool skipFirstVertex = true;
                foreach (Vector3 vertex in iteration)
                {
                    if (!skipFirstVertex)
                    {
                        Gizmos.DrawLine(vertexFrom, vertex);
                    }

                    vertexFrom = vertex;
                    skipFirstVertex = false;



                }
            }

            for (int loopCount = 0; loopCount < sphereRock.vertexPositions.Count; loopCount++)
            {

                for (int innerLoopCount = 0; innerLoopCount < sphereRock.vertexPositions[loopCount].Count; innerLoopCount++)
                {
                    Vector3 verticalVertexFrom = sphereRock.vertexPositions[innerLoopCount][loopCount];
                    Vector3 verticalVertexTo;

                    if (sphereRock.vertexPositions.Count - 1 != innerLoopCount)
                    {
                        verticalVertexTo = sphereRock.vertexPositions[innerLoopCount + 1][loopCount];
                    }
                    else
                    {
                        verticalVertexTo = sphereRock.vertexPositions[0][loopCount];
                    }

                    Gizmos.DrawLine(verticalVertexFrom, verticalVertexTo);

                }
            }

        }

        public void DrawGizmo(SphereRock sphereRock)
        {
            Gizmos.matrix = sphereRock.transform.localToWorldMatrix;

            //  DrawLines(sphereRock);

            // Draw black cubes on every vertex position of the gem
            foreach (List<Vector3> iteration in sphereRock.vertexPositions)
            {
                foreach (Vector3 spawnPosition in iteration)
                {
                    Gizmos.color = Color.black;
                    float scaleModeModifier = 1f / (sphereRock.width / 2);
                    float cubeSize = Mathf.Clamp(0.05f / scaleModeModifier, 0.05f, 0.3f);
                    Gizmos.DrawCube(spawnPosition, new Vector3(cubeSize, cubeSize, cubeSize));
                    Gizmos.color = Color.blue;
                }
            }

        }
    }
}
