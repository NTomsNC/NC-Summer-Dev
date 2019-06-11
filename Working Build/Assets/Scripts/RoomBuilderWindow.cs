using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class RoomBuilderWindow : EditorWindow
{
    GameObject root_MainRoomGO;

    GameObject roomRoot;
    GameObject doorRoot;

    List<GameObject> activeDoors = new List<GameObject>();

    bool bShowOptions = false;
    bool bActiveRoom = false;

    Object roomMesh = null;
    Object doorMesh = null;

    string roomName = "NewRoom";

    /// <summary>
    /// Calls the Room Builder Window
    /// </summary>
    [MenuItem("Window/RoomBuilder")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(RoomBuilderWindow));
    }

    /// <summary>
    /// Controls the GUI for the Room Builder Script
    /// </summary>
    private void OnGUI()
    {
        GenerateNewRoom();

        if (bShowOptions)
        {
            Reset();

            GenerateNewMesh();

            GenerateDoors();

            GeneratePreFabSave();
        }
        else
        {
            GUILayout.Label("A room has not been instantiated, Please click generate to begin creating a room");
        }
    }

    #region OnGUI
    /// <summary>
    /// Initially Generated a New Room to Edit
    /// </summary>
    private void GenerateNewRoom()
    {
        if (GUILayout.Button("Generate New Room"))
        {
            GameObject newRoom = new GameObject();
            newRoom.tag = "Room";
            newRoom.name = "NewRoom";

            root_MainRoomGO = newRoom;

            Room room = root_MainRoomGO.GetComponent<Room>();

            if (room == null)
            {
                root_MainRoomGO.AddComponent<Room>();
            }

            bShowOptions = true;
            bActiveRoom = true;
        }
    }

    /// <summary>
    /// Loads the Main Room Mesh into the Room
    /// </summary>
    private void GenerateNewMesh()
    {
        GUILayout.Label("Generate Room");

        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Room Mesh");
        roomMesh = EditorGUILayout.ObjectField(roomMesh, typeof(GameObject), true);
        EditorGUILayout.EndHorizontal();

        if (roomMesh != null)
        {
            roomMesh.name = "RoomMesh";

            if (GUILayout.Button("Generate"))
            {
                GameObject gameObject = (GameObject)roomMesh;
                roomRoot = GameObject.Instantiate(gameObject, Vector3.zero, gameObject.transform.rotation);
                roomRoot.name = "Room";
                roomRoot.transform.parent = root_MainRoomGO.transform;

                root_MainRoomGO.GetComponent<Room>().meshCollider = roomRoot.GetComponent<MeshCollider>();

            }

            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("Room");

            if (GUILayout.Button("Select"))
            {
                Selection.activeObject = roomRoot;
            }
            EditorGUILayout.EndHorizontal();
        }
    }

    /// <summary>
    /// Controlls the Doors in the Active Room
    /// </summary>
    private void GenerateDoors()
    {
        GUILayout.Space(15);
        GUILayout.Label("Generate Doors");
        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Door Mesh");
        doorMesh = EditorGUILayout.ObjectField(doorMesh, typeof(GameObject), true);
        EditorGUILayout.EndHorizontal();

        if (GUILayout.Button("Generate"))
        {
            if (doorRoot == null)
            {
                GameObject Doors = new GameObject();
                Doors.name = "Doors";

                doorRoot = Doors;

                Doors.transform.parent = root_MainRoomGO.transform;
            }

            if (doorMesh != null)
            {
                GameObject d = (GameObject)doorMesh;

                Doorway doorway = d.GetComponent<Doorway>();

                if (doorway == null)
                    d.AddComponent<Doorway>();

                activeDoors.Add(GameObject.Instantiate(d, Vector3.zero, d.transform.rotation));

                root_MainRoomGO.GetComponent<Room>().doorways = new Doorway[activeDoors.Count];

                for (int i = 0; i < activeDoors.Count; i++)
                {
                    root_MainRoomGO.GetComponent<Room>().doorways[i] = activeDoors[i].GetComponent<Doorway>();
                }
                
            }
            else
            {
                activeDoors.Add(new GameObject());
            }
            activeDoors[activeDoors.Count - 1].name = "Door " + activeDoors.Count;
            activeDoors[activeDoors.Count - 1].transform.parent = doorRoot.transform;
        }

        for (int i = 0; i < activeDoors.Count; i++)
        {
            EditorGUILayout.BeginHorizontal();
            GUILayout.Label(activeDoors[i].name);

            if (GUILayout.Button("<<", GUILayout.Width(30)))
            {
                Nudge(activeDoors[i]);
            }

            if (GUILayout.Button("Rot", GUILayout.Width(30)))
            {
                activeDoors[i].transform.Rotate(new Vector3(0, 90, 0));
            }

            if (GUILayout.Button("Select"))
            {
                Selection.activeObject = activeDoors[i];
            }

            EditorGUILayout.EndHorizontal();
        }
    }

    /// <summary>
    /// Saves the Active Room
    /// </summary>
    private void GeneratePreFabSave()
    {
        GUILayout.Space(5);
        GUILayout.Label("Save");
        roomName = GUILayout.TextArea(roomName);
        if (GUILayout.Button("Save"))
        {
            PrefabUtility.SaveAsPrefabAsset(roomRoot, "Assets/" + "Prefabs/" + roomName + ".prefab");
        }
    }
    #endregion

    /// <summary>
    /// Looks to see if Room was Destroyed
    /// </summary>
    private void Update()
    {
        if (root_MainRoomGO == null && bActiveRoom)
        {
            bShowOptions = false;
            bActiveRoom = false;

            Repaint();

            activeDoors.Clear();
        }
    }

    /// <summary>
    /// Resets the Active Room and GUI
    /// </summary>
    private void Reset()
    {
        if (GUILayout.Button("Reset"))
        {
            DestroyImmediate(root_MainRoomGO);
            activeDoors.Clear();
            bShowOptions = false;
        }
    }

    /// <summary>
    /// Move a Gameobject Forward 0.0001f
    /// </summary>
    /// <param name="go"></param>
    private void Nudge(GameObject go)
    {
        go.transform.position += (0.0001f * go.transform.forward);
    }
}
