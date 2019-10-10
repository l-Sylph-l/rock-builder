using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(TestCubeGeneration))]
public class RockBuilderHandles : Editor
{

    TestCubeGeneration testCube;

    void OnEnable()
    {
        testCube = (TestCubeGeneration)target;
    }


    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
    }

    void OnSceneGUI()
    {
        Handles.Label(testCube.transform.position + new Vector3(0f, 2f, 0f), testCube.name);
    }
}
