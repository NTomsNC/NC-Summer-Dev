using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class RoomBuilderWindow : EditorWindow
{
    GameObject roomRoot;
    GameObject DoorsRoot;
    GameObject mainRoomMesh;

    List<GameObject> activeDoors = new List<GameObject>();

    Object roomMesh = null;
    Object doorMesh = null;

    string roomName = "NewRoom";

    bool activeRoom = false;
    bool showOptions = false;
    bool needsReDraw = false;

    [MenuItem("Window/RoomBuilder")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(RoomBuilderWindow));
    }

    private void OnGUI()
    {
        GenerateNewRoom();

        GenerateOptions();

        if (!showOptions)
        {
            GUILayout.Label("A room has not been instantiated, Please click generate to begin creating a room");
        }
    }

    private void Update()
    {
        if (roomRoot == null && activeRoom)
        {
            showOptions = false;
            activeRoom = false;

            Repaint();

            activeDoors.Clear();
        }
    }

    private void Reset()
    {
        if (GUILayout.Button("Reset"))
        {
            DestroyImmediate(roomRoot);
            activeDoors.Clear();
            showOptions = false;
        }
    }

    private void GenerateNewRoom()
    {
        if (GUILayout.Button("Generate New Room"))
        {
            GameObject newRoom = new GameObject();
            newRoom.tag = "Room";
            newRoom.name = "NewRoom";
            roomRoot = newRoom;

            showOptions = true;
            activeRoom = true;
        }
    }

    private void GenerateOptions()
    {
        if (showOptions)
        {
            Reset();

            #region Mesh
            GUILayout.Space(5);

            GUILayout.Label("Generate Room");
            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("Room Mesh");
            roomMesh = EditorGUILayout.ObjectField(roomMesh, typeof(GameObject), true);
            EditorGUILayout.EndHorizontal();
            #endregion

            if (roomMesh != null)
            {
                roomMesh.name = "RoomMesh";

                if (GUILayout.Button("Generate"))
                {
                    GameObject r = (GameObject)roomMesh;
                    mainRoomMesh = GameObject.Instantiate(r, Vector3.zero, r.transform.rotation);
                    mainRoomMesh.name = "Room";
                    mainRoomMesh.transform.parent = roomRoot.transform;
                }

                EditorGUILayout.BeginHorizontal();
                GUILayout.Label("Room");

                if (GUILayout.Button("Select"))
                {
                    Selection.activeObject = mainRoomMesh;
                }
                EditorGUILayout.EndHorizontal();
            }

            #region Doors
            GUILayout.Space(15);
            GUILayout.Label("Generate Doors");
            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("Door Mesh");
            doorMesh = EditorGUILayout.ObjectField(doorMesh, typeof(GameObject), true);
            EditorGUILayout.EndHorizontal();

            if (GUILayout.Button("Generate"))
            {
                if (DoorsRoot == null)
                {
                    GameObject Doors = new GameObject();
                    Doors.name = "Doors";

                    DoorsRoot = Doors;

                    Doors.transform.parent = roomRoot.transform;
                }

                if (doorMesh != null)
                {
                    GameObject d = (GameObject)doorMesh;
                    activeDoors.Add(GameObject.Instantiate(d, Vector3.zero, d.transform.rotation));
                }
                else
                {
                    activeDoors.Add(new GameObject());
                }
                activeDoors[activeDoors.Count - 1].name = "Door " + activeDoors.Count;
                activeDoors[activeDoors.Count - 1].transform.parent = DoorsRoot.transform;
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
            #endregion

            #region Save
            GUILayout.Space(5);
            GUILayout.Label("Save");
            roomName = GUILayout.TextArea(roomName);
            if (GUILayout.Button("Save"))
            {
                PrefabUtility.SaveAsPrefabAsset(roomRoot, "Assets/" + "Prefabs/" + roomName + ".prefab");
            }
            #endregion
        }
    }

    private void Nudge(GameObject go)
    {
        go.transform.position += (0.0001f * go.transform.forward);
    }
}
