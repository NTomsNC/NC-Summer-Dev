using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class LevelBuilder : MonoBehaviour
{
<<<<<<< HEAD
<<<<<<< HEAD
    [Header("Room Prefabs -----------------------------------------------")]
    [Space(20)]
=======
=======
>>>>>>> parent of 76823b3e... Save Class
    [Space(5)]
    public Room startRoomPrefab, endRoomPrefab;
    public GameObject Player;
>>>>>>> parent of 76823b3e... Save Class

    public Room startRoomPrefab;
    public Room endRoomPrefab;
    public LayerMask roomLayerMask;

    //public List<Room> roomPrefabs = new List<Room>();
    public List<sRoom> roomPrefabs = new List<sRoom>();

    [Header("Door Prefab ------------------------------------------------")]
    public GameObject frame;

    [Header("Player Prefab ----------------------------------------------")]
    public GameObject Player;

    [Header("Generation Prefab -----------------------------------------")]
    [Tooltip("For testing purposes. Disable when building game")]
    public Vector2 iterationRange = new Vector2(5, 15);
    public bool useIterationRange = false;

    [Header("Additional Features")]
    public bool trimExtraHallways = false;

    [Header("Instance data ----------------------------------------------")]
    public int level;
    public int seed;

    [Header("SaveClass --------------------------------------------------")]
    public SaveClass saveGame;
    public int saveNum = 1;

    [Header("Debug ------------------------------------------------------")]
    public bool debugMode = false;

    int level;
    int seed;

    int level;
    int seed;

    StartRoom startRoom;
    EndRoom endRoom;

    List<Room> rooms = new List<Room>();
    List<Doorway> availableDoorways = new List<Doorway>();

<<<<<<< HEAD
<<<<<<< HEAD
    [System.Serializable]
    public class sRoom
=======

    //Used to get the level for other scripts
    public int Level
    {
        get { return level; }
    }

    private void Start()
>>>>>>> parent of 76823b3e... Save Class
    {
        public Room room;
        public int chance;

<<<<<<< HEAD
        sRoom(Room inDoor, int inChance)
        {
            chance = inChance;
            room = inDoor;
        }
    }
=======
>>>>>>> parent of 76823b3e... Save Class

    //Used to get the level for other scripts
    public int Level
    {
        get { return level; }
    }

    private void Start()
    {
        if (roomLayerMask == 0) roomLayerMask = LayerMask.GetMask("Rooms");

        //Loading level status
        level = PlayerPrefs.GetInt("Level", 1);
        seed = PlayerPrefs.GetInt("Seed", System.DateTime.Now.Second);
        Random.InitState(seed);

<<<<<<< HEAD
=======
=======
>>>>>>> parent of 76823b3e... Save Class
        //Loading level status
        level = PlayerPrefs.GetInt("Level", 1);
        seed = PlayerPrefs.GetInt("Seed", System.DateTime.Now.Second);
        Random.InitState(seed);

<<<<<<< HEAD
>>>>>>> parent of 76823b3e... Save Class
=======
>>>>>>> parent of 76823b3e... Save Class
        //Saving level status
        PlayerPrefs.SetInt("Level", level);
        PlayerPrefs.SetInt("Seed", seed);
        PlayerPrefs.Save();

        StartCoroutine("GenerateLevel");
    }

    //Delete when UI is set up
    #region TempUI
    private void OnGUI()
    {
        GUI.Label(new Rect(10, 10, 250, 30), "Current Level: " + level.ToString());
    }
    #endregion

    //Enumerator that generate the level. Primary functionality
    IEnumerator GenerateLevel()
    {
        WaitForFixedUpdate interval = new WaitForFixedUpdate();

        // Place start room
        PlaceStartRoom();

        yield return interval;

        //Random iterations
        int iterations = 0;
<<<<<<< HEAD
<<<<<<< HEAD
        if (!useIterationRange) iterations = Random.Range(level * 2 + 5, level * 3);
=======
        if (!useIterationRange)  iterations = Random.Range(level * 2 + 5 , level * 3);
>>>>>>> parent of 76823b3e... Save Class
=======
        if (!useIterationRange)  iterations = Random.Range(level * 2 + 5 , level * 3);
>>>>>>> parent of 76823b3e... Save Class
        else iterations = Random.Range((int)iterationRange.x, (int)iterationRange.y);

        for (int i = 0; i < iterations; i++)
        {
            // Place random room from list
            yield return StartCoroutine(PlaceRoom());
        }

        if (trimExtraHallways)
        {
            RemoveDoors();
            RemoveHallways();
        }

        //Place end room
        yield return StartCoroutine(PlaceEndRoom());

        //Level generation finished
        if (debugMode)
            Debug.Log("Generation Finished");

        PlacePlayer();
    }

    private void RemoveHallways()
    {
        GameObject[] hallways = GameObject.FindGameObjectsWithTag("Hallway");
        for (int i = 0; i < hallways.Length; i++)
        {
            Doorway[] doorways = hallways[i].GetComponent<Room>().doorways;
            for (int j = 0; j < doorways.Length; j++)
            {
                List<GameObject> deactivatedDoors = new List<GameObject>();
                if (doorways[j].gameObject.active)
                {
                    hallways[i].gameObject.transform.GetChild(0).gameObject.GetComponent<Renderer>().material.color = Color.red;
                    for (int d = 0; d < doorways.Length; d++)
                    {
                        if (!doorways[d].gameObject.active)
                        {
                            deactivatedDoors.Add(doorways[d].gameObject);
                        }
                    }

                    hallways[i].gameObject.SetActive(false);
                }

                if (deactivatedDoors.Count != 0)
                {
                    for (int d = 0; d < deactivatedDoors.Count; d++)
                    {
                        if (debugMode)
                            Debug.Log("Added Door");

                        GameObject newDoor = GameObject.Instantiate(deactivatedDoors[d], deactivatedDoors[d].transform.position, deactivatedDoors[d].transform.rotation);
                        newDoor.SetActive(true);
                    }
                }
            }
        }
    }

    void RemoveDoors()
    {
        GameObject[] Doors = GameObject.FindGameObjectsWithTag("Door");
        for (int i = 0; i < Doors.Length; i++)
        {
            for (int j = i + 1; j < Doors.Length; j++)
            {
                if (Vector3.Distance(Doors[i].transform.position, Doors[j].transform.position) < 0.5f)
                {
                    if (debugMode)
                        Debug.Log("Door found");

                    Doors[i].SetActive(false);
                    Doors[j].SetActive(false);
                }
            }
        }
    }

    //Places starting room
    void PlaceStartRoom()
    {
        if (debugMode)
            Debug.Log("Place start room");

        //Instantiate
        startRoom = Instantiate(startRoomPrefab) as StartRoom;
        startRoom.transform.parent = transform;

        //get doorways from current room and add to list
        AddDoorwaysToList(startRoom, ref availableDoorways);

        //position room
        startRoom.transform.position = Vector3.zero;
        startRoom.transform.rotation = Quaternion.identity;
        startRoom.RoomBounds.Expand(-0.1f);

        rooms.Add(startRoom);
    }

    //Places the player in the starting room
    void PlacePlayer()
    {
        GameObject controller = Instantiate(Player);

        Vector3 startPos = startRoom.transform.position;
        startPos.y += 1;
        controller.transform.position = startPos;
    }

    //Places the ending room for the level
    IEnumerator PlaceEndRoom()
    {
        WaitForFixedUpdate interval = new WaitForFixedUpdate();

        if (debugMode)
            Debug.Log("Place end room");

        //Instantiate
        endRoom = Instantiate(endRoomPrefab) as EndRoom;
        endRoom.transform.parent = transform;

        //Create doorway list to loop over
        List<Doorway> allAvailableDoors = new List<Doorway>(availableDoorways);
        List<Doorway> currentDoors = new List<Doorway>();

        AddDoorwaysToList(endRoom, ref currentDoors);

        //Get doorways and add to global list
        AddDoorwaysToList(endRoom, ref availableDoorways);

        bool roomPlaced = false;

        //Try available doors
        foreach (Doorway availableDoor in allAvailableDoors)
        {
            //try doors in current room
            foreach (Doorway currentDoor in currentDoors)
            {
                //position room
                Room eRoom = endRoom;
                PositionAtDoorway(ref eRoom, currentDoor, availableDoor);
                yield return interval;

                if (CheckRoomOverlap(endRoom))
                {
                    continue;
                }

                roomPlaced = true;

                //Add room to list
                rooms.Add(endRoom);

                //remove occupied doorways
                currentDoor.gameObject.SetActive(false);
                availableDoorways.Remove(currentDoor);

                availableDoor.gameObject.SetActive(false);
                availableDoorways.Remove(availableDoor);

                //exit loop if room is placed
                break;
            }

            //Exit loop if room has been placed
            if (roomPlaced)
            {
                break;
            }
        }

        //Room couldnt be placed. Restart generator and try again
        if (!roomPlaced)
        {
            Destroy(endRoom.gameObject);
            ResetGenerator();
        }
    }

    private int PickRoom()
    {
        int chance = 0;
        for (int i = 0; i < roomPrefabs.Count; i++)
            chance += roomPrefabs[i].chance;

        int r = Random.Range(0, chance);

        int total = 0;
        for (int j = 0; j < roomPrefabs.Count; j++)
        {
            total += roomPrefabs[j].chance;
            if (r < total)
            {
                r = j;
                break;
            }
            else
            {
            }
        }

        if (debugMode)
            Debug.Log(r);

        return r;
    }

    //Used to place a room and determine location and rotation
    IEnumerator PlaceRoom()
    {
        WaitForFixedUpdate interval = new WaitForFixedUpdate();

        if (debugMode)
            Debug.Log("Place room");

        //Logarithmetically choose a room
        //int a = Random.Range(0, roomPrefabs.Count);
        //int b = Random.Range(0, roomPrefabs.Count);
        //int r = a < b ? a : b;

        int r = PickRoom();

        //instantiate room
        Room currentRoom = Instantiate(roomPrefabs[r].room) as Room;
        currentRoom.transform.parent = transform;

        //Create doorway list to loop over
        List<Doorway> allAvailableDoors = new List<Doorway>(availableDoorways);
        List<Doorway> currentDoors = new List<Doorway>();

        AddDoorwaysToList(currentRoom, ref currentDoors);

        //Get doorways and add to global list
        AddDoorwaysToList(currentRoom, ref availableDoorways);

        bool roomPlaced = false;

        //Try available doors
        foreach (Doorway availableDoor in allAvailableDoors)
        {
            //try doors in current room
            foreach (Doorway currentDoor in currentDoors)
            {
                //position room
                PositionAtDoorway(ref currentRoom, currentDoor, availableDoor);
                yield return interval;      //slows down placement to allow proper overlap checking. If not here it will check before bounds have actually moved to new location

                if (CheckRoomOverlap(currentRoom))
                {
                    continue;
                }

                roomPlaced = true;

                //Add room to list
                rooms.Add(currentRoom);

                //remove occupied doorways
                if (!trimExtraHallways)
                {
                    currentDoor.gameObject.SetActive(false);
                    availableDoorways.Remove(currentDoor);

                    availableDoor.gameObject.SetActive(false);
                    availableDoorways.Remove(availableDoor);
                }

                // Adds a connector
                //GameObject doorObject = Instantiate(frame, currentDoor.transform.position, currentDoor.transform.rotation);
                //doorObject.transform.parent = transform;

                //exit loop if room is placed
                break;
            }

            //Exit loop if room has been placed
            if (roomPlaced)
            {
                break;
            }
        }

        //Room couldnt be placed. Restart generator and try again
        if (!roomPlaced)
        {
            Destroy(currentRoom.gameObject);
            ResetGenerator();
        }
    }

    //Resets the entire generator if room placement fails
    public void ResetGenerator()
    {
        if (debugMode)
            Debug.LogError("Reset generation");

        StopCoroutine("GenerateLevel");

        //Delete all rooms
        if (startRoom)
        {
            Destroy(startRoom.gameObject);
        }
        if (endRoom)
        {
            Destroy(endRoom.gameObject);
        }

        foreach (Room room in rooms)
        {
            Destroy(room.gameObject);
        }

        GameObject[] doors = GameObject.FindGameObjectsWithTag("Rim");
        foreach (GameObject go in doors)
        {
            Destroy(go);
        }

        //Clear Lists
        rooms.Clear();
        availableDoorways.Clear();

        //reset
        StartCoroutine("GenerateLevel");
    }

    //Adds each doorway to a list
    void AddDoorwaysToList(Room room, ref List<Doorway> list)
    {
        foreach (Doorway door in room.doorways)
        {
            int r = Random.Range(0, list.Count);
            list.Insert(r, door);
        }
    }

    //Positions room at the chosen doorway. Will attempt with a new doorway if overlapping a room
    void PositionAtDoorway(ref Room room, Doorway roomDoorway, Doorway targetDoorway)
    {
        //reset position and rotation
        room.transform.position = Vector3.zero;
        room.transform.rotation = Quaternion.identity;

        //Rotate room to match previous doorway
        Vector3 roomDoorEuler = roomDoorway.transform.eulerAngles;
        Vector3 targetDoorEuler = targetDoorway.transform.eulerAngles;
        float deltaAngle = Mathf.DeltaAngle(roomDoorEuler.y, targetDoorEuler.y);
        Quaternion newRotation = Quaternion.AngleAxis(deltaAngle, Vector3.up);
        room.transform.rotation = newRotation * Quaternion.Euler(0, 180f, 0);

        //position room
        Vector3 roomOffset = roomDoorway.transform.position - room.transform.position;
        room.transform.position = targetDoorway.transform.position - roomOffset;
    }

    //Checks if current room is overlapping previously placed rooms
    bool CheckRoomOverlap(Room room)
    {
        //ENABLE if rooms start overlapping
        Bounds rBounds = room.RoomBounds;
        rBounds.Expand(-0.4f);

        Collider[] colliders = Physics.OverlapBox(rBounds.center, rBounds.extents / 2, room.transform.rotation, roomLayerMask);

        if (colliders.Length > 0)
        {
            foreach (Collider c in colliders)
            {
                if (c.gameObject.transform.parent.gameObject.GetInstanceID() != room.gameObject.GetInstanceID())
                {
                    if (debugMode)
                        Debug.Log("Overlap Detected");

                    return true;
                }
            }
        }

        float expansion = 0.4f;
        Vector3 roomPos = room.gameObject.transform.position;
        Vector3 pt1 = new Vector3(room.RoomBounds.center.x - (room.RoomBounds.extents.x - expansion), room.RoomBounds.center.y + (room.RoomBounds.extents.y - expansion), room.RoomBounds.center.z - (room.RoomBounds.extents.z - expansion));
        Vector3 pt2 = new Vector3(room.RoomBounds.center.x - (room.RoomBounds.extents.x - expansion), room.RoomBounds.center.y - (room.RoomBounds.extents.y - expansion), room.RoomBounds.center.z + (room.RoomBounds.extents.z - expansion));
        Vector3 pt3 = new Vector3(room.RoomBounds.center.x - (room.RoomBounds.extents.x - expansion), room.RoomBounds.center.y + (room.RoomBounds.extents.y - expansion), room.RoomBounds.center.z + (room.RoomBounds.extents.z - expansion));
        Vector3 pt4 = new Vector3(room.RoomBounds.center.x + (room.RoomBounds.extents.x - expansion), room.RoomBounds.center.y - (room.RoomBounds.extents.y - expansion), room.RoomBounds.center.z - (room.RoomBounds.extents.z - expansion));
        Vector3 pt5 = new Vector3(room.RoomBounds.center.x + (room.RoomBounds.extents.x - expansion), room.RoomBounds.center.y + (room.RoomBounds.extents.y - expansion), room.RoomBounds.center.z - (room.RoomBounds.extents.z - expansion));
        Vector3 pt6 = new Vector3(room.RoomBounds.center.x + (room.RoomBounds.extents.x - expansion), room.RoomBounds.center.y - (room.RoomBounds.extents.y - expansion), room.RoomBounds.center.z + (room.RoomBounds.extents.z - expansion));

        foreach (Room r in rooms)
        {
            if (r.RoomBounds.Intersects(room.RoomBounds) ||
                r.RoomBounds.Contains(roomPos) || r.RoomBounds.Contains(room.RoomBounds.min) || r.RoomBounds.Contains(room.RoomBounds.max) || r.RoomBounds.Contains(pt1)
                || r.RoomBounds.Contains(pt2) || r.RoomBounds.Contains(pt3) || r.RoomBounds.Contains(pt4) || r.RoomBounds.Contains(pt5) || r.RoomBounds.Contains(pt6))
            {
                if (debugMode)
                    Debug.Log("Overlap Detected");

                return true;
            }
        }

        return false;
    }
}

[CustomEditor(typeof(LevelBuilder))]
public class LevelBuilderEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        if (GUI.Button(new Rect(0, 20, 400, 20), "Reset all"))
        {
<<<<<<< HEAD
<<<<<<< HEAD
            //PlayerPrefs.SetInt("Level", 1);
            //PlayerPrefs.SetInt("Seed", System.DateTime.Now.Second);
            //PlayerPrefs.Save();

            LevelBuilder lvlBuilder = GameObject.FindGameObjectWithTag("LevelBuilder").GetComponent<LevelBuilder>();

            lvlBuilder.saveGame.ResetAll(1);
=======
            PlayerPrefs.SetInt("Level", 1);
            PlayerPrefs.SetInt("Seed", System.DateTime.Now.Second);
            PlayerPrefs.Save();
>>>>>>> parent of 76823b3e... Save Class
=======
            PlayerPrefs.SetInt("Level", 1);
            PlayerPrefs.SetInt("Seed", System.DateTime.Now.Second);
            PlayerPrefs.Save();
>>>>>>> parent of 76823b3e... Save Class
        }

    }
}