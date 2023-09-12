using Cinemachine;
using UnityEditor;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.InputSystem;

public class NDSEditor : EditorWindow
{
    [MenuItem("Window/NDS Hub/Xtreme FPS")]


    public static void ShowWindow()
    {
        // Create a new Editor Window instance and show it
        NDSEditor window = GetWindow<NDSEditor>("XtremeFPS");
        window.Show();
    }

    private string cinemachineStatusText;
    private string inputSystemStatusText;
    private void OnGUI()
    {
        #region Intro
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
        EditorGUILayout.Space();
        GUILayout.Space(20); // Add some vertical spacing
        GUILayout.Label("Automatic Checks.", new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleCenter, fontStyle = FontStyle.Bold, fontSize = 13 }, GUILayout.ExpandWidth(true));
        GUI.color = Color.white;
        #endregion
        #region Cinemachine
        GUI.color = Color.yellow;
        GUILayout.Label("Cinemachine:-", new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleLeft, fontStyle = FontStyle.Bold, fontSize = 13 }, GUILayout.ExpandWidth(true));
        GUI.color = Color.white;
        if (IsCinemachineInstalled())
        {
            cinemachineStatusText = "Cinemachine is installed!";
            GUI.color = Color.white;
            GUILayout.Label(cinemachineStatusText, new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleLeft, fontStyle = FontStyle.Bold, fontSize = 13 }, GUILayout.ExpandWidth(true));
        }
        else
        {
            cinemachineStatusText = "Cinemachine is not installed. Click the button to install.";
            // Create a button that stays at the bottom
            GUI.color = Color.red;
            GUILayout.Label(cinemachineStatusText, new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleLeft, fontStyle = FontStyle.Bold, fontSize = 13 }, GUILayout.ExpandWidth(true));
            GUI.color = Color.white;
            Rect buttonRect = GUILayoutUtility.GetRect(200, 50); // Define button dimensions
            buttonRect.x = (EditorGUIUtility.currentViewWidth - buttonRect.width) * 0.5f; // Center the button horizontally
            if (GUI.Button(buttonRect, "Install Cinemachine"))
            {
                // Code to execute when the button is clicked
                InstallCinemachine();
            }
        }
        EditorGUILayout.Space();
        #endregion
        #region Input System
        GUI.color = Color.yellow;
        GUILayout.Label("Input System:-", new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleLeft, fontStyle = FontStyle.Bold, fontSize = 13 }, GUILayout.ExpandWidth(true));
        GUI.color = Color.white;
        if (IsInputSystemInstalled())
        {
            inputSystemStatusText = "InputSystem is installed!";
            GUI.color = Color.white;
            GUILayout.Label(inputSystemStatusText, new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleLeft, fontStyle = FontStyle.Bold, fontSize = 13 }, GUILayout.ExpandWidth(true));
        }
        else
        {
            inputSystemStatusText = "InputSystem is not installed. Click the button to install.";
            // Create a button that stays at the bottom
            GUI.color = Color.red;
            GUILayout.Label(inputSystemStatusText, new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleLeft, fontStyle = FontStyle.Bold, fontSize = 13 }, GUILayout.ExpandWidth(true));
            GUI.color = Color.white;
            Rect buttonRect = GUILayoutUtility.GetRect(200, 50); // Define button dimensions
            buttonRect.x = (EditorGUIUtility.currentViewWidth - buttonRect.width) * 0.5f; // Center the button horizontally
            if (GUI.Button(buttonRect, "Install InputSystem"))
            {
                // Code to execute when the button is clicked
                InstallCinemachine();
            }
        }
        EditorGUILayout.Space();
        #endregion
    }

    bool IsCinemachineInstalled()
    {
        // Check if Cinemachine exists in the project
        return typeof(CinemachineVirtualCamera).Assembly != null;
    }
    private void InstallCinemachine()
    {
        Client.Add("com.unity.cinemachine");
        cinemachineStatusText = "Installing Cinemachine...";
    }

    bool IsInputSystemInstalled()
    {
        // Check if Input System exists in the project
        return typeof(InputSystem).Assembly != null;
    }

    public void InstallInputSystem()
    {
        Client.Add("com.unity.inputsystem");
        inputSystemStatusText = "Installing Unity Input System...";
    }

}

