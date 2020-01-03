using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace RockBuilder
{
    [CustomEditor(typeof(CustomRock))]
    public class CustomRockHandles : Editor
    {
        CustomRock rock;

        void OnEnable()
        {
            rock = (CustomRock)target;
            //RockBuilderWindow.ShowWindow();
            //Debug.Log("Current Pipeline: " + RenderPipelineManager.currentPipeline);
        }

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
        }

        void OnSceneGUI()
        {
            if (rock.rockBuildPoints != null)
            {
                Handles.color = new Color(0.0f, 1f, 1f, 0.6f);
                for (int index = 0; index < rock.rockBuildPoints.Count; index++)
                {
                    Vector3 position = rock.rockBuildPoints[index] + rock.transform.position;
                    rock.rockBuildPoints[index] = Handles.PositionHandle(position, Quaternion.identity);
                    Handles.CubeHandleCap(
                    index,
                    position,
                    Quaternion.identity,
                    0.5f,
                    EventType.Repaint
                );

                    rock.rockBuildPoints[index] = rock.rockBuildPoints[index] - rock.transform.position;
                }
            }

            if(rock != null){
                Handles.Label(rock.transform.position + new Vector3(0f, 1f, 0f), "Center point");
            }

             if (rock.mesh != null && rock.mesh.vertices != null) 
            {   
                int loopCount = 0;
                    foreach(Vector3 vertex in rock.mesh.vertices){
                        Handles.Label(rock.transform.position + vertex, loopCount + ". " + vertex);
                        loopCount++;
                    }

            }

            Handles.BeginGUI();

            GUILayout.BeginHorizontal();

            GUILayout.Space(50);

            if (GUILayout.Button("Edit Rock", GUILayout.Width(150)))
            {
                RockBuilderWindow.ShowWindow();
            }

            if (GUILayout.Button("Add point", GUILayout.Width(150)))
            {
                rock.AddNewBuildPoint();
            }

            GUILayout.EndHorizontal();

            Handles.EndGUI();
        }
    }
}
