﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RockBuilder
{
    public class GemPreview
    {
        private static GemPreview instance = null;
        private static readonly object padlock = new object();

        GemPreview()
        {
        }

        public static GemPreview Instance
        {
            get
            {
                lock (padlock)
                {
                    if (instance == null)
                    {
                        instance = new GemPreview();
                    }
                    return instance;
                }
            }
        }

        public void DrawLines(List<Vector3> spawnPoints, int edges)
        {
            Gizmos.color = Color.blue;
            for (int loopCount = 0; edges > loopCount; loopCount++)
            {

                if (loopCount == edges - 1)
                {
                    //Draw line from the bottom peak to the upper pavillon vertices 
                    if (loopCount % 2 == 0)
                    {
                        Gizmos.DrawLine(spawnPoints[0], spawnPoints[1 + loopCount + edges / 2]);
                    }

                    // Draw the upper pavillon circumference 
                    Gizmos.DrawLine(spawnPoints[(edges / 2) + 1 + loopCount], spawnPoints[(edges / 2) + 1]);
                }
                else
                {
                    //Draw line from the bottom peak to the upper pavillon vertices     
                    if (loopCount % 2 == 0)
                    {
                        Gizmos.DrawLine(spawnPoints[0], spawnPoints[1 + loopCount + edges / 2]);
                    }
                    // Draw the upper pavillon circumference 
                    Gizmos.DrawLine(spawnPoints[(edges / 2) + 1 + loopCount], spawnPoints[(edges / 2) + 2 + loopCount]);
                }


                if (loopCount < edges / 2)
                {
                    int index;
                    if (loopCount == 0)
                    {
                        index = (edges / 2) + edges;
                        // Draw from the lower pavillon vertices to the upper vertices
                        Gizmos.DrawLine(spawnPoints[(edges / 2) + 2], spawnPoints[1 + loopCount]);
                        Gizmos.DrawLine(spawnPoints[index], spawnPoints[1 + loopCount]);

                        // Draw from the pavillon vertices to the lower crown vertices
                        Gizmos.DrawLine(spawnPoints[index + 1], spawnPoints[(edges / 2) + edges]);
                        Gizmos.DrawLine(spawnPoints[index + 1], spawnPoints[(edges / 2) + 1]);
                        Gizmos.DrawLine(spawnPoints[index + 1], spawnPoints[(edges / 2) + 2]);

                        // Draw from the lower crown vertices to the upper crown vertices
                        Gizmos.DrawLine(spawnPoints[index + 1], spawnPoints[(edges / 2) + index + 1]);
                        Gizmos.DrawLine(spawnPoints[index + 1], spawnPoints[(edges) + index]);

                        // Draw the first line of the top plane of the gem
                        Gizmos.DrawLine(spawnPoints[(edges / 2) + index + 1], spawnPoints[(edges) + index]);
                    }
                    else
                    {
                        index = (edges / 2) + (loopCount * 2);
                        // Draw from the lower pavillon vertices to the upper vertices
                        Gizmos.DrawLine(spawnPoints[index], spawnPoints[1 + loopCount]);
                        Gizmos.DrawLine(spawnPoints[index + 2], spawnPoints[1 + loopCount]);

                        index = (edges / 2) + edges + loopCount + 1;
                        // Draw from the pavillon vertices to the lower crown vertices
                        // Always connect one crown vertex to three pavillon vertices
                        Gizmos.DrawLine(spawnPoints[index], spawnPoints[(edges / 2) + loopCount * 2]);
                        Gizmos.DrawLine(spawnPoints[index], spawnPoints[(edges / 2) + 1 + loopCount * 2]);
                        Gizmos.DrawLine(spawnPoints[index], spawnPoints[(edges / 2) + 2 + loopCount * 2]);

                        // Draw from the lower crown vertices to the upper crown vertices
                        // Always connect one lower crown vertex to two upper crown vertices
                        Gizmos.DrawLine(spawnPoints[index], spawnPoints[(edges / 2) + index]);
                        Gizmos.DrawLine(spawnPoints[index], spawnPoints[(edges / 2) + index - 1]);

                        index = edges * 2 + loopCount;
                        // Draw the top plane of the gem
                        Gizmos.DrawLine(spawnPoints[index], spawnPoints[index + 1]);
                    }
                }
            }
        }

        public void DrawGizmo(Gem gem)
        {

            DrawLines(gem.vertexPositions, gem.edges);

            // Draw black cubes on every vertex position of the gem
            foreach (Vector3 spawnPosition in gem.vertexPositions)
            {
                Gizmos.color = Color.black;
                float scaleModeModifier = 1f / (gem.radius);
                float cubeSize = Mathf.Clamp(0.05f / scaleModeModifier, 0.05f, 0.3f);
                Gizmos.DrawCube(spawnPosition, new Vector3(cubeSize, cubeSize, cubeSize));
                Gizmos.color = Color.blue;
            }

        }
    }
}
