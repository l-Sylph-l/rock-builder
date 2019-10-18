using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
[ExecuteInEditMode]
public class DiamondGenerator : MonoBehaviour
{

    public int edges = 10;

    void Start()
    {
        CreateMesh(4f, 8f, 3f, edges);
    }

    private List<Vector3> CreateVertexPositions(float radius, float height, float heightPeak, int edges)
    {
        List<Vector3> spawnPoints = new List<Vector3>();

        // Get the points for the bottom circle
        for (int loopCount = 0; edges > loopCount; loopCount++)
        {
            Vector3 spawnPoint = DrawCircularVertices(radius, -height, edges, loopCount);
            spawnPoints.Add(spawnPoint);
        }

        // Get the points for the upper circle
        for (int loopCount = 0; edges > loopCount; loopCount++)
        {
            Vector3 spawnPoint = DrawCircularVertices(radius, height, edges, loopCount);
            spawnPoints.Add(spawnPoint);
        }

        // Get the points for the upper and bottom peak
        for (int loopCount = 0; edges > loopCount; loopCount++)
        {
            spawnPoints.Add(transform.position + (Vector3.up * (height / 2 + heightPeak)));
        }
        for (int loopCount = 0; edges > loopCount; loopCount++)
        {
            spawnPoints.Add(transform.position - (Vector3.up * (height / 2 + heightPeak)));
        }

        return spawnPoints;
    }

    private Vector3 DrawCircularVertices(float radius, float height, int edges, int loopCount)
    {
        Vector3 spawnPoint;
        float degree = (360f / edges) * loopCount;
        float radian = degree * Mathf.Deg2Rad;
        float x = Mathf.Cos(radian);
        float z = Mathf.Sin(radian);
        spawnPoint = new Vector3(x, 0, z) * radius;
        spawnPoint.y = height / 2;
        spawnPoint += transform.position;
        return spawnPoint;
    }

    private void DrawLines(List<Vector3> spawnPoints, int edges)
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

    public void CreateMesh(float radius, float height, float heightPeak, int edges)
    {

        List<Vector3> vertexPositions = CreateVertexPositions(radius, height, heightPeak, edges);

        Vector3[] vertices = new Vector3[edges * 4];
        Vector2[] uv = new Vector2[edges * 4];
        int vertexLoop = 0;
        float circumference = Vector3.Distance(vertexPositions[0], vertexPositions[1]) * edges;
        float uvHeight = (1f - (height / circumference)) / 2;

        for (int loopCount = 0; edges > loopCount; loopCount++)
        {
            if (edges - 1 != loopCount)
            {
                vertices[vertexLoop] = vertexPositions[edges + loopCount] - transform.position;
                vertices[vertexLoop + 1] = vertexPositions[loopCount] - transform.position;
                vertices[vertexLoop + 2] = vertexPositions[loopCount + 1] - transform.position; ;
                vertices[vertexLoop + 3] = vertexPositions[edges + loopCount + 1] - transform.position;

                if (loopCount == 0)
                {
                    uv[vertexLoop] = new Vector2(0f, 1f - uvHeight);
                    uv[vertexLoop + 1] = new Vector2(0f, uvHeight);
                    uv[vertexLoop + 2] = new Vector2(((float)loopCount + 1f) / (float)edges, uvHeight);
                    uv[vertexLoop + 3] = new Vector2(((float)loopCount + 1f) / (float)edges, 1f - uvHeight);
                }
                else
                {
                    uv[vertexLoop] = new Vector2((float)loopCount / (float)edges, 1f - uvHeight);
                    uv[vertexLoop + 1] = new Vector2((float)loopCount / (float)edges, uvHeight);
                    uv[vertexLoop + 2] = new Vector2(((float)loopCount + 1f) / (float)edges, uvHeight);
                    uv[vertexLoop + 3] = new Vector2(((float)loopCount + 1f) / (float)edges, 1f - uvHeight);
                }

                vertexLoop = vertexLoop + 4;

            }
            else
            {
                vertices[vertexLoop] = vertexPositions[edges + loopCount] - transform.position;
                vertices[vertexLoop + 1] = vertexPositions[loopCount] - transform.position;
                vertices[vertexLoop + 2] = vertexPositions[0] - transform.position;
                vertices[vertexLoop + 3] = vertexPositions[edges] - transform.position;

                uv[vertexLoop] = new Vector2((float)loopCount / (float)edges, 1f - uvHeight);
                uv[vertexLoop + 1] = new Vector2((float)loopCount / (float)edges, uvHeight);
                uv[vertexLoop + 2] = new Vector2(((float)loopCount + 1f) / (float)edges, uvHeight);
                uv[vertexLoop + 3] = new Vector2(((float)loopCount + 1f) / (float)edges, 1f - uvHeight);
            }
        }

        //// Get the vertices for both peaks
        //for (int loopCount = 0; edges*2 > loopCount; loopCount++)
        //{
        //    vertices[vertexLoop] = vertexPositions[edges + loopCount] - transform.position;
        //    vertexLoop++;
        //}

        int[] triangles = new int[edges * 6];
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


        Mesh mesh = new Mesh();
        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.uv = uv;
        mesh.name = "generated diamond mesh";
        mesh.Optimize();
        mesh.RecalculateNormals();
        //NormalSolver.RecalculateNormals(mesh, 60);
        GetComponent<MeshFilter>().sharedMesh = mesh;
    }

    void OnDrawGizmosSelected()
    {
        List<Vector3> spawnList = CreateVertexPositions(4f, 8f, 3f, edges);

        DrawLines(spawnList, edges);

        foreach (Vector3 spawnPosition in spawnList)
        {
            // Draw a yellow sphere at the transform's position
            Gizmos.color = Color.black;
            Gizmos.DrawSphere(spawnPosition, 0.3f);

            Gizmos.color = Color.blue;
        }
    }
}

