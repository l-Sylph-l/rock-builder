using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;
using System.IO;
using RockBuilder;

namespace RockBuilder
{
    public class RockBuilderWindow : EditorWindow
    {
        private GUIStyle guiStyle = new GUIStyle();
        private GUIStyle infoText = new GUIStyle();
        private GUIStyle guiColor = new GUIStyle();
        int toolbarInt = 0;
        string[] toolbarStrings = { "Rocks", "Gemstones", "Help" };

        // Parameter für die Steine
        string firstParameterRocks = "Rock_01"; // Objektname
        bool secondParameterRocks = false; // Eigene Shape erstellen
        int thirdParamaterRocks = 10; // Polycount
        int fourthParamaterRocks = 0; // LODs
        private Material rockMaterial; // Material

        // Parameter für die Kristalle/Edelsteine
        string firstParameterGemstones = "Gemstone_01"; // Objektname
        string secondParameterGemstones = ""; // Shape
        int thirdParamaterGemstones = 3; // Vertices
        float fourthParamaterGemstones = 1.0f; // Radius
        float fifthParamaterGemstones = 1.0f; // Height
        float sixthParamaterGemstones = 1.0f; // Peak Height
        bool seventhParameterGemstones = false; // Smooth
        int eightParamaterGemstones = 0; // LODs
        private Material gemstoneMaterial; // Material
        float specialParameterDiamond = 0.5f; // Top Radius Diamant

        // Objecktreferenz für einen Kristall
        Crystal crystal;

        [MenuItem("Tools/RockBuilder")]

        public static void ShowWindow()
        {
            GetWindow<RockBuilderWindow>("Rock Builder");
        }

