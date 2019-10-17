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
            Vector3 spawnPoint;
            int degree = (360 / edges) * loopCount;
            float radian = degree * Mathf.Deg2Rad;
            float x = Mathf.Cos(radian);
            float z = Mathf.Sin(radian);
            spawnPoint = new Vector3(x, 0, z) * radius;
            spawnPoint.y = -height / 2;
            spawnPoint += transform.position;
            spawnPoints.Add(spawnPoint);
        }

        for (int loopCount = 0; edges > loopCount; loopCount++)
        {
            Vector3 spawnPoint;
            float degree = (360f / edges) * loopCount;
            float radian = degree * Mathf.Deg2Rad;
            float x = Mathf.Cos(radian);
            float z = Mathf.Sin(radian);
            spawnPoint = new Vector3(x, 0, z) * radius;
            spawnPoint.y = height / 2;
            spawnPoint += transform.position;
            spawnPoints.Add(spawnPoint);
        }


        spawnPoints.Add(transform.position + (Vector3.up * (height + heightPeak)));
        spawnPoints.Add(transform.position - (Vector3.up * (height + heightPeak)));
        return spawnPoints;
    }

    void OnDrawGizmosSelected()
    {
        List<Vector3> spawnList = CreateDiamond(4f, 8f, 1f, 20);

        foreach(Vector3 spawnPosition in spawnList)
        {
            // Draw a yellow sphere at the transform's position
            Gizmos.color = Color.black;
            Gizmos.DrawSphere(spawnPosition, 0.3f);
        }
    }
}

