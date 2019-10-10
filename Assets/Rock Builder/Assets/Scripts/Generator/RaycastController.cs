using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[ExecuteInEditMode]

public class RaycastController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //// Called from OnSceneGUI in a subclass of Editor
    //// Camera.current did not work
    ////Ray ray = Camera.current.ScreenPointToRay( Event.current.mousePosition );
    //Ray ray = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);

    //RaycastHit hit;
    //     if(Physics.Raycast(ray, out hit ) )
    //     {
    //         // do stuff
    //     }
}