        void OnGUI()
        {
            // Code für das UI des RockBuilder Fensters ALLE Tabs
            toolbarInt = GUILayout.Toolbar(toolbarInt, toolbarStrings);

            // Anzeige für den "Rocks"-Teil
            if (toolbarInt == 0)
            {
                GUILayout.Space(5);

                guiStyle.fontSize = 20;
                guiStyle.margin = new RectOffset(3, 0, 0, 0);
                guiColor.normal.textColor = Color.black;
                GUILayout.Label("Rock Builder - Rocks", guiStyle);

                GUILayout.Space(20);

                // Erster Rocks-Parameter => Der Name des Objekts(Steins), welches anschliessend generiert werden soll           
                firstParameterRocks = EditorGUILayout.TextField("Object Name", firstParameterRocks);

                // Zweiter Rocks-Paramter => Die Checkbox, ob der User eine eigene Form extruden oder lieber die Defaultform (rund) haben möchte           
                secondParameterRocks = EditorGUILayout.Toggle("Make your own shape", secondParameterRocks);
                EditorGUILayout.HelpBox("By clicking 'Make your own shape', a cube will appear in the editor window. Extrude his faces to make the shape you want. If you don't click the checkbox, the generated rock will be round by default.", MessageType.Info);

                // Dritter Rocks-Parameter => Slidebar für den gewünschten Polycount zwischen 10 und 10'000  
                thirdParamaterRocks = EditorGUILayout.IntSlider("Polycount", thirdParamaterRocks, 10, 10000);

                // Vierter Rocks-Parameter => Slidebar für die Anzahl der LODs  
                fourthParamaterRocks = EditorGUILayout.IntSlider("LODs", fourthParamaterRocks, 0, 3);

                GUILayout.Space(15);

                // Auswahl der Stein-Materialien      
                GUILayout.Label("Choose Material");

                GUILayout.Space(5);

                // Diese Prüfung entscheidet, welche Shader angezeigt werden => Lightweight/Universal oder HD-Renderpipeline
                if (RenderPipelineManager.currentPipeline != null && RenderPipelineManager.currentPipeline.ToString().Contains("HD"))
                {
                    rockMaterial = (Material)EditorGUILayout.ObjectField(rockMaterial, typeof(Material), true);
                }
                // else steht für die Lightweight/Universal Pipeline oder gar keine
                else
                {

                }

                GUILayout.Space(15);

                // Button für das Generieren des Steins
                if (GUILayout.Button("Let's rock!", GUILayout.Height(25)))
                {
                    Debug.Log("Rocks-Generate Button was pressed"); // Gibt eine Logmeldung aus
                }
            }

            // Anzeige für den "Gemstones"-Teil
            if (toolbarInt == 1)
            {
                GUILayout.Space(5);

                guiStyle.fontSize = 20;
                guiColor.normal.textColor = Color.black;
                GUILayout.Label("Rock Builder - Gemstones", guiStyle);

                GUILayout.Space(20);

                // Erster Gemstones-Parameter => Der Name des Objekts, welches anschliessend generiert werden soll           
                firstParameterGemstones = EditorGUILayout.TextField("Object Name", firstParameterGemstones);

                GUILayout.Space(15);

                // Auswahl der Art des Kristalls/Edelstein     
                GUILayout.Label("Choose Shape");

                GUILayout.Space(5);

                EditorGUI.BeginDisabledGroup(true);
                EditorGUILayout.TextField(secondParameterGemstones);
                EditorGUI.EndDisabledGroup();

                GUILayout.Space(10);

                GUILayout.BeginHorizontal();
                GUILayout.Box(LoadPNG("Assets/Rock Builder/Assets/Images/Crystal_icon.png"), new GUILayoutOption[] { GUILayout.Width(30), GUILayout.Height(30) });
                if (GUILayout.Button(" Crystal ", GUILayout.Height(60)))
                {
                    Debug.Log("Crystal Button was pressed"); // Gibt eine Logmeldung aus
                    secondParameterGemstones = "Crystal";
                    crystal = CrystalService.Instance.CreateEmptyCrystal(firstParameterGemstones);
                }
                GUILayout.Box(LoadPNG("Assets/Rock Builder/Assets/Images/Gem_icon.png"), new GUILayoutOption[] { GUILayout.Width(30), GUILayout.Height(30) });
                if (GUILayout.Button("   Gem   ", GUILayout.Height(60)))
                {
                    Debug.Log("Gem Button was pressed"); // Gibt eine Logmeldung aus
                    secondParameterGemstones = "Gem";
                }
                GUILayout.Box(LoadPNG("Assets/Rock Builder/Assets/Images/Diamond_icon.png"), new GUILayoutOption[] { GUILayout.Width(30), GUILayout.Height(30) });
                if (GUILayout.Button("Diamond", GUILayout.Height(60)))
                {
                    Debug.Log("Diamond Button was pressed"); // Gibt eine Logmeldung aus
                    secondParameterGemstones = "Diamond";
                }
                GUILayout.EndHorizontal();

                GUILayout.Space(15);

                // Der zweite Teil der Gemstones, wird erst eingeblendet, wenn eine Shape ausgewählt wurde
                if (secondParameterGemstones != "" && crystal != null)
                {
                    UpdateCrystal();

                    // Dritter Gemstones-Parameter => Slidebar für die Anzahl Vertices  
                    thirdParamaterGemstones = EditorGUILayout.IntSlider("Vertices", thirdParamaterGemstones, 3, 200);

                    // Beschränkt die Usereingaben für den Radius => 0.01 - 1000
                    if (fourthParamaterGemstones < 0.009 || fourthParamaterGemstones > 1000)
                    {
                        fourthParamaterGemstones = 1.0f;
                    }
                    // Vierter Gemstones-Parameter für den Radius der Objekte
                    fourthParamaterGemstones = EditorGUILayout.FloatField("Radius", fourthParamaterGemstones);

                    // Diese Eingabe wird nur bei Diamanten benötigt, deshalb wird hier geprüft
                    if (secondParameterGemstones == "Diamond")
                    {
                        // Beschränkt die Usereingaben für den Top Radius der Diamanten => 0.01 - 1000
                        if (specialParameterDiamond < 0.009 || specialParameterDiamond > 1000)
                        {
                            specialParameterDiamond = 0.5f;
                        }
                        specialParameterDiamond = EditorGUILayout.FloatField("Top Radius", specialParameterDiamond);
                    }

                    // Beschränkt die Usereingaben für die Höhe => 0.01 - 1000
                    if (fifthParamaterGemstones < 0.009 || fifthParamaterGemstones > 1000)
                    {
                        fifthParamaterGemstones = 1.0f;
                    }
                    // Fünfter Gemstones-Parameter für die Höhe der Objekte
                    fifthParamaterGemstones = EditorGUILayout.FloatField("Height", fifthParamaterGemstones);

                    // Beschränkt die Usereingaben für die Höhe => 0.01 - 100
                    if (sixthParamaterGemstones < 0.009 || sixthParamaterGemstones > 100)
                    {
                        sixthParamaterGemstones = 1.0f;
                    }
                    // Sechster Gemstones-Parameter für die Höhe der Spitze
                    sixthParamaterGemstones = EditorGUILayout.FloatField("Peak Height", sixthParamaterGemstones);

                    // Siebter Gemstones-Paramter => Die Checkbox, um ein Objekt zu smoothen         
                    seventhParameterGemstones = EditorGUILayout.Toggle("Smooth", seventhParameterGemstones);

                    // Achter Gemstones-Parameter => Slidebar für die Anzahl der LODs  
                    eightParamaterGemstones = EditorGUILayout.IntSlider("LODs", eightParamaterGemstones, 0, 3);

                    GUILayout.Space(15);

                    // Auswahl der Gemstones-Materialien      
                    GUILayout.Label("Choose Material");

                    GUILayout.Space(5);

                    // Diese Prüfung entscheidet, welche Shader angezeigt werden => Lightweight/Universal oder HD-Renderpipeline (wird momentan nicht mehr benötigt)
                    //if (RenderPipelineManager.currentPipeline != null && RenderPipelineManager.currentPipeline.ToString().Contains("HD"))
                    gemstoneMaterial = (Material)EditorGUILayout.ObjectField(gemstoneMaterial, typeof(Material), true);

                    GUILayout.Space(15);

                    // Button für das Generieren des Gemstones
                    if (GUILayout.Button("Let's rock!", GUILayout.Height(25)))
                    {

                        // generate existing mesh if diamondgenerator exists, otherwise create a new diamond generator
                        if (crystal)
                        {
                            crystal = CrystalService.Instance.CreateCrystal(crystal);
                            crystal.GetComponent<MeshRenderer>().material = gemstoneMaterial;
                        }
                    }
                }
            }

            // Anzeige für den Helpteil => Infos etc.
            if (toolbarInt == 2)
            {
                GUILayout.Space(5);

                infoText.wordWrap = true;
                infoText.margin = new RectOffset(5, 5, 0, 0);
                guiStyle.fontSize = 20;
                GUILayout.Label("Rock Builder - Help", guiStyle);

                GUILayout.Space(15);

                EditorGUILayout.HelpBox("Rock Builder is a simple tool to create rocks and gemstones with different shaders. It's made by two students from the SAE Zurich as their bachelor thesis.", MessageType.Info);

                GUILayout.Space(15);

                if (GUILayout.Button("Open user manual", GUILayout.Height(25)))
                {
                    // Öffnet das Usermanual PDF-File
                    Application.OpenURL(System.Environment.CurrentDirectory + "/Assets/Rock Builder/Assets/Resources/Dummy.pdf");
                    Debug.Log("User manual Button was pressed"); // Gibt eine Logmeldung aus
                }

                GUILayout.Space(20);

                GUILayout.Label("Contact", guiStyle);

                GUILayout.Space(15);

                GUILayout.Label("Please send us an email if you have any questions or if you have problems with the tool. We do our best to answer the email as fast as possible. Thank you!", infoText);

                GUILayout.Space(15);

                // Mit einem Klick öffnet sich direkt die Mailmaske
                if (GUILayout.Button("Email: rockbuilder-help@hotmail.com", EditorStyles.label))
                    Application.OpenURL("mailto:rockbuilder-help@hotmail.com?");
            }
        }

        private void Update()
        {
            CheckIfCrystalSelected();
        }

        // Wird benötigt um PNG-Dateien auf dem UI anzeigen zu können
        private Texture2D LoadPNG(string filePath)
        {

            Texture2D tex = null;
            byte[] fileData;

            if (File.Exists(filePath))
            {
                fileData = File.ReadAllBytes(filePath);
                tex = new Texture2D(2, 2);
                tex.LoadImage(fileData); // This will auto-resize the texture dimensions
            }
            return tex;
        }

        private void CheckIfCrystalSelected()
        {
            if (crystal == null)
            {
                crystal = CrystalService.Instance.GetCrystalFromSelection();
                if (crystal != null)
                {
                    firstParameterGemstones = crystal.name;
                    secondParameterGemstones = "Crystal";
                    thirdParamaterGemstones = crystal.edges;
                    fourthParamaterGemstones = crystal.radius;
                    fifthParamaterGemstones = crystal.height;
                    sixthParamaterGemstones = crystal.heightPeak;
                    seventhParameterGemstones = crystal.smooth;
                    if(crystal.GetComponent<MeshRenderer>().sharedMaterial != null)
                    {
                        gemstoneMaterial = crystal.GetComponent<MeshRenderer>().sharedMaterial;
                    }
                    this.Repaint();
                }
            } else
            {
                if(crystal != CrystalService.Instance.GetCrystalFromSelection())
                {
                    crystal = null;
                    this.Repaint();
                }
            }
        }

        private void UpdateCrystal()
        {
            if(crystal != null)
            {
                crystal.edges = thirdParamaterGemstones; // Vertices
                crystal.radius = fourthParamaterGemstones; // Radius
                crystal.height = fifthParamaterGemstones; // Height
                crystal.heightPeak = sixthParamaterGemstones; // Peak Height
                crystal.smooth = seventhParameterGemstones; // Smooth
            }
        }
    }
}