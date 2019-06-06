using UnityEngine;
using UnityEditor;

// Just playing around, Dont mind me

public class LevelBuilderWindow : EditorWindow
{
    [MenuItem("Window/LevelBuilder")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(LevelBuilderWindow));
    }

    public GameObject levelBuilder;

    private void OnGUI()
    {
        if (GUILayout.Button("Generate Level"))
        {

        }
        
    }
}
