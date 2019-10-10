using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
using UnityEngine.EventSystems;

public class RockBuilderWindow : EditorWindow
{
    private GUIStyle guiStyle = new GUIStyle();
    private GUIStyle guiColor = new GUIStyle();
    int toolbarInt = 0;
    string[] toolbarStrings = { "Hello", "Stones", "Crystals" };

    // Alle Bilder für die Buttons (funktioniert leider nocht nicht...)
    private Texture shape_crystal;
    private Texture shape_gem;
    private Texture shape_diamond;

    // Parameter für die Steine
    string firstParameterStones = "Stone_01";
    bool secondParameterStones = false;
    float thirdParamaterStones = 10.0f;
    int fourthParamaterStones = 0;
    string fifthParameterStones = "MAT_Granite";

    // Parameter für die Kristalle/Edelsteine
    string firstParameterCrystals = "Crystal_01";
    string secondParameterCrystals = "Crystal";
    float thirdParamaterCrystals = 3.0f;
    float fourthParamaterCrystals = 1.0f;
    float fifthParamaterCrystals = 1.0f;
    bool sixthParameterStones = false;
    string seventhParameterCrystals = "MAT_Gem_01";

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
            thirdParamaterStones = EditorGUILayout.Slider("Polycount", thirdParamaterStones, 10, 10000);

            // Vierter Stones-Parameter => Slidebar für die Anzahl der LODs  
            fourthParamaterStones = EditorGUILayout.IntSlider("LODs", fourthParamaterStones, 0, 3);

            GUILayout.Space(15);

            // Auswahl der Stein-Materialien      
            GUILayout.Label("Choose Material");

            GUILayout.Space(5);

            EditorGUI.BeginDisabledGroup(true);
            EditorGUILayout.TextField(fifthParameterStones);
            EditorGUI.EndDisabledGroup();

            GUILayout.Space(10);

            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Granite", GUILayout.Height(60), GUILayout.Width(55)))
            {
                Debug.Log("MAT_Granite Button was pressed"); // Gibt eine Logmeldung aus
                fifthParameterStones = "MAT_Granite";
            }
            if (GUILayout.Button("Sandstone", GUILayout.Height(60), GUILayout.Width(55)))
            {
                Debug.Log("MAT_Sandstone Button was pressed"); // Gibt eine Logmeldung aus
                fifthParameterStones = "MAT_Sandstone";
            }
            if (GUILayout.Button("Limestone", GUILayout.Height(60), GUILayout.Width(55)))
            {
                Debug.Log("MAT_Limestone Button was pressed"); // Gibt eine Logmeldung aus
                fifthParameterStones = "MAT_Limestone";
            }
            GUILayout.EndHorizontal();
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
            thirdParamaterCrystals = EditorGUILayout.Slider("Vertices", thirdParamaterCrystals, 3, 200);

            // Beschränkt die Usereingaben für den Radius => 1 - 1000
            if (fourthParamaterCrystals < 1 || fourthParamaterCrystals > 1000)
            {
                fourthParamaterCrystals = 1.0f;
            }
            // Vierter Crystal-Parameter für den Radius der Objekte
            fourthParamaterCrystals = EditorGUILayout.FloatField("Radius", fourthParamaterCrystals);

            // Beschränkt die Usereingaben für die Höhe => 1 - 1000
            if (fifthParamaterCrystals < 1 || fifthParamaterCrystals > 1000)
            {
                fifthParamaterCrystals = 1.0f;
            }
            // Fünfter Crystal-Parameter für die Höhe der Objekte
            fifthParamaterCrystals = EditorGUILayout.FloatField("Height", fifthParamaterCrystals);

            // Sechster Crystal-Paramter => Die Checkbox, um ein Objekt zu smoothen         
            sixthParameterStones = EditorGUILayout.Toggle("Smooth", sixthParameterStones);

            GUILayout.Space(15);

            // Auswahl der Kristall-Materialien      
            GUILayout.Label("Choose Material");

            GUILayout.Space(5);

            EditorGUI.BeginDisabledGroup(true);
            EditorGUILayout.TextField(seventhParameterCrystals);
            EditorGUI.EndDisabledGroup();

            GUILayout.Space(10);

            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Gem 01", GUILayout.Height(60), GUILayout.Width(55)))
            {
                Debug.Log("MAT_Gem_01 Button was pressed"); // Gibt eine Logmeldung aus
                seventhParameterCrystals = "MAT_Gem_01";
            }
            if (GUILayout.Button("Gem 02", GUILayout.Height(60), GUILayout.Width(55)))
            {
                Debug.Log("MAT_Gem_02 Button was pressed"); // Gibt eine Logmeldung aus
                seventhParameterCrystals = "MAT_Gem_02";
            }
            if (GUILayout.Button("Gem 03", GUILayout.Height(60), GUILayout.Width(55)))
            {
                Debug.Log("MAT_Gem_03 Button was pressed"); // Gibt eine Logmeldung aus
                seventhParameterCrystals = "MAT_Gem_03";
            }
            GUILayout.EndHorizontal();
        }
    }
}
