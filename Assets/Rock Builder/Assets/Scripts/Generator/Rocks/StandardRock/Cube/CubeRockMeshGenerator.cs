using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace RockBuilder
{
    public class CubeRockMeshGenerator
    {
        private static CubeRockMeshGenerator instance = null;
        private static readonly object padlock = new object();

        private int triangleVerticesCount;
        private int vertexLoop;
        Vector3[] vertices;
        Vector2[] uv;
        int[] triangles;
        float noiseFactor;

        CubeRockMeshGenerator()
        {
        }

        public static CubeRockMeshGenerator Instance
        {
            get
            {
                lock (padlock)
                {
                    if (instance == null)
                    {
                        instance = new CubeRockMeshGenerator();
                    }
                    return instance;
                }
            }
        }

        public CubeRock CreateVertexPositions(CubeRock cubeRock)
        {
            Vector3 rockPosition = cubeRock.transform.position;

            cubeRock.bottomPlaneVertices = new List<Vector3>();
            cubeRock.upperPlaneVertices = new List<Vector3>();

            cubeRock.bottomVerticalBezelsVertices = new List<Vector3>();
            cubeRock.upperVerticalBezelsVertices = new List<Vector3>();

            cubeRock.bottomBezelsVertices = new List<Vector3>();
            cubeRock.upperBezelsVertices = new List<Vector3>();

            float positivePositionX = cubeRock.width / 2;
            float positivePositionY = cubeRock.height / 2;
            float positivePositionZ = cubeRock.depth / 2;

            float negativePositionX = -cubeRock.width / 2;
            float negativePositionY = -cubeRock.height / 2;
            float negativePositionZ = -cubeRock.depth / 2;

            Vector3 firstBottomVertex = new Vector3(positivePositionX, negativePositionY, positivePositionZ);
            Vector3 secondBottomVertex = new Vector3(positivePositionX, negativePositionY, negativePositionZ);
            Vector3 thirdBottomVertex = new Vector3(negativePositionX, negativePositionY, negativePositionZ);
            Vector3 fourthBottomVertex = new Vector3(negativePositionX, negativePositionY, positivePositionZ);

            Vector3 firstUpperVertex = new Vector3(positivePositionX, positivePositionY, positivePositionZ);
            Vector3 secondUpperVertex = new Vector3(positivePositionX, positivePositionY, negativePositionZ);
            Vector3 thirdUpperVertex = new Vector3(negativePositionX, positivePositionY, negativePositionZ);
            Vector3 fourthUpperVertex = new Vector3(negativePositionX, positivePositionY, positivePositionZ);

            Vector3 firstBottomOffset = new Vector3(positivePositionX, negativePositionY, positivePositionZ - cubeRock.bezelSize / 2);
            Vector3 secondBottomOffset = new Vector3(positivePositionX, negativePositionY, negativePositionZ + cubeRock.bezelSize / 2);
            Vector3 thirdBottomOffset = new Vector3(positivePositionX - cubeRock.bezelSize / 2, negativePositionY, negativePositionZ);
            Vector3 fourthBottomOffset = new Vector3(negativePositionX + cubeRock.bezelSize / 2, negativePositionY, negativePositionZ);
            Vector3 fifthBottomOffset = new Vector3(negativePositionX, negativePositionY, negativePositionZ + cubeRock.bezelSize / 2);
            Vector3 sixthBottomOffset = new Vector3(negativePositionX, negativePositionY, positivePositionZ - cubeRock.bezelSize / 2);
            Vector3 seventhBottomOffset = new Vector3(negativePositionX + cubeRock.bezelSize / 2, negativePositionY, positivePositionZ);
            Vector3 eighthBottomOffset = new Vector3(positivePositionX - cubeRock.bezelSize / 2, negativePositionY, positivePositionZ);

            Vector3 firstUpperOffset = new Vector3(positivePositionX, positivePositionY, positivePositionZ - cubeRock.bezelSize / 2);
            Vector3 secondUpperOffset = new Vector3(positivePositionX, positivePositionY, negativePositionZ + cubeRock.bezelSize / 2);
            Vector3 thirdUpperOffset = new Vector3(positivePositionX - cubeRock.bezelSize / 2, positivePositionY, negativePositionZ);
            Vector3 fourthUpperOffset = new Vector3(negativePositionX + cubeRock.bezelSize / 2, positivePositionY, negativePositionZ);
            Vector3 fifthUpperOffset = new Vector3(negativePositionX, positivePositionY, negativePositionZ + cubeRock.bezelSize / 2);
            Vector3 sixthUpperOffset = new Vector3(negativePositionX, positivePositionY, positivePositionZ - cubeRock.bezelSize / 2);
            Vector3 seventhUpperOffset = new Vector3(negativePositionX + cubeRock.bezelSize / 2, positivePositionY, positivePositionZ);
            Vector3 eighthUpperOffset = new Vector3(positivePositionX - cubeRock.bezelSize / 2, positivePositionY, positivePositionZ);

            Vector3 bezelOffsetY = new Vector3(0, cubeRock.bezelSize / 2, 0);

            cubeRock.bottomVerticalBezelsVertices.Add(firstBottomOffset + bezelOffsetY);
            cubeRock.bottomVerticalBezelsVertices.Add(secondBottomOffset + bezelOffsetY);
            cubeRock.bottomVerticalBezelsVertices.Add(thirdBottomOffset + bezelOffsetY);
            cubeRock.bottomVerticalBezelsVertices.Add(fourthBottomOffset + bezelOffsetY);
            cubeRock.bottomVerticalBezelsVertices.Add(fifthBottomOffset + bezelOffsetY);
            cubeRock.bottomVerticalBezelsVertices.Add(sixthBottomOffset + bezelOffsetY);
            cubeRock.bottomVerticalBezelsVertices.Add(seventhBottomOffset + bezelOffsetY);
            cubeRock.bottomVerticalBezelsVertices.Add(eighthBottomOffset + bezelOffsetY);

            cubeRock.upperVerticalBezelsVertices.Add(firstUpperOffset - bezelOffsetY);
            cubeRock.upperVerticalBezelsVertices.Add(secondUpperOffset - bezelOffsetY);
            cubeRock.upperVerticalBezelsVertices.Add(thirdUpperOffset - bezelOffsetY);
            cubeRock.upperVerticalBezelsVertices.Add(fourthUpperOffset - bezelOffsetY);
            cubeRock.upperVerticalBezelsVertices.Add(fifthUpperOffset - bezelOffsetY);
            cubeRock.upperVerticalBezelsVertices.Add(sixthUpperOffset - bezelOffsetY);
            cubeRock.upperVerticalBezelsVertices.Add(seventhUpperOffset - bezelOffsetY);
            cubeRock.upperVerticalBezelsVertices.Add(eighthUpperOffset - bezelOffsetY);

            cubeRock.bottomBezelsVertices.Add(Vector3.Lerp(eighthBottomOffset + bezelOffsetY, firstBottomOffset + bezelOffsetY, 0.5f));
            cubeRock.bottomBezelsVertices.Add(Vector3.Lerp(secondBottomOffset + bezelOffsetY, thirdBottomOffset + bezelOffsetY, 0.5f));
            cubeRock.bottomBezelsVertices.Add(Vector3.Lerp(fourthBottomOffset + bezelOffsetY, fifthBottomOffset + bezelOffsetY, 0.5f));
            cubeRock.bottomBezelsVertices.Add(Vector3.Lerp(sixthBottomOffset + bezelOffsetY, seventhBottomOffset + bezelOffsetY, 0.5f));

            cubeRock.upperBezelsVertices.Add(Vector3.Lerp(eighthUpperOffset - bezelOffsetY, firstUpperOffset - bezelOffsetY, 0.5f));
            cubeRock.upperBezelsVertices.Add(Vector3.Lerp(secondUpperOffset - bezelOffsetY, thirdUpperOffset - bezelOffsetY, 0.5f));
            cubeRock.upperBezelsVertices.Add(Vector3.Lerp(fourthUpperOffset - bezelOffsetY, fifthUpperOffset - bezelOffsetY, 0.5f));
            cubeRock.upperBezelsVertices.Add(Vector3.Lerp(sixthUpperOffset - bezelOffsetY, seventhUpperOffset - bezelOffsetY, 0.5f));

            Vector3 firstBezelOffset = new Vector3(-cubeRock.bezelSize / 2, 0, -cubeRock.bezelSize / 2);
            Vector3 secondBezelOffset = new Vector3(-cubeRock.bezelSize / 2, 0, cubeRock.bezelSize / 2);
            Vector3 thirdBezelOffset = new Vector3(cubeRock.bezelSize / 2, 0, cubeRock.bezelSize / 2);
            Vector3 fourthBezelOffset = new Vector3(cubeRock.bezelSize / 2, 0, -cubeRock.bezelSize / 2);

            cubeRock.bottomPlaneVertices.Add(firstBottomVertex + firstBezelOffset);
            cubeRock.bottomPlaneVertices.Add(secondBottomVertex + secondBezelOffset);
            cubeRock.bottomPlaneVertices.Add(thirdBottomVertex + thirdBezelOffset);
            cubeRock.bottomPlaneVertices.Add(fourthBottomVertex + fourthBezelOffset);

            cubeRock.upperPlaneVertices.Add(firstUpperVertex + firstBezelOffset);
            cubeRock.upperPlaneVertices.Add(secondUpperVertex + secondBezelOffset);
            cubeRock.upperPlaneVertices.Add(thirdUpperVertex + thirdBezelOffset);
            cubeRock.upperPlaneVertices.Add(fourthUpperVertex + fourthBezelOffset);

            return cubeRock;
        }

        public Mesh CreateRockMesh(CubeRock standardRock)
        {
            return CreateHardMesh(standardRock);
        }

        private Mesh CreateHardMesh(CubeRock cubeRock)
        {

            vertexLoop = 0;
            triangleVerticesCount = 0;
            int verticesCount = 40 * 21;
            vertices = new Vector3[verticesCount];
            uv = new Vector2[verticesCount];
            triangles = new int[40 * 21];
            noiseFactor = cubeRock.noise;
            int increaseValue = 2;

            List<Vector3> bottomPlaneVertices = cubeRock.bottomPlaneVertices;
            List<Vector3> upperPlaneVertices = cubeRock.upperPlaneVertices;
            List<Vector3> bottomBezelsVertices = cubeRock.bottomVerticalBezelsVertices;
            List<Vector3> upperBezelsVertices = cubeRock.upperVerticalBezelsVertices;

            List<List<Vector3>> frontPlane = new List<List<Vector3>>();
            List<List<Vector3>> leftPlane = new List<List<Vector3>>();
            List<List<Vector3>> backPlane = new List<List<Vector3>>();
            List<List<Vector3>> rightPlane = new List<List<Vector3>>();
            List<List<Vector3>> upperPlane = new List<List<Vector3>>();
            List<List<Vector3>> bottomPlane = new List<List<Vector3>>();

            List<Vector3> frontPlaneList = new List<Vector3> { bottomBezelsVertices[0], bottomBezelsVertices[1], upperBezelsVertices[0], upperBezelsVertices[1] };
            List<Vector3> leftPlaneList = new List<Vector3> { bottomBezelsVertices[2], bottomBezelsVertices[3], upperBezelsVertices[2], upperBezelsVertices[3] };
            List<Vector3> backPlaneList = new List<Vector3> { bottomBezelsVertices[4], bottomBezelsVertices[5], upperBezelsVertices[4], upperBezelsVertices[5] };
            List<Vector3> rightPlaneList = new List<Vector3> { bottomBezelsVertices[6], bottomBezelsVertices[7], upperBezelsVertices[6], upperBezelsVertices[7] };
            List<Vector3> upperPlaneList = new List<Vector3> { upperPlaneVertices[0], upperPlaneVertices[1], upperPlaneVertices[3], upperPlaneVertices[2] };
            List<Vector3> bottomPlaneList = new List<Vector3> { bottomPlaneVertices[2], bottomPlaneVertices[1], bottomPlaneVertices[3], bottomPlaneVertices[0] };

            frontPlane.Add(frontPlaneList);
            leftPlane.Add(leftPlaneList);
            backPlane.Add(backPlaneList);
            rightPlane.Add(rightPlaneList);
            upperPlane.Add(upperPlaneList);
            bottomPlane.Add(bottomPlaneList);

            List<List<Vector3>> frontBevelRight = new List<List<Vector3>>();
            List<List<Vector3>> frontBevelLeft = new List<List<Vector3>>();
            List<List<Vector3>> backBevelRight = new List<List<Vector3>>();
            List<List<Vector3>> backBevelLeft = new List<List<Vector3>>();

            List<Vector3> frontBevelRightList = new List<Vector3> { bottomBezelsVertices[7], bottomBezelsVertices[0], upperBezelsVertices[7], upperBezelsVertices[0] };
            List<Vector3> frontBevelLeftList = new List<Vector3> { bottomBezelsVertices[1], bottomBezelsVertices[2], upperBezelsVertices[1], upperBezelsVertices[2] };
            List<Vector3> backBevelRightList = new List<Vector3> { bottomBezelsVertices[3], bottomBezelsVertices[4], upperBezelsVertices[3], upperBezelsVertices[4] };
            List<Vector3> backBevelLeftList = new List<Vector3> { bottomBezelsVertices[5], bottomBezelsVertices[6], upperBezelsVertices[5], upperBezelsVertices[6] };

            frontBevelRight.Add(frontBevelRightList);
            frontBevelLeft.Add(frontBevelLeftList);
            backBevelRight.Add(backBevelRightList);
            backBevelLeft.Add(backBevelLeftList);

            List<List<Vector3>> upperBevelFront = new List<List<Vector3>>();
            List<List<Vector3>> upperBevelLeft = new List<List<Vector3>>();
            List<List<Vector3>> upperBevelBack = new List<List<Vector3>>();
            List<List<Vector3>> upperBevelRight = new List<List<Vector3>>();

            List<Vector3> upperBevelFrontList = new List<Vector3> { upperBezelsVertices[0], upperBezelsVertices[1], upperPlaneVertices[0], upperPlaneVertices[1] };
            List<Vector3> upperBevelLeftList = new List<Vector3> { upperBezelsVertices[2], upperBezelsVertices[3], upperPlaneVertices[1], upperPlaneVertices[2] };
            List<Vector3> upperBevelBackList = new List<Vector3> { upperBezelsVertices[4], upperBezelsVertices[5], upperPlaneVertices[2], upperPlaneVertices[3] };
            List<Vector3> upperBevelRightList = new List<Vector3> { upperBezelsVertices[6], upperBezelsVertices[7], upperPlaneVertices[3], upperPlaneVertices[0] };

            upperBevelFront.Add(upperBevelFrontList);
            upperBevelLeft.Add(upperBevelLeftList);
            upperBevelBack.Add(upperBevelBackList);
            upperBevelRight.Add(upperBevelRightList);

            List<List<Vector3>> bottomBevelFront = new List<List<Vector3>>();
            List<List<Vector3>> bottomBevelLeft = new List<List<Vector3>>();
            List<List<Vector3>> bottomBevelBack = new List<List<Vector3>>();
            List<List<Vector3>> bottomBevelRight = new List<List<Vector3>>();

            List<Vector3> bottomBevelFrontList = new List<Vector3> { bottomPlaneVertices[0], bottomPlaneVertices[1], bottomBezelsVertices[0], bottomBezelsVertices[1] };
            List<Vector3> bottomBevelLeftList = new List<Vector3> { bottomPlaneVertices[1], bottomPlaneVertices[2], bottomBezelsVertices[2], bottomBezelsVertices[3] };
            List<Vector3> bottomBevelBackList = new List<Vector3> { bottomPlaneVertices[2], bottomPlaneVertices[3], bottomBezelsVertices[4], bottomBezelsVertices[5] };
            List<Vector3> bottomBevelRightList = new List<Vector3> { bottomPlaneVertices[3], bottomPlaneVertices[0], bottomBezelsVertices[6], bottomBezelsVertices[7] };

            bottomBevelFront.Add(bottomBevelFrontList);
            bottomBevelLeft.Add(bottomBevelLeftList);
            bottomBevelBack.Add(bottomBevelBackList);
            bottomBevelRight.Add(bottomBevelRightList);

            List<Vector3> upperRightBevelCrossFront = new List<Vector3>() { upperPlaneList[0], rightPlaneList[3], frontPlaneList[2] };
            List<Vector3> upperLeftBevelCrossFront = new List<Vector3>() { upperPlaneList[1], frontPlaneList[3], leftPlaneList[2] };
            List<Vector3> upperLeftBevelCrossBack = new List<Vector3>() { upperPlaneList[2], backPlaneList[3], rightPlaneList[2] };
            List<Vector3> upperRightBevelCrossBack = new List<Vector3>() { upperPlaneList[3], leftPlaneList[3], backPlaneList[2] };

            List<Vector3> bottomRightBevelCrossFront = new List<Vector3>() { bottomPlaneList[3], frontPlaneList[0], rightPlaneList[1] };
            List<Vector3> bottomLeftBevelCrossFront = new List<Vector3>() { bottomPlaneList[1], leftPlaneList[0], frontPlaneList[1] };
            List<Vector3> bottomLeftBevelCrossBack = new List<Vector3>() { bottomPlaneList[0], backPlaneList[0], leftPlaneList[1] };
            List<Vector3> bottomRightBevelCrossBack = new List<Vector3>() { bottomPlaneList[2], rightPlaneList[0], backPlaneList[1] };

            frontPlane = increasePolyCount(frontPlane, increaseValue);
            leftPlane = increasePolyCount(leftPlane, increaseValue);
            backPlane = increasePolyCount(backPlane, increaseValue);
            rightPlane = increasePolyCount(rightPlane, increaseValue);
            upperPlane = increasePolyCount(upperPlane, increaseValue);
            bottomPlane = increasePolyCount(bottomPlane, increaseValue);

            FillPlane(frontPlane, cubeRock.width, cubeRock.height, increaseValue);
            FillPlane(leftPlane, cubeRock.width, cubeRock.height, increaseValue);
            FillPlane(backPlane, cubeRock.depth, cubeRock.height, increaseValue);
            FillPlane(rightPlane, cubeRock.width, cubeRock.height, increaseValue);
            FillPlane(upperPlane, cubeRock.depth, cubeRock.width, increaseValue);
            FillPlane(bottomPlane, cubeRock.depth, cubeRock.width, increaseValue);

            FillPlane(frontBevelRight, cubeRock.bezelSize, cubeRock.height, increaseValue);
            FillPlane(frontBevelLeft, cubeRock.bezelSize, cubeRock.height, increaseValue);
            FillPlane(backBevelRight, cubeRock.bezelSize, cubeRock.height, increaseValue);
            FillPlane(backBevelLeft, cubeRock.bezelSize, cubeRock.height, increaseValue);

            FillPlane(upperBevelFront, cubeRock.width, cubeRock.bezelSize, increaseValue);
            FillPlane(upperBevelLeft, cubeRock.depth, cubeRock.bezelSize, increaseValue);
            FillPlane(upperBevelBack, cubeRock.width, cubeRock.bezelSize, increaseValue);
            FillPlane(upperBevelRight, cubeRock.depth, cubeRock.bezelSize, increaseValue);

            FillPlane(bottomBevelFront, cubeRock.bezelSize, cubeRock.height, increaseValue);
            FillPlane(bottomBevelLeft, cubeRock.bezelSize, cubeRock.depth, increaseValue);
            FillPlane(bottomBevelBack, cubeRock.bezelSize, cubeRock.height, increaseValue);
            FillPlane(bottomBevelRight, cubeRock.bezelSize, cubeRock.depth, increaseValue);

            FillTriangle(upperRightBevelCrossFront);
            FillTriangle(upperLeftBevelCrossFront);
            FillTriangle(upperRightBevelCrossBack);
            FillTriangle(upperLeftBevelCrossBack);

            FillTriangle(bottomRightBevelCrossFront);
            FillTriangle(bottomLeftBevelCrossFront);
            FillTriangle(bottomLeftBevelCrossBack);
            FillTriangle(bottomRightBevelCrossBack);

            Mesh mesh = new Mesh();
            mesh.Clear();
            mesh.vertices = vertices;
            mesh.triangles = triangles;
            mesh.uv = uv;
            mesh.name = "generated cube rock mesh";
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

        private void FillPlane(List<List<Vector3>> planeVertices, float uvWidth, float uvHeight, int increaseFactor)
        {
            float loopCount = 0;
            float rowCount = 0;
            float widthIteration = uvWidth / (planeVertices.Count / increaseFactor);
            float heightIteration = uvHeight / (planeVertices.Count / increaseFactor);
            float biggerIteration;

            if (widthIteration >= heightIteration)
            {
                biggerIteration = uvHeight;
            }
            else
            {
                biggerIteration = heightIteration;
            }

            foreach (List<Vector3> vertexList in planeVertices)
            {
                triangles[triangleVerticesCount] = vertexLoop;
                triangles[triangleVerticesCount + 1] = vertexLoop + 1;
                triangles[triangleVerticesCount + 2] = vertexLoop + 2;
                triangles[triangleVerticesCount + 3] = vertexLoop + 1;
                triangles[triangleVerticesCount + 4] = vertexLoop + 3;
                triangles[triangleVerticesCount + 5] = vertexLoop + 2;

                if (loopCount == increaseFactor)
                {
                    loopCount = 0;
                    rowCount++;
                }

                float firstWidthUv = widthIteration * loopCount;
                float secondWidthUv = widthIteration * (loopCount + 1);
                float firstHeightUv = heightIteration * rowCount;
                float secondHeightUv = heightIteration * (rowCount + 1);

                if (loopCount == 0)
                {
                    firstWidthUv = 1;

                }
                else
                {
                    firstWidthUv = 1 - (firstWidthUv / biggerIteration);

                }

                if (rowCount == 0)
                {
                    firstHeightUv = 0;
                }
                else
                {
                    firstHeightUv = firstHeightUv / biggerIteration;
                }

                secondWidthUv = 1 - (secondWidthUv / biggerIteration);
                secondHeightUv = secondHeightUv / biggerIteration;

                uv[vertexLoop] = new Vector2(firstWidthUv, firstHeightUv);
                uv[vertexLoop + 1] = new Vector2(secondWidthUv, firstHeightUv);
                uv[vertexLoop + 2] = new Vector2(firstWidthUv, secondHeightUv);
                uv[vertexLoop + 3] = new Vector2(secondWidthUv, secondHeightUv);

                triangleVerticesCount += 6;

                foreach (Vector3 vertex in vertexList)
                {
                    vertices[vertexLoop] = vertex;
                    vertexLoop++;
                }

                loopCount++;
            }
        }

        private void FillTriangle(List<Vector3> triangleVertices)
        {
            triangles[triangleVerticesCount] = vertexLoop;
            triangles[triangleVerticesCount + 1] = vertexLoop + 1;
            triangles[triangleVerticesCount + 2] = vertexLoop + 2;

            uv[vertexLoop] = new Vector2(0, 1);
            uv[vertexLoop + 1] = new Vector2(0, 0);
            uv[vertexLoop + 2] = new Vector2(1, 1);

            triangleVerticesCount += 3;

            foreach (Vector3 vertex in triangleVertices)
            {
                vertices[vertexLoop] = vertex;
                vertexLoop++;
            }

        }

        private List<List<Vector3>> increasePolyCount(List<List<Vector3>> planeVertices, int increaseFactor)
        {
            List<List<Vector3>> increasedPolyCountPlanes = new List<List<Vector3>>();

            foreach (List<Vector3> vertexList in planeVertices)
            {

                int bottomRightIndex = 0;
                int bottomLeftIndex = 1;
                int upperRightIndex = 2;
                int upperLeftIndex = 3;

                for (int rowCount = 0; rowCount < increaseFactor; rowCount++)
                {
                    float rowLerpFactor = (1f / increaseFactor) * rowCount;
                    float nextRowLerpFactor = (1f / increaseFactor) * (rowCount + 1);

                    Vector3 lerpVertexBottomRight = Vector3.Lerp(vertexList[bottomRightIndex], vertexList[upperRightIndex], rowLerpFactor);
                    Vector3 lerpVertexBottomLeft = Vector3.Lerp(vertexList[bottomLeftIndex], vertexList[upperLeftIndex], rowLerpFactor);
                    Vector3 lerpVertexUpperRight = Vector3.Lerp(vertexList[bottomRightIndex], vertexList[upperRightIndex], nextRowLerpFactor);
                    Vector3 lerpVertexUpperLeft = Vector3.Lerp(vertexList[bottomLeftIndex], vertexList[upperLeftIndex], nextRowLerpFactor);


                    for (int loopCount = 0; loopCount < increaseFactor; loopCount++)
                    {
                        float lerpFactor = (1f / increaseFactor) * loopCount;
                        float nextLerpFactor = (1f / increaseFactor) * (loopCount + 1);

                        Vector3 newVertexBottomRight = Vector3.Lerp(lerpVertexBottomRight, lerpVertexBottomLeft, lerpFactor);
                        Vector3 newVertexBottomLeft = Vector3.Lerp(lerpVertexBottomRight, lerpVertexBottomLeft, nextLerpFactor);
                        Vector3 newVertexUpperRight = Vector3.Lerp(lerpVertexUpperRight, lerpVertexUpperLeft, lerpFactor);
                        Vector3 newVertexUpperLeft = Vector3.Lerp(lerpVertexUpperRight, lerpVertexUpperLeft, nextLerpFactor);

                        if (loopCount != 0 && rowCount != 0)
                        {
                            newVertexBottomRight = AddNoise(newVertexBottomRight);
                        }

                        if (loopCount != increaseFactor - 1 && rowCount != 0)
                        {
                            newVertexBottomLeft = AddNoise(newVertexBottomLeft);
                        }

                        if (loopCount != 0 && rowCount != increaseFactor - 1)
                        {
                            newVertexUpperRight = AddNoise(newVertexUpperRight);
                        }

                        if (loopCount != increaseFactor - 1 && rowCount != increaseFactor - 1)
                        {
                            newVertexUpperLeft = AddNoise(newVertexUpperLeft);
                        }

                        if (loopCount > 0)
                        {
                            int indexBefore = (loopCount - 1) + (rowCount * increaseFactor);
                            List<Vector3> squareBefore = increasedPolyCountPlanes[indexBefore];
                            newVertexBottomRight = squareBefore[1];
                            newVertexUpperRight = squareBefore[3];
                        }

                        if (rowCount > 0)
                        {
                            int indexRowBefore = (loopCount) + ((rowCount - 1) * increaseFactor);
                            List<Vector3> squareBefore = increasedPolyCountPlanes[indexRowBefore];
                            newVertexBottomRight = squareBefore[2];
                            newVertexBottomLeft = squareBefore[3];
                        }

                        // Smooth algorythm
                        //    if(noiseFactor != 0 &&){
                        //         newVertexBottomRight = AddNoise(newVertexBottomRight);
                        //         newVertexBottomLeft = AddNoise(newVertexBottomLeft);
                        //         newVertexUpperRight = AddNoise(newVertexUpperRight);
                        //         newVertexUpperLeft = AddNoise(newVertexUpperLeft);
                        //     }

                        List<Vector3> newSquare = new List<Vector3> { newVertexBottomRight, newVertexBottomLeft, newVertexUpperRight, newVertexUpperLeft };

                        increasedPolyCountPlanes.Add(newSquare);
                    }
                }
            }

            // for (int rowCount = 0; rowCount < increaseFactor; rowCount++)
            // {

            //     List<List<Vector3>> row = new List<List<Vector3>>();


            //     for (int loopCount = 0; loopCount < increaseFactor; loopCount++)
            //     {
            //         row.Add(increasedPolyCountPlanes[loopCount]);
            //     }


            //     for (int loopCount = 0; loopCount < increaseFactor; loopCount++)
            //     {
            //         Vector3 vertex1 = row[loopCount][0];
            //         Vector3 vertex2 = row[loopCount][1];
            //     }
            // }

            planeVertices = increasedPolyCountPlanes;

            return planeVertices;
        }

        private Vector3 AddNoise(Vector3 vertex)
        {
            float halfNoiseFactor = noiseFactor / 2;
            float noiseX = Random.Range(-halfNoiseFactor, halfNoiseFactor);
            float noiseY = Random.Range(-halfNoiseFactor, halfNoiseFactor);
            float noiseZ = Random.Range(-halfNoiseFactor, halfNoiseFactor);

            Vector3 noiseVector = new Vector3(noiseX, noiseY, noiseZ);
            return vertex + noiseVector;
        }
    }
}