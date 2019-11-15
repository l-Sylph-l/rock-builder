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
        float sixthParamaterGemstones = 0.5f; // Peak Height
        bool seventhParameterGemstones = false; // Smooth
        int eightParamaterGemstones = 0; // LODs
        bool ninthParameterGemstones = false; // Collider
        private Material gemstoneMaterial; // Material
        float specialParameterDiamond = 0.5f; // Top Radius Diamant

        // Objecktreferenz für einen Kristall
        Crystal crystal;

        // Objecktreferenz für ein Juwel
        Gem gem;

        // Objecktreferenz für einen Diamant
        Diamond diamond;

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
                if (GUILayout.Button(" Crystal ", GUILayout.Height(60)) && crystal == null)
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
                    gem = GemService.Instance.CreateEmptyGem(firstParameterGemstones);
                }
                GUILayout.Box(LoadPNG("Assets/Rock Builder/Assets/Images/Diamond_icon.png"), new GUILayoutOption[] { GUILayout.Width(30), GUILayout.Height(30) });
                if (GUILayout.Button("Diamond", GUILayout.Height(60)) && diamond == null)
                {
                    Debug.Log("Diamond Button was pressed"); // Gibt eine Logmeldung aus
                    secondParameterGemstones = "Diamond";
                    diamond = DiamondService.Instance.CreateEmptyDiamond(firstParameterGemstones);
                }
                GUILayout.EndHorizontal();

                GUILayout.Space(15);

                // Der zweite Teil der Gemstones, wird erst eingeblendet, wenn eine Shape ausgewählt wurde
                if (secondParameterGemstones != "" && (crystal != null || diamond != null || gem != null))
                {
                    UpdateGemStones();

                    // Dritter Gemstones-Parameter => Slidebar für die Anzahl Vertices
                    if (secondParameterGemstones == "Crystal")
                        thirdParamaterGemstones = EditorGUILayout.IntSlider("Edges", thirdParamaterGemstones, 3, 200);
                    if (secondParameterGemstones == "Diamond" || secondParameterGemstones == "Gem")
                    {
                        // Bei den Diamanten darf die Anzahl Edges nicht ungerade sein, deshalb diese Prüfung hier
                        thirdParamaterGemstones = EditorGUILayout.IntSlider("Edges", thirdParamaterGemstones, 6, 200);
                        if (thirdParamaterGemstones % 2 != 0)
                        {
                            thirdParamaterGemstones = thirdParamaterGemstones + 1;
                        }
                    }

                    // Beschränkt die Usereingaben für den Radius => 0.01 - 1000
                    if (fourthParamaterGemstones < 0.009 || fourthParamaterGemstones > 1000)
                    {
                        fourthParamaterGemstones = 1.0f;
                    }
                    // Vierter Gemstones-Parameter für den Radius der Objekte
                    if (secondParameterGemstones != "Gem")
                        fourthParamaterGemstones = EditorGUILayout.FloatField("Radius", fourthParamaterGemstones);
                    else
                        fourthParamaterGemstones = EditorGUILayout.FloatField("RadiusX", fourthParamaterGemstones);

                    // Beschränkt die Usereingaben für die Höhe der Spitze der Kristalle => 0.01 - 1000
                    if (sixthParamaterGemstones < 0.009 || sixthParamaterGemstones > 1000)
                    {
                        sixthParamaterGemstones = 0.5f;
                    }
                    // Sechster Gemstones-Parameter für die Höhe der Spitze oder der Krone
                    if (secondParameterGemstones == "Crystal")
                        sixthParamaterGemstones = EditorGUILayout.FloatField("Peak Height", sixthParamaterGemstones);
                    if (secondParameterGemstones == "Gem")
                        sixthParamaterGemstones = EditorGUILayout.FloatField("RadiusY", sixthParamaterGemstones);
                    if (secondParameterGemstones == "Diamond")
                        sixthParamaterGemstones = EditorGUILayout.FloatField("Crown Height", sixthParamaterGemstones);

                    // Beschränkt die Usereingaben für die Höhe => 0.01 - 1000
                    if (fifthParamaterGemstones < 0.009 || fifthParamaterGemstones > 1000)
                    {
                        fifthParamaterGemstones = 1.0f;
                    }
                    // Fünfter Gemstones-Parameter für die Höhe der Objekte
                    if (secondParameterGemstones != "Gem")
                        fifthParamaterGemstones = EditorGUILayout.FloatField("Body Height", fifthParamaterGemstones);
                    else
                        fifthParamaterGemstones = EditorGUILayout.FloatField("width", fifthParamaterGemstones);

                    // Siebter Gemstones-Paramter => Die Checkbox, um ein Objekt zu smoothen         
                    seventhParameterGemstones = EditorGUILayout.Toggle("Smooth", seventhParameterGemstones);

                    // Neunter Gemstones-Paramter => Die Checkbox, um dem Objekt einen Collider zu verpassen         
                    ninthParameterGemstones = EditorGUILayout.Toggle("Collider", ninthParameterGemstones);

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
                        if (gem)
                        {
                            gem = GemService.Instance.CreateGem(gem);
                            gem.GetComponent<MeshRenderer>().material = gemstoneMaterial;
                        }
                        if (diamond)
                        {
                            diamond = DiamondService.Instance.CreateDiamond(diamond);
                            diamond.GetComponent<MeshRenderer>().material = gemstoneMaterial;
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
            CheckIfGemSelected();
            CheckIfDiamondSelected();
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
                    seventhParameterGemstones = crystal.smoothFlag;
                    eightParamaterGemstones = crystal.lodCount;
                    ninthParameterGemstones = crystal.colliderFlag; 
                    if (crystal.GetComponent<MeshRenderer>().sharedMaterial != null)
                    {
                        gemstoneMaterial = crystal.GetComponent<MeshRenderer>().sharedMaterial;
                    }
                    this.Repaint();
                }
            }
            else
            {
                if (crystal != CrystalService.Instance.GetCrystalFromSelection())
                {
                    crystal = null;
                    this.Repaint();
                }
            }
        }

        private void CheckIfGemSelected()
        {
            if (gem == null)
            {
                gem = GemService.Instance.GetGemFromSelection();
                if (gem != null)
                {
                    firstParameterGemstones = gem.name;
                    secondParameterGemstones = "Gem";
                    thirdParamaterGemstones = gem.edges;
                    fourthParamaterGemstones = gem.radiusX;
                    sixthParamaterGemstones = gem.radiusY;
                    fifthParamaterGemstones = gem.width;
                    seventhParameterGemstones = gem.smoothFlag;
                    eightParamaterGemstones = gem.lodCount;
                    ninthParameterGemstones = gem.colliderFlag;
                    if (gem.GetComponent<MeshRenderer>().sharedMaterial != null)
                    {
                        gemstoneMaterial = gem.GetComponent<MeshRenderer>().sharedMaterial;
                    }
                    this.Repaint();
                }
            }
            else
            {
                if (gem != GemService.Instance.GetGemFromSelection())
                {
                    gem = null;
                    this.Repaint();
                }
            }
        }

        private void CheckIfDiamondSelected()
        {
            if (diamond == null)
            {
                diamond = DiamondService.Instance.GetDiamondFromSelection();
                if (diamond != null)
                {
                    firstParameterGemstones = diamond.name;
                    secondParameterGemstones = "Diamond";
                    thirdParamaterGemstones = diamond.edges;
                    fourthParamaterGemstones = diamond.radius;
                    fifthParamaterGemstones = diamond.pavillonHeight;
                    sixthParamaterGemstones = diamond.crownHeight;
                    seventhParameterGemstones = diamond.smoothFlag;
                    eightParamaterGemstones = diamond.lodCount;
                    ninthParameterGemstones = diamond.colliderFlag;
                    if (diamond.GetComponent<MeshRenderer>().sharedMaterial != null)
                    {
                        gemstoneMaterial = diamond.GetComponent<MeshRenderer>().sharedMaterial;
                    }
                    this.Repaint();
                }
            }
            else
            {
                if (diamond != DiamondService.Instance.GetDiamondFromSelection())
                {
                    diamond = null;
                    this.Repaint();
                }
            }
        }

        private void UpdateGemStones()
        {
            if (crystal != null)
            {
                crystal.edges = thirdParamaterGemstones;
                crystal.radius = fourthParamaterGemstones;
                crystal.height = fifthParamaterGemstones;
                crystal.heightPeak = sixthParamaterGemstones;
                crystal.smoothFlag = seventhParameterGemstones;
                crystal.lodCount = eightParamaterGemstones;
                crystal.colliderFlag = ninthParameterGemstones;
            }
            if (gem != null)
            {
                gem.edges = thirdParamaterGemstones;
                gem.radiusX = fourthParamaterGemstones;
                gem.radiusY = sixthParamaterGemstones;
                gem.width = fifthParamaterGemstones;
                gem.smoothFlag = seventhParameterGemstones;
                gem.lodCount = eightParamaterGemstones;
                gem.colliderFlag = ninthParameterGemstones;
            }
            if (diamond != null)
            {
                diamond.edges = thirdParamaterGemstones;
                diamond.radius = fourthParamaterGemstones;
                diamond.pavillonHeight = fifthParamaterGemstones;
                diamond.crownHeight = sixthParamaterGemstones;
                diamond.smoothFlag = seventhParameterGemstones;
                diamond.lodCount = eightParamaterGemstones;
                diamond.colliderFlag = ninthParameterGemstones;
            }
        }

        private void OnDestroy()
        {
            if (crystal && crystal.mesh == null)
            {
                DestroyImmediate(crystal.gameObject);
            }

            if (gem && gem.mesh == null)
            {
                DestroyImmediate(gem.gameObject);
            }

            if (diamond && diamond.mesh == null)
            {
                DestroyImmediate(diamond.gameObject);
            }
        }
    }
}