using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;

public class RockBuilderWindow : EditorWindow
{
    private GUIStyle guiStyle = new GUIStyle();
    private GUIStyle guiColor = new GUIStyle();
    int toolbarInt = 0;
    string[] toolbarStrings = { "Hello", "Stones", "Crystals" };

    // Parameter für die Steine
    string firstParameterStones = "Stone_01"; // Objektname
    bool secondParameterStones = false; // Eigene Shape erstellen
    int thirdParamaterStones = 10; // Polycount
    int fourthParamaterStones = 0; // LODs
    private Material stoneMaterial; // Material

    // Parameter für die Kristalle/Edelsteine
    string firstParameterCrystals = "Crystal_01"; // Objektname
    string secondParameterCrystals = "Crystal"; // Shape
    int thirdParamaterCrystals = 3; // Vertices
    float fourthParamaterCrystals = 1.0f; // Radius
    float fifthParamaterCrystals = 1.0f; // Height
    float sixthParamaterCrystals = 1.0f; // Peak Height
    bool seventhParameterCrystals = false; // Smooth
    private Material crystalMaterial; // Material

    [MenuItem("Tools/RockBuilder")]

    public static void ShowWindow()
    {
        GetWindow<RockBuilderWindow>("Rock Builder");
    }

    void OnGUI()
    {
        // Code für das UI des RockBuilder Fensters ALLE Tabs
        toolbarInt = GUILayout.Toolbar(toolbarInt, toolbarStrings);

        // Anzeige für den Startteil => Auswahl ob Steine oder Kristalle + Infos
        if (toolbarInt == 0)
        {
            GUILayout.Space(5);

            guiStyle.fontSize = 20;
            GUILayout.Label("Rock Builder", guiStyle);

            GUILayout.Space(15);

            EditorGUILayout.HelpBox("Rock Builder is a simple tool to create stones and crystals with different shaders. It's made by two students from the SAE Zurich as their bachelor thesis. Have fun!", MessageType.Info);

            GUILayout.Space(20);

            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Stones"))
            {
                Debug.Log("Stone Button was pressed"); // Gibt eine Logmeldung aus
                toolbarInt = 1;
            }
            if (GUILayout.Button("Crystals"))
            {
                Debug.Log("Crystal Button was pressed"); // Gibt eine Logmeldung aus
                toolbarInt = 2;
            }
            GUILayout.EndHorizontal();
        }

        // Anzeige für den "Stones"-Teil
        if (toolbarInt == 1)
        {
            GUILayout.Space(5);

            guiStyle.fontSize = 20;
            guiColor.normal.textColor = Color.black;
            GUILayout.Label("Rock Builder - Stones", guiStyle);

            GUILayout.Space(20);

            // Erster Stones-Parameter => Der Name des Objekts(Steins), welches anschliessend generiert werden soll           
            firstParameterStones = EditorGUILayout.TextField("Object Name", firstParameterStones);

            // Zweiter Stones-Paramter => Die Checkbox, ob der User eine eigene Form extruden oder lieber die Defaultform (rund) haben möchte           
            secondParameterStones = EditorGUILayout.Toggle("Make your own shape", secondParameterStones);
            EditorGUILayout.HelpBox("By clicking 'Make your own shape', a cube will appear in the editor window. Extrude his faces to make the shape you want. If you don't click the checkbox, the generated stone will be round by default.", MessageType.Info);

            // Dritter Stones-Parameter => Slidebar für den gewünschten Polycount zwischen 10 und 10'000  
            thirdParamaterStones = EditorGUILayout.IntSlider("Polycount", thirdParamaterStones, 10, 10000);

            // Vierter Stones-Parameter => Slidebar für die Anzahl der LODs  
            fourthParamaterStones = EditorGUILayout.IntSlider("LODs", fourthParamaterStones, 0, 3);

            GUILayout.Space(15);

            // Auswahl der Stein-Materialien      
            GUILayout.Label("Choose Material");

            GUILayout.Space(5);

            // Diese Prüfung entscheidet, welche Shader angezeigt werden => Lightweight/Universal oder HD-Renderpipeline
            if (RenderPipelineManager.currentPipeline != null && RenderPipelineManager.currentPipeline.ToString().Contains("HD"))
            {
                stoneMaterial = (Material)EditorGUILayout.ObjectField(stoneMaterial, typeof(Material), true);
            }
            // else steht für die Lightweight/Universal Pipeline oder gar keine
            else
            {

            }

            GUILayout.Space(15);

            // Button für das Generieren des Steins
            if (GUILayout.Button("Let's rock!", GUILayout.Height(25)))
            {
                Debug.Log("Stone-Generate Button was pressed"); // Gibt eine Logmeldung aus
            }
        }

        // Anzeige für den "Crystals"-Teil
        if (toolbarInt == 2)
        {
            GUILayout.Space(5);

            guiStyle.fontSize = 20;
            guiColor.normal.textColor = Color.black;
            GUILayout.Label("Rock Builder - Crystals", guiStyle);

            GUILayout.Space(20);

            // Erster Crystal-Parameter => Der Name des Objekts, welches anschliessend generiert werden soll           
            firstParameterCrystals = EditorGUILayout.TextField("Object Name", firstParameterCrystals);

            GUILayout.Space(15);

            // Auswahl der Art des Kristalls/Edelstein     
            GUILayout.Label("Choose Shape");

            GUILayout.Space(5);

            EditorGUI.BeginDisabledGroup(true);
            EditorGUILayout.TextField(secondParameterCrystals);
            EditorGUI.EndDisabledGroup();

            GUILayout.Space(10);

            GUILayout.BeginHorizontal();
            if (GUILayout.Button(" Crystal ", GUILayout.Height(60)))
            {
                Debug.Log("Crystal Button was pressed"); // Gibt eine Logmeldung aus
                secondParameterCrystals = "Crystal";
            }
            if (GUILayout.Button("   Gem   ", GUILayout.Height(60)))
            {
                Debug.Log("Gem Button was pressed"); // Gibt eine Logmeldung aus
                secondParameterCrystals = "Gem";
            }
            if (GUILayout.Button("Diamond", GUILayout.Height(60)))
            {
                Debug.Log("Diamond Button was pressed"); // Gibt eine Logmeldung aus
                secondParameterCrystals = "Diamond";
            }
            GUILayout.EndHorizontal();

            GUILayout.Space(15);

            // Dritter Crystal-Parameter => Slidebar für die Anzahl Vertices  
            thirdParamaterCrystals = EditorGUILayout.IntSlider("Vertices", thirdParamaterCrystals, 3, 200);

            // Beschränkt die Usereingaben für den Radius => 0.1 - 1000
            if (fourthParamaterCrystals < 0.1 || fourthParamaterCrystals > 1000)
            {
                fourthParamaterCrystals = 1.0f;
            }
            // Vierter Crystal-Parameter für den Radius der Objekte
            fourthParamaterCrystals = EditorGUILayout.FloatField("Radius", fourthParamaterCrystals);

            // Beschränkt die Usereingaben für die Höhe => 0.1 - 1000
            if (fifthParamaterCrystals < 0.1 || fifthParamaterCrystals > 1000)
            {
                fifthParamaterCrystals = 1.0f;
            }
            // Fünfter Crystal-Parameter für die Höhe der Objekte
            fifthParamaterCrystals = EditorGUILayout.FloatField("Height", fifthParamaterCrystals);

            // Beschränkt die Usereingaben für die Höhe => 0.1 - 100
            if (sixthParamaterCrystals < 0.1 || sixthParamaterCrystals > 100)
            {
                sixthParamaterCrystals = 1.0f;
            }
            // Sechster Crystal-Parameter für die Höhe der Spitze
            sixthParamaterCrystals = EditorGUILayout.FloatField("Peak Height", sixthParamaterCrystals);

            // Siebter Crystal-Paramter => Die Checkbox, um ein Objekt zu smoothen         
            seventhParameterCrystals = EditorGUILayout.Toggle("Smooth", seventhParameterCrystals);

            GUILayout.Space(15);

            // Auswahl der Kristall-Materialien      
            GUILayout.Label("Choose Material");

            GUILayout.Space(5);

            // Diese Prüfung entscheidet, welche Shader angezeigt werden => Lightweight/Universal oder HD-Renderpipeline (wird momentan nicht mehr benötigt)
            //if (RenderPipelineManager.currentPipeline != null && RenderPipelineManager.currentPipeline.ToString().Contains("HD"))
            crystalMaterial = (Material)EditorGUILayout.ObjectField(crystalMaterial, typeof(Material), true);

            GUILayout.Space(15);

            // Variable for the diamond generator
            DiamondGenerator diamondGenerator = null;
            if (Selection.activeGameObject)
            {
                diamondGenerator = Selection.activeGameObject.GetComponent<DiamondGenerator>();
            }

            // update all values if diamondgenerator isn't null
            if (diamondGenerator)
            {
                diamondGenerator.previewRadius = fourthParamaterCrystals;
                diamondGenerator.previewHeight = fifthParamaterCrystals;
                diamondGenerator.previewHeightPeak = sixthParamaterCrystals;
                diamondGenerator.previewEdges = thirdParamaterCrystals;
                diamondGenerator.showPreview = true;
            }

            // Button für das Generieren des Kristalls
            if (GUILayout.Button("Let's rock!", GUILayout.Height(25)))
            {
                // generate existing mesh if diamondgenerator exists, otherwise create a new diamond generator
                if (diamondGenerator)
                {
                    diamondGenerator.CreateMesh(fourthParamaterCrystals, fifthParamaterCrystals, sixthParamaterCrystals, thirdParamaterCrystals, seventhParameterCrystals).material = crystalMaterial;
                }
                else
                {
                    Transform cameraTransform = SceneView.lastActiveSceneView.camera.transform;             
                    diamondGenerator = new GameObject().AddComponent(typeof(DiamondGenerator)) as DiamondGenerator;
                    diamondGenerator.transform.position = (cameraTransform.forward * (fourthParamaterCrystals * 3f + fifthParamaterCrystals * 2f)) + cameraTransform.position;
                    cameraTransform.LookAt(diamondGenerator.transform);
                    diamondGenerator.CreateMesh(fourthParamaterCrystals, fifthParamaterCrystals, sixthParamaterCrystals, thirdParamaterCrystals, seventhParameterCrystals).material = crystalMaterial;
                    Debug.Log("Crystal-Generate Button was pressed"); // Gibt eine Logmeldung aus
                }
            }
        }
    }
}
