using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
[ExecuteInEditMode]

public class DiamondGenerator : MonoBehaviour
{

    void Start()
    {
        
    }

    private List<Vector3> CreateDiamond(float radius, float height, float heightPeak, int edges)
    {
        List<Vector3> spawnPoints = new List<Vector3>();
        for (int loopCount = 0; edges > loopCount; loopCount++)
        {
            Vector3 spawnPoint = DrawCircularVertices(radius, -height, edges, loopCount);
            spawnPoints.Add(spawnPoint);
        }

        for (int loopCount = 0; edges > loopCount; loopCount++)
        {
            Vector3 spawnPoint = DrawCircularVertices(radius, height, edges, loopCount);
            spawnPoints.Add(spawnPoint);
        }

        spawnPoints.Add(transform.position + (Vector3.up * (height + heightPeak)));
        spawnPoints.Add(transform.position - (Vector3.up * (height + heightPeak)));

        return spawnPoints;
    }

    private Vector3 DrawCircularVertices(float radius, float height, int edges, int loopCount)
    {
        Vector3 spawnPoint;
        int degree = (360 / edges) * loopCount;
        float radian = degree * Mathf.Deg2Rad;
        float x = Mathf.Cos(radian);
        float z = Mathf.Sin(radian);
        spawnPoint = new Vector3(x, 0, z) * radius;
        spawnPoint.y = -height / 2;
        spawnPoint += transform.position;
        return spawnPoint;
    }

    private void DrawLines(List<Vector3> spawnPoints, int edges)
    {
        Gizmos.color = Color.blue;
        for (int loopCount = 0; edges > loopCount; loopCount++)
        {
            //Draw vertical lines       
            Gizmos.DrawLine(spawnPoints[loopCount], spawnPoints[loopCount+edges]);

            //Draw horizontal lines       
            if(loopCount < edges - 1)
            {
                Gizmos.DrawLine(spawnPoints[loopCount], spawnPoints[loopCount + 1]);
                Gizmos.DrawLine(spawnPoints[loopCount + edges], spawnPoints[loopCount + edges + 1]);
            } else
            {
                Gizmos.DrawLine(spawnPoints[edges-1], spawnPoints[0]);
                Gizmos.DrawLine(spawnPoints[edges*2 - 1], spawnPoints[edges]);
            }


            // Draw lines to the peak
            Gizmos.DrawLine(spawnPoints[loopCount + edges], spawnPoints[(edges * 2) + 1]);
            Gizmos.DrawLine(spawnPoints[loopCount], spawnPoints[(edges*2)]);
        }
    }

    void OnDrawGizmosSelected()
    {
        int edges = 12;

        List<Vector3> spawnList = CreateDiamond(4f, 8f, 1f, edges);

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

