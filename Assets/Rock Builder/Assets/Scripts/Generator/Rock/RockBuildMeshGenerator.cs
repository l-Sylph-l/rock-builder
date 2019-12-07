using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RockBuilder
{
    public class RockBuildMeshGenerator
    {
        private static RockBuildMeshGenerator instance = null;
        private static readonly object padlock = new object();

        RockBuildMeshGenerator()
        {
        }

        public static RockBuildMeshGenerator Instance
        {
            get
            {
                lock (padlock)
                {
                    if (instance == null)
                    {
                        instance = new RockBuildMeshGenerator();
                    }
                    return instance;
                }
            }
        }

        private void CreateIterations(List<Vector3> buildPoints)
        {
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


        }

        private void SortVerticesClockwise(List<List<Vector3>> unsortedVertexPositions)
        {
            List<List<Vector3>> sortedVertexPositions = new List<List<Vector3>>();

            foreach (List<Vector3> iteration in unsortedVertexPositions)
            {
                foreach (Vector3 vertexPosition in iteration)
                {
                    
                    // from 0 to 3 o'clock
                    if (vertexPosition.x > 0 && vertexPosition.z > 0)
                    {

                    }
                    // from 3 to 6 o'clock
                    if (vertexPosition.x > 0 && vertexPosition.z < 0)
                    {

                    }
                    // from 6 to 9 o'clock
                    if (vertexPosition.x < 0 && vertexPosition.z < 0)
                    {

                    }
                    // from 9 to 12 o'clock
                    if (vertexPosition.x < 0 && vertexPosition.z > 0)
                    {

                    }

                }
            }
        }
    }
}
