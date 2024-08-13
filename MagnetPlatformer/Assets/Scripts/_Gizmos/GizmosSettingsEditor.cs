using UnityEditor;
using UnityEngine;

public class GizmosSettingsEditorWindow : EditorWindow
{
    [MenuItem("Window/Gizmos Settings")]
    public static void ShowWindow()
    {
        GetWindow<GizmosSettingsEditorWindow>("Gizmos Settings");
    }

    void OnGUI()
    {
        GUILayout.Label("Gizmos Settings", EditorStyles.boldLabel);

        bool gizmosEnabled = GizmosSettings.GizmosEnabled;

        gizmosEnabled = EditorGUILayout.Toggle("Enable Gizmos", gizmosEnabled);

        if (GUI.changed)
        {
            GizmosSettings.GizmosEnabled = gizmosEnabled;
            Debug.Log(GizmosSettings.GizmosEnabled);
        }
    }
}