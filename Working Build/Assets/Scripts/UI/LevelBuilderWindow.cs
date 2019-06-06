using UnityEngine;
using UnityEditor;

// Just playing around, Dont mind me

public class LevelBuilderWindow : EditorWindow
{
    public LevelBuilder levelBuilder;

    [MenuItem("Window/LevelBuilder")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(LevelBuilderWindow));
    }

    private void OnGUI()
    {
        if (GUILayout.Button("Generate Level - WIP"))
        {            
            if(levelBuilder == null) levelBuilder = GameObject.FindGameObjectWithTag("LevelBuilder").GetComponent<LevelBuilder>();

            //Currently broken and only placing start room. This is likely due to the generator taking more than tick to complete

            levelBuilder.ClearPlayer();
            levelBuilder.StartCoroutine(levelBuilder.GenerateLevel());
        }

        if (GUILayout.Button("Clear Generate"))
        {
            if (levelBuilder == null) levelBuilder = GameObject.FindGameObjectWithTag("LevelBuilder").GetComponent<LevelBuilder>();
            
            levelBuilder.ResetGenerator(false);
            levelBuilder.ClearPlayer();
        }
    }
}
