/*Copyright © Non-Dynamic Studios*/
/*2023*/
/*Note: This is an important editor script*/

using NDS.UniversalWeaponSystem.ShellEjection;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ShellEjection)), CanEditMultipleObjects]
public class ShellEjectionEditor : Editor
{
    ShellEjection shellEjection;
    SerializedObject serShellEjection_UI;

    private void OnEnable()
    {
        shellEjection = (ShellEjection)target;
        serShellEjection_UI = new SerializedObject(shellEjection);
    }

    public override void OnInspectorGUI()
    {
        serShellEjection_UI.Update();
        #region Intro
        EditorGUILayout.Space();
        GUI.color = Color.black;
        GUILayout.Label("Xtreme FPS Controller", new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleCenter, fontStyle = FontStyle.Bold, fontSize = 16 });
        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
        GUI.color = Color.green;
        GUILayout.Label("Shell Ejection Script", new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleCenter, fontStyle = FontStyle.Bold, fontSize = 16 });
        EditorGUILayout.Space();
        GUI.color = Color.black;
        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

        #endregion
        #region Settings

        //Mobile Controls settings
        GUILayout.Label("Settings", new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleCenter, fontStyle = FontStyle.Bold, fontSize = 13 }, GUILayout.ExpandWidth(true));
        EditorGUILayout.Space();

        GUI.color = Color.white;
        shellEjection.lifeTime = EditorGUILayout.Slider(new GUIContent("Life Time", "The time after which the shell(gameObject) will get destroyed."), shellEjection.lifeTime, 2f, 8f);
        shellEjection.minForce = EditorGUILayout.Slider(new GUIContent("Min Force", "The maximum amount of force with which the shell should be thrown out."), shellEjection.minForce, 50f, 200f);
        shellEjection.maxForce = EditorGUILayout.Slider(new GUIContent("Max Force", "The maximum amount of force with which the shell should be thrown out."), shellEjection.maxForce, shellEjection.minForce, 400f);
        EditorGUILayout.Space();
        GUI.color = Color.black;
        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

        #endregion
        #region Update Changes
        //Sets any changes from the prefab
        if (GUI.changed)
        {
            EditorUtility.SetDirty(shellEjection);
            Undo.RecordObject(shellEjection, "Shell Ejector Change");
            serShellEjection_UI.ApplyModifiedProperties();
        }
        #endregion
    }
}
