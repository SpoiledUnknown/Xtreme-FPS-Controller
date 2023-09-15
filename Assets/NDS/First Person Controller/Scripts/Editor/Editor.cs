using UnityEditor;
using UnityEditor.PackageManager;
using UnityEngine;

public class NDSEditor : EditorWindow
{
    #region Setup
    [MenuItem("Window/NDS Hub/Xtreme FPS")]
    public static void ShowWindow()
    {
        // Create a new Editor Window instance and show it
        NDSEditor window = GetWindow<NDSEditor>("XtremeFPS");
        window.Show();
    }

    
    #endregion
    #region Varibales

    //bools
    private bool aboutButton = true;
    private bool systemButton = false;
    private bool layerButton = false;
    private bool completeButton = false;

    
    #endregion
    private void OnGUI()
    {
        EditorGUILayout.BeginHorizontal();
        
        // Left section (buttons)
        EditorGUILayout.BeginVertical(GUILayout.Width(200));
        if (GUILayout.Button("About.", GUILayout.Width(200), GUILayout.Height(100)))
        {
            aboutButton = true;
            systemButton = false;
            layerButton = false;
            completeButton = false;
        }
        if(GUILayout.Button("System.", GUILayout.Width(200), GUILayout.Height(100)))
        {
            systemButton = true;
            aboutButton = false;
            layerButton = false;
            completeButton = false;
        }
        if(GUILayout.Button("Layer & Tags.", GUILayout.Width(200), GUILayout.Height(100)))
        {
            systemButton = false;
            aboutButton = false;
            layerButton = true;
            completeButton = false;
        }
        if(GUILayout.Button("Create Complete.", GUILayout.Width(200), GUILayout.Height(100)))
        {
            systemButton = false;
            aboutButton = false;
            layerButton = false;
            completeButton = true;
        }
        
        

        // Add more buttons as needed
        EditorGUILayout.EndVertical();

        GUI.color = Color.black;

        // Right section (content)
        EditorGUILayout.BeginVertical();
        if (aboutButton)
        {
            #region Intro
            EditorGUILayout.LabelField("About Xtreme FPS");
            EditorGUILayout.Space();
            GUI.color = Color.black;
            GUILayout.Label("Xtreme FPS Controller", new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleCenter, fontStyle = FontStyle.Bold, fontSize = 16 });
            GUI.color = Color.green;
            GUILayout.Label("NDS || By SpoiledUnknown", new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleCenter, fontStyle = FontStyle.Normal, fontSize = 12 });
            GUI.color = Color.red;
            GUILayout.Label("version 1.0.0", new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleCenter, fontStyle = FontStyle.Italic, fontSize = 12 });
            GUI.color = Color.black;
            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
            GUI.color = Color.green;
            GUILayout.Label("Xtreme FPS Controller Setup", new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleCenter, fontStyle = FontStyle.Bold, fontSize = 16 });
            GUI.color = Color.black;
            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
            GUILayout.Space(20); // Add some vertical spacing
            #endregion
        }
        if (systemButton)
        {
            EditorGUILayout.LabelField("System Intalls");
            #region Cinemachine
            GUI.color = Color.yellow;
            GUILayout.Label("Cinemachine:-", new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleLeft, fontStyle = FontStyle.Bold, fontSize = 13 }, GUILayout.ExpandWidth(true));
            GUI.color = Color.white;
            // Create a button that stays at the bottom
            GUI.color = Color.red;
            GUI.color = Color.white;
            Rect buttonRect = GUILayoutUtility.GetRect(200, 50); // Define button dimensions
            if (GUI.Button(buttonRect, "Install Cinemachine."))
            {
                // Code to execute when the button is clicked
                InstallCinemachine();
            }
            EditorGUILayout.Space();
            #endregion
            #region Input System
            GUI.color = Color.yellow;
            GUILayout.Label("Input System:-", new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleLeft, fontStyle = FontStyle.Bold, fontSize = 13 }, GUILayout.ExpandWidth(true));
            GUI.color = Color.white;
            // Create a button that stays at the bottom
            GUI.color = Color.red;
            GUI.color = Color.white;
            Rect inputButtonRect = GUILayoutUtility.GetRect(200, 50); // Define button dimensions
            if (GUI.Button(inputButtonRect, "Install InputSystem."))
            {
                // Code to execute when the button is clicked
                InstallInputSystem();
            }
            EditorGUILayout.Space();
            #endregion
        }
        if (layerButton)
        {
            EditorGUILayout.LabelField("Layer And Tag Setup");
            #region Create Layers
            GUI.color = Color.yellow;
            GUILayout.Label("Layers:-", new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleLeft, fontStyle = FontStyle.Bold, fontSize = 13 }, GUILayout.ExpandWidth(true));
            GUI.color = Color.white;
            // Create a button that stays at the bottom
            GUI.color = Color.red;
            GUI.color = Color.white;
            Rect buttonRect = GUILayoutUtility.GetRect(200, 50); // Define button dimensions
            if (GUI.Button(buttonRect, "Create Layers."))
            {
                // Code to execute when the button is clicked
                
            }
            EditorGUILayout.Space();
            #endregion
            #region Create Tags
            GUI.color = Color.yellow;
            GUILayout.Label("Tags:-", new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleLeft, fontStyle = FontStyle.Bold, fontSize = 13 }, GUILayout.ExpandWidth(true));
            GUI.color = Color.white;
            // Create a button that stays at the bottom
            GUI.color = Color.red;
            GUI.color = Color.white;
            Rect inputButtonRect = GUILayoutUtility.GetRect(200, 50); // Define button dimensions
            if (GUI.Button(inputButtonRect, "Create Tags."))
            {
                // Code to execute when the button is clicked
                
            }
            EditorGUILayout.Space();
            #endregion
        }
        EditorGUILayout.EndVertical();

        EditorGUILayout.EndHorizontal();
        /*
        

        #region Create Controller
        GUI.color = Color.black;
        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
        GUILayout.Label("Tag Creation.", new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleCenter, fontStyle = FontStyle.Bold, fontSize = 13 }, GUILayout.ExpandWidth(true));
        GUI.color = Color.white;
        GUILayout.Space(20); // Add some vertical spacing
        Rect ccButtonRect = GUILayoutUtility.GetRect(200, 50); // Define button dimensions
        ccButtonRect.x = (EditorGUIUtility.currentViewWidth - ccButtonRect.width) * 0.5f; // Center the button horizontally
        if (GUI.Button(ccButtonRect, "Create Tag"))
        {
            // Code to execute when the button is clicked
            CreateTag();
        }
        EditorGUILayout.Space();
        #endregion*/
    }

    #region CineMachine
    private void InstallCinemachine()
    {
        Client.Add("com.unity.cinemachine");
    }
    #endregion
    #region Input System
    public void InstallInputSystem()
    {
        Client.Add("com.unity.inputsystem");
    }
    #endregion

    // Helper method to check if an assembly is loaded
    private static bool IsAssemblyLoaded(string assemblyName)
    {
        System.AppDomain currentDomain = System.AppDomain.CurrentDomain;
        System.Reflection.Assembly[] assemblies = currentDomain.GetAssemblies();

        foreach (System.Reflection.Assembly assembly in assemblies)
        {
            if (assembly.FullName.Contains(assemblyName))
            {
                return true;
            }
        }

        return false;
    }
}

