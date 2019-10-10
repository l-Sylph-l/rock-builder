using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
[ExecuteInEditMode]

public class TestCubeGeneration : MonoBehaviour
{
    void Start()
    {
        CreateCube(1f, 1f, 1f);
    }

    private void CreateCube(float width, float height, float depth)
    {
        Vector3[] vertices = {
            //vertices top
            new Vector3 (-(width/2), (height/2), (depth/2)),
            new Vector3 (-(width/2), (height/2), -(depth/2)),
            new Vector3 ((width/2), (height/2), -(depth/2)),
            new Vector3 ((width/2), (height/2), (depth/2)),      

         //    0, 2, 1, //face top
        //          0, 3, 2,

            //vertices bottom
            new Vector3 (-(width/2), (-height/2), -(depth/2)),
            new Vector3 (-(width/2), (-height/2), (depth/2)),
            new Vector3 ((width/2), (-height/2), (depth/2)),
            new Vector3 ((width/2), (-height/2), -(depth/2)),

            //vertices front
            new Vector3 (-(width/2), (height/2), -(depth/2)),
            new Vector3 (-(width/2), -(height/2), -(depth/2)),
            new Vector3 ((width/2), -(height/2), -(depth/2)),
            new Vector3 ((width/2), (height/2), -(depth/2)),

            //vertices back
            new Vector3 ((width/2), (height/2), (depth/2)),
            new Vector3 ((width/2), -(height/2), (depth/2)),
            new Vector3 (-(width/2), -(height/2), (depth/2)),
            new Vector3 (-(width/2), (height/2), (depth/2)),

            //vertices right
            new Vector3 ((width/2), (height/2), -(depth/2)),
            new Vector3 ((width/2), -(height/2), -(depth/2)),
            new Vector3 ((width/2), -(height/2), (depth/2)),
            new Vector3 ((width/2), (height/2), (depth/2)),

            //vertices left
            new Vector3 (-(width/2), (height/2), (depth/2)),
            new Vector3 (-(width/2), -(height/2), (depth/2)),
            new Vector3 (-(width/2), -(height/2), -(depth/2)),
            new Vector3 (-(width/2), (height/2), -(depth/2))

        };

        Vector2[] uv = {
            //uv top
            new Vector2 (0, 1),
            new Vector2 (1, 1),
            new Vector2 (0, 0),
            new Vector2 (1, 0),

            //uv bottom
            new Vector2 (0, 1),
            new Vector2 (1, 1),
            new Vector2 (0, 0),
            new Vector2 (1, 0),

            //uv front
            new Vector2 (0, 1),
            new Vector2 (1, 1),
            new Vector2 (0, 0),
            new Vector2 (1, 0),

            //uv back
            new Vector2 (0, 1),
            new Vector2 (1, 1),
            new Vector2 (0, 0),
            new Vector2 (1, 0),

            //uv right
            new Vector2 (0, 1),
            new Vector2 (1, 1),
            new Vector2 (0, 0),
            new Vector2 (1, 0),

            //uv left
            new Vector2 (0, 1),
            new Vector2 (1, 1),
            new Vector2 (0, 0),
            new Vector2 (1, 0)
        };

        int[] triangles = new int[36];
        int verticesCount = 0;
        int triangleVerticesCount = 0;

        for (int i = 0; verticesCount < vertices.Length; i = i + 2)
        {
            triangles[triangleVerticesCount] = i;
            triangles[triangleVerticesCount + 1] = i = i + 2;
            triangles[triangleVerticesCount + 2] = i = i - 1;
            triangles[triangleVerticesCount + 3] = i = i - 1;
            triangles[triangleVerticesCount + 4] = i = i + 3;
            triangles[triangleVerticesCount + 5] = i = i - 1;

            triangleVerticesCount += 6;
            verticesCount += 4;
        }

        //     int[] triangles = {
        //         0, 2, 1, //face top
        //          0, 3, 2,

        //         4, 6, 5, //face bottom
        //          4, 7, 6,

        //         8, 10, 9, //face front
        //          8, 11, 6,

        //         0, 7, 4, //face back
        //          0, 4, 3,

        //         5, 4, 7, //face right
        //          5, 7, 6,

        //         0, 6, 7, //face left
        //          0, 1, 6
        //     };

        Debug.LogWarning(triangles.Length);
        Mesh mesh = new Mesh();
        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.uv = uv;
        mesh.name = "generated mesh";
        mesh.Optimize();
        mesh.RecalculateNormals();
        //NormalSolver.RecalculateNormals(mesh, 60);
        GetComponent<MeshFilter>().sharedMesh = mesh;
    }


}

