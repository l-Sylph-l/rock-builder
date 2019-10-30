using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.Rendering;
using RockBuilder;

[CustomEditor(typeof(CrystalGenerator))]
public class RockBuilderHandles : Editor
{

    CrystalGenerator diamond;

    void OnEnable()
    {
        diamond = (CrystalGenerator)target;
        //RockBuilderWindow.ShowWindow();
        //Debug.Log("Current Pipeline: " + RenderPipelineManager.currentPipeline);
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
    }

    void OnSceneGUI()
    {
        Handles.Label(diamond.transform.TransformPoint(new Vector3(0f, diamond.previewHeight + diamond.previewHeightPeak, 0f)), "Vertices: " + diamond.GetComponent<MeshFilter>().sharedMesh.vertices.Length);

        Handles.BeginGUI();

        GUILayout.BeginHorizontal();

        GUILayout.Space(50);

        if (GUILayout.Button("Edit Diamond", GUILayout.Width(150)))
        {
            RockBuilderWindow.ShowWindow();
        }

        GUILayout.EndHorizontal();

        Handles.EndGUI();
    }
}
