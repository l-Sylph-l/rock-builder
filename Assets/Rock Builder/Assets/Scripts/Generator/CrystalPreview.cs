﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RockBuilder
{
    public class CrystalPreview
    {
        private static CrystalPreview instance = null;
        private static readonly object padlock = new object();

        CrystalPreview()
        {
        }

        public static CrystalPreview Instance
        {
            get
            {
                lock (padlock)
                {
                    if (instance == null)
                    {
                        instance = new CrystalPreview();
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
                //Draw vertical lines       
                Gizmos.DrawLine(spawnPoints[loopCount], spawnPoints[loopCount + edges]);

                //Draw horizontal lines       
                if (loopCount < edges - 1)
                {
                    Gizmos.DrawLine(spawnPoints[loopCount], spawnPoints[loopCount + 1]);
                    Gizmos.DrawLine(spawnPoints[loopCount + edges], spawnPoints[loopCount + edges + 1]);
                }
                else
                {
                    Gizmos.DrawLine(spawnPoints[edges - 1], spawnPoints[0]);
                    Gizmos.DrawLine(spawnPoints[edges * 2 - 1], spawnPoints[edges]);
                }

                // Draw lines to the peak
                Gizmos.DrawLine(spawnPoints[loopCount], spawnPoints[(edges * 2) + 1]);
                Gizmos.DrawLine(spawnPoints[loopCount + edges], spawnPoints[(edges * 2)]);
            }
        }

        public void DrawGizmo(Crystal crystal)
        {

            DrawLines(crystal.vertexPositions, crystal.edges);

            // Draw black cubes on every vertex position of the diamond
            foreach (Vector3 spawnPosition in crystal.vertexPositions)
            {
                Gizmos.color = Color.black;
                float scaleModeModifier = 1f / (crystal.radius + crystal.height);
                float cubeSize = Mathf.Clamp(0.05f / scaleModeModifier, 0.05f, 0.3f);
                Gizmos.DrawCube(spawnPosition, new Vector3(cubeSize, cubeSize, cubeSize));
                Gizmos.color = Color.blue;
            }

        }
    }
}
