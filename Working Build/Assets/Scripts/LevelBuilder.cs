using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class LevelBuilder : MonoBehaviour
{
    // Singleton
    #region Singleton Pattern
    public static LevelBuilder Instance { get; private set; }
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    #endregion
    
    // Public
    #region Public
    [Header("Character Prefabs")]
    public GameObject Player;

    [Header("Room Prefabs")]
    public Room startRoomPrefab;
    public Room endRoomPrefab;

    [Space(5)]
    public LayerMask roomLayerMask;

    public List<Room> roomPrefabs = new List<Room>();

    [Header("Level Generation Prefabs")]

    [Tooltip("For testing purposes. Disable when building game")]
    public Vector2 iterationRange = new Vector2(5, 15);
    public bool useIterationRange = false;
    #endregion

    // Private
    #region Private
    int level;

    StartRoom startRoom;
    EndRoom endRoom;

    List<Room> rooms = new List<Room>();
    List<Doorway> availableDoorways = new List<Doorway>();
    #endregion

    //Used to get the level for other scripts
    public int Level
    {
        get { return level; }
    }

    private void Start()
    {
        if(roomLayerMask == 0) roomLayerMask = LayerMask.GetMask("Rooms");

        StartCoroutine("GenerateLevel");

        level = PlayerPrefs.GetInt("Level", 1);
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
        if (!useIterationRange)  iterations = Random.Range(level * 2 + 5 , level * 3);
        else iterations = Random.Range((int)iterationRange.x, (int)iterationRange.y);

        for (int i = 0; i < iterations; i++)
        {
            // Place random room from list
            yield return StartCoroutine(PlaceRoom());
        }

        //Place end room
        yield return StartCoroutine(PlaceEndRoom());

        //Level generation finished
        Debug.Log("Generation Finished");

        PlacePlayer();
    }

    //Places starting room
    void PlaceStartRoom()
    {
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

    //Used to place a room and determine location and rotation
    IEnumerator PlaceRoom()
    {
        WaitForFixedUpdate interval = new WaitForFixedUpdate();
        Debug.Log("Place room");

        //Logarithmetically choose a room
        int a = Random.Range(0, roomPrefabs.Count);
        int b = Random.Range(0, roomPrefabs.Count);
        int r = a < b ? a : b;
        
        //instantiate room
        Room currentRoom = Instantiate(roomPrefabs[r]) as Room;
        currentRoom.transform.parent = transform;

        //Create doorway list to loop over
        List<Doorway> allAvailableDoors = new List<Doorway>(availableDoorways);
        List<Doorway> currentDoors = new List<Doorway>();

        AddDoorwaysToList(currentRoom, ref currentDoors);

        //Get doorways and add to global list
        AddDoorwaysToList(currentRoom, ref availableDoorways);

        bool roomPlaced = false;

        //Try available doors
        foreach(Doorway availableDoor in allAvailableDoors)
        {
            //try doors in current room
            foreach(Doorway currentDoor in currentDoors)
            {
                //position room
                PositionAtDoorway(ref currentRoom, currentDoor, availableDoor);
                yield return interval;      //slows down placement to allow proper overlap checking. If not here it will check before bounds have actually moved to new location

                if(CheckRoomOverlap(currentRoom))
                {
                    continue;
                }

                roomPlaced = true;

                //Add room to list
                rooms.Add(currentRoom);

                //remove occupied doorways
                currentDoor.gameObject.SetActive(false);
                availableDoorways.Remove(currentDoor);

                availableDoor.gameObject.SetActive(false);
                availableDoorways.Remove(availableDoor);
                
                // Adds a connector
                //GameObject doorObject = Instantiate(frame, currentDoor.transform.position, currentDoor.transform.rotation);
                //doorObject.transform.parent = transform;

                //exit loop if room is placed
                break;
            }

            //Exit loop if room has been placed
            if(roomPlaced)
            {
                break;
            }
        }

        //Room couldnt be placed. Restart generator and try again
        if(!roomPlaced)
        {
            Destroy(currentRoom.gameObject);
            ResetGenerator();
        }
    }

    //Resets the entire generator if room placement fails
    public void ResetGenerator()
    {
        Debug.LogError("Reset generation");

        StopCoroutine("GenerateLevel");

        //Delete all rooms
        if(startRoom)
        {
            Destroy(startRoom.gameObject);
        }
        if(endRoom)
        {
            Destroy(endRoom.gameObject);
        }

        foreach(Room room in rooms)
        {
            Destroy(room.gameObject);
        }

        //GameObject[] doors = GameObject.FindGameObjectsWithTag("Rim");
        //foreach (GameObject go in doors)
        //{
        //    Destroy(go);
        //}

        //Clear Lists
        rooms.Clear();
        availableDoorways.Clear();

        //reset
        StartCoroutine("GenerateLevel");
    }

    //Adds each doorway to a list
    void AddDoorwaysToList(Room room, ref List<Doorway> list)
    {
        foreach(Doorway door in room.doorways)
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
            foreach(Collider c in colliders)
            {
                if(c.gameObject.transform.parent.gameObject.GetInstanceID() != room.gameObject.GetInstanceID())
                {
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
                Debug.Log("Overlap Detected");
                return true;
            }
        }

        return false;
    }
}

//[CustomEditor(typeof(LevelBuilder))]
//public class LevelBuilderEditor : Editor
//{
//    public override void OnInspectorGUI()
//    {
//        DrawDefaultInspector();

//        if(GUI.Button(new Rect(0, 0, 100, 30), "Reset Levels"))
//            PlayerPrefs.SetInt("Level", 1);

//    }
//}