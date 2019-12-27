using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace RockBuilder
{
    public class CustomRockListIteration
    {
        public List<Vector3> unsortedVertexList;

        public List<Vector3> firstQuarter = new List<Vector3>();
        public List<Vector3> secondQuarter = new List<Vector3>();
        public List<Vector3> thirdQuarter = new List<Vector3>();
        public List<Vector3> fourthQuarter = new List<Vector3>();

        public CustomRockListIteration(List<Vector3> vertexPositions)
        {
            unsortedVertexList = vertexPositions;
        }

        public void Sort()
        {
            firstQuarter.OrderBy(vector => vector.x);
            firstQuarter.OrderByDescending(vector => vector.z);

            secondQuarter.OrderByDescending(vector => vector.x);
            secondQuarter.OrderBy(vector => vector.z);

            thirdQuarter.OrderByDescending(vector => vector.x);
            thirdQuarter.OrderByDescending(vector => vector.z);

            fourthQuarter.OrderBy(vector => vector.x);
            fourthQuarter.OrderBy(vector => vector.z);
        }

        public int GetHighestCount()
        {
            int highestCount = firstQuarter.Count;

            if (secondQuarter.Count > highestCount)
            {
                highestCount = secondQuarter.Count;
            }

            if (thirdQuarter.Count > highestCount)
            {
                highestCount = thirdQuarter.Count;
            }

            if (fourthQuarter.Count > highestCount)
            {
                highestCount = fourthQuarter.Count;
            }

            return highestCount;
        }

        public Vector3 GetCenterPoint()
        {
            List<Vector3> vertexList = unsortedVertexList.ToList();
            if (vertexList != null || vertexList.Count != 0)
            {
                float maxCoordinateX = vertexList.OrderBy(vector => vector.x).First().x;
                float maxCoordinateZ = vertexList.OrderBy(vector => vector.z).First().z;
                float minCoordinateX = vertexList.OrderByDescending(vector => vector.x).First().x;
                float minCoordinateZ = vertexList.OrderByDescending(vector => vector.z).First().z;

                float middlePointX = minCoordinateX + (maxCoordinateX - minCoordinateX);
                float middlePointZ = minCoordinateZ + (maxCoordinateZ - minCoordinateZ);

                return new Vector3(middlePointX, 0f, middlePointZ);
            }
            else
            {
                return Vector3.zero;
            }
        }

        public void EqualizeVertexCount(int vertexCount)
        {
            if (firstQuarter.Count != vertexCount)
            {
                InterpolateVertices(firstQuarter, fourthQuarter, secondQuarter, vertexCount);
            }

            if (secondQuarter.Count != vertexCount)
            {
                InterpolateVertices(secondQuarter, firstQuarter, thirdQuarter, vertexCount);
            }

            if (thirdQuarter.Count != vertexCount)
            {
                InterpolateVertices(thirdQuarter, secondQuarter, fourthQuarter, vertexCount);
            }

            if (fourthQuarter.Count != vertexCount)
            {
                InterpolateVertices(fourthQuarter, thirdQuarter, firstQuarter, vertexCount);
            }
        }

        private void InterpolateVertices(List<Vector3> listToInterpolate, List<Vector3> listBefore, List<Vector3> listAfter, int vertexCount)
        {
            int interpolationCountBefore;
            int interpolationCountAfter;
            int interpolationCount = vertexCount - listToInterpolate.Count;
            float interpolationFactorBefore;
            float interpolationFactorAfter;

            if (interpolationCount % 2 == 0)
            {
                interpolationCountBefore = interpolationCount / 2;
                interpolationCountAfter = interpolationCount / 2;
                interpolationFactorBefore = 1f / (interpolationCountBefore + 1);
                interpolationFactorAfter = 1f / (interpolationCountAfter + 1);
            }
            else
            {
                interpolationCountBefore = Mathf.RoundToInt(interpolationCount / 2);
                interpolationCountAfter = interpolationCountBefore - 1;
                interpolationFactorBefore = 1f / (interpolationCountBefore);
                interpolationFactorAfter = 1f / (interpolationCountAfter);
            }

            Vector3 firstVertex = listToInterpolate.First();

            for (int loopCount = 0; interpolationCountBefore > loopCount; loopCount++)
            {
                Vector3 interpolatedVertex = Vector3.Lerp(firstVertex, listBefore.Last(), interpolationFactorBefore * (loopCount + 1));
                listBefore.Add(interpolatedVertex);
            }

            Vector3 lastVertex = listToInterpolate.First();

            for (int loopCount = 0; interpolationCountBefore > loopCount; loopCount++)
            {
                Vector3 interpolatedVertex = Vector3.Lerp(lastVertex, listAfter.Last(), interpolationFactorAfter * (loopCount + 1));
                listAfter.Add(interpolatedVertex);
            }

            Sort();
        }

        public int GetVertexCount()
        {
            int vertexCount = 0;
            vertexCount += firstQuarter.Count;
            vertexCount += secondQuarter.Count;
            vertexCount += thirdQuarter.Count;
            vertexCount += fourthQuarter.Count;
            return vertexCount;
        }

        public List<Vector3> GetSortedVertexList()
        {
            List<Vector3> sortedVertices = new List<Vector3>();
            sortedVertices.AddRange(firstQuarter);
            sortedVertices.AddRange(secondQuarter);
            sortedVertices.AddRange(thirdQuarter);
            sortedVertices.AddRange(fourthQuarter);

            return sortedVertices;
        }
    }
}