using UnityEngine;
using UnityEditor;

public class RockBuilderWindow : EditorWindow
{
    string firstParam = "Object Name";
    Color color;

    [MenuItem("Tools/RockBuilder")]
    public static void ShowWindow ()
    {
        GetWindow<RockBuilderWindow>("Rock Builder");
    }

    void OnGUI ()
    {
        // Code für das UI des RockBuilder Fensters
        GUILayout.Label("Rock Builder 2019", EditorStyles.boldLabel);
        firstParam = EditorGUILayout.TextField("Object Name", firstParam);

        // Color Test
        color = EditorGUILayout.ColorField("Color", color);

        if (GUILayout.Button("Colorize Objects"))
        {
            Debug.Log("Stones Button was pressed"); // Gibt eine Logmeldung aus

            Colorize();
        }
    }

    // Testmethode um etwas einzufärben
    void Colorize()
    {
        foreach (GameObject obj in Selection.gameObjects)
        {
            Renderer renderer = obj.GetComponent<Renderer>();

            if (renderer != null)
            {
                renderer.sharedMaterial.color = color;
            }
        }
    }
}
