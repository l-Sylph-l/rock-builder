using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

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

        public Mesh CreateRockMesh(RockBuild rockBuildObject)
        {
            List<List<Vector3>> vertexIteratios = CreateIterations(rockBuildObject.rockBuildPoints);
            rockBuildObject.sortedVertices = SortVerticesClockwise(vertexIteratios);

            return null;
        }

        private List<List<Vector3>> CreateIterations(List<Vector3> buildPoints)
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

            return null;
        }

        private List<RockBuildListIteration> SortVerticesClockwise(List<List<Vector3>> unsortedVertexPositions)
        {
            List<RockBuildListIteration> sortedVertexPositions = new List<RockBuildListIteration>();

            foreach (List<Vector3> iteration in unsortedVertexPositions)
            {
                RockBuildListIteration sortedIteration = new RockBuildListIteration(iteration);

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

        private void equalizeVertexCount(List<RockBuildListIteration> sortedVertexPositions)
        {
            int highestCount = 0;
            foreach (RockBuildListIteration iteration in sortedVertexPositions)
            {
                highestCount = iteration.GetHighestCount();
            }
            foreach (RockBuildListIteration iteration in sortedVertexPositions)
            {
                
            }
        }
    }
}