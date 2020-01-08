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

            cubeRock.bottomCornerVertices = new List<Vector3>();
            cubeRock.upperCornerVertices = new List<Vector3>();

            cubeRock.bottomVerticalBezelsVertices = new List<Vector3>();
            cubeRock.upperVerticalBezelsVertices = new List<Vector3>();

            cubeRock.bottomBezelsVertices = new List<Vector3>();
            cubeRock.upperBezelsVertices = new List<Vector3>();

            float positivePositionX = cubeRock.width / 2;
            float positivePositionY = cubeRock.heigth / 2;
            float positivePositionZ = cubeRock.depth / 2;

            float negativePositionX = -cubeRock.width / 2;
            float negativePositionY = -cubeRock.heigth / 2;
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

            cubeRock.bottomCornerVertices.Add(firstBottomVertex + firstBezelOffset);
            cubeRock.bottomCornerVertices.Add(secondBottomVertex + secondBezelOffset);
            cubeRock.bottomCornerVertices.Add(thirdBottomVertex + thirdBezelOffset);
            cubeRock.bottomCornerVertices.Add(fourthBottomVertex + fourthBezelOffset);

            cubeRock.upperCornerVertices.Add(firstUpperVertex + firstBezelOffset);
            cubeRock.upperCornerVertices.Add(secondUpperVertex + secondBezelOffset);
            cubeRock.upperCornerVertices.Add(thirdUpperVertex + thirdBezelOffset);
            cubeRock.upperCornerVertices.Add(fourthUpperVertex + fourthBezelOffset);

            return cubeRock;
        }

        public Mesh CreateRockMesh(CubeRock standardRock)
        {
            return CreateHardMesh(standardRock);
        }

        private Mesh CreateHardMesh(CubeRock cubeRock)
        {

            int vrticesCount = 10 * 6;
            Vector3[] vertices = new Vector3[vrticesCount];
            Vector2[] uv = new Vector2[vrticesCount];
            int vertexLoop = 0;

            foreach (Vector3 vertex in cubeRock.bottomVerticalBezelsVertices)
            {
                vertices[vertexLoop] = vertex;
                vertexLoop++;
            }

            foreach (Vector3 vertex in cubeRock.upperVerticalBezelsVertices)
            {
                vertices[vertexLoop] = vertex;
                vertexLoop++;
            }

            foreach (Vector3 vertex in cubeRock.bottomCornerVertices)
            {
                vertices[vertexLoop] = vertex;
                vertexLoop++;
            }

            foreach (Vector3 vertex in cubeRock.upperCornerVertices)
            {
                vertices[vertexLoop] = vertex;
                vertexLoop++;
            }


            int[] triangles = new int[10 * 6];
            int verticesCount = 0;
            int triangleVerticesCount = 0;

            for (int loopCount = 0; loopCount < 8; loopCount += 2)
            {
                triangles[triangleVerticesCount] = loopCount;
                triangles[triangleVerticesCount + 1] = loopCount + 1;
                triangles[triangleVerticesCount + 2] = loopCount + 8;
                triangles[triangleVerticesCount + 3] = loopCount + 8;
                triangles[triangleVerticesCount + 4] = loopCount + 1;
                triangles[triangleVerticesCount + 5] = loopCount + 9;

                verticesCount += 4;
                triangleVerticesCount += 6;
            }


            triangles[triangleVerticesCount] = verticesCount;
            triangles[triangleVerticesCount + 1] = verticesCount + 2;
            triangles[triangleVerticesCount + 2] = verticesCount + 1;
            triangles[triangleVerticesCount + 3] = verticesCount;
            triangles[triangleVerticesCount + 4] = verticesCount + 3;
            triangles[triangleVerticesCount + 5] = verticesCount + 2;
            triangles[triangleVerticesCount + 6] = verticesCount + 4;
            triangles[triangleVerticesCount + 7] = verticesCount + 5;
            triangles[triangleVerticesCount + 8] = verticesCount + 6;
            triangles[triangleVerticesCount + 9] = verticesCount + 4;
            triangles[triangleVerticesCount + 10] = verticesCount + 6;
            triangles[triangleVerticesCount + 11] = verticesCount + 7;

            verticesCount += 8;
            triangleVerticesCount += 12;




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


    }
}