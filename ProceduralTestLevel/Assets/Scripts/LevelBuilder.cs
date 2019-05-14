using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelBuilder : MonoBehaviour
{
    [Space(5)]
    public Room startRoomPrefab, endRoomPrefab;

    [Space(10)]
    public List<Room> roomPrefabs = new List<Room>();

    [Space(10)]
    public Vector2 iterationRange = new Vector2(3, 10);

    StartRoom startRoom;
    EndRoom endRoom;

    List<Room> rooms = new List<Room>();

    List<Doorway> availableDoorways = new List<Doorway>();

    public LayerMask roomLayerMask;

    private void Start()
    {
        if(roomLayerMask == 0) roomLayerMask = LayerMask.GetMask("Rooms");

        StartCoroutine("GenerateLevel");
    }

    IEnumerator GenerateLevel()
    {
        WaitForSeconds startup = new WaitForSeconds(1);
        WaitForFixedUpdate interval = new WaitForFixedUpdate();

        //yield return startup;

        // Place start room
        PlaceStartRoom();
        yield return interval;

        //Random iterations
        int iterations = Random.Range((int)iterationRange.x, (int)iterationRange.y);

        for (int i = 0; i < iterations; i++)
        {
            // Place random room from list
            PlaceRoom();
            yield return interval;
        }

        //Place end room
        PlaceEndRoom();
        yield return interval;

        //Level generation finished
        Debug.Log("Generation Finished");

        yield return new WaitForSeconds(3);
        //ResetGenerator();
    }

    void PlaceStartRoom()
    {
        Debug.Log("Place start room");

        //Instantiate
        startRoom = Instantiate(startRoomPrefab) as StartRoom;
        startRoom.transform.parent = this.transform;

        //get doorways from current room and add to list
        AddDoorwaysToList(startRoom, ref availableDoorways);

        //position room
        startRoom.transform.position = Vector3.zero;
        startRoom.transform.rotation = Quaternion.identity;
    }

    void PlaceEndRoom()
    {
        Debug.Log("Place end room");
    }

    void PlaceRoom()
    {
        Debug.Log("Place room");

        //instantiate room
        int r = Random.Range(0, roomPrefabs.Count);
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

    void ResetGenerator()
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

        //Clear Lists
        rooms.Clear();
        availableDoorways.Clear();

        //reset
        StartCoroutine("GenerateLevel");
    }

    void AddDoorwaysToList(Room room, ref List<Doorway> list)
    {
        foreach(Doorway door in room.doorways)
        {
            int r = Random.Range(0, list.Count);
            list.Insert(r, door);
        }
    }

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

    bool CheckRoomOverlap(Room room)
    {
        //Bounds bounds = room.RoomBounds;
        //bounds.Expand(-0.1f);

        //Collider[] colliders = Physics.OverlapBox(bounds.center, bounds.size / 2, room.transform.rotation, roomLayerMask);

        //if (colliders.Length > 0)
        //{
        //    //ignore collisions with current room
        //    foreach (Collider c in colliders)
        //    {
        //        if (c.transform.parent.gameObject.Equals(room.gameObject))
        //        {
        //            continue;
        //        }
        //        else
        //        {
        //            Debug.LogError("Overlap Detected");
        //            return true;
        //        }
        //    }
        //}

        foreach(Room r in rooms)
        {
            if((room.RoomBounds.Intersects(r.meshCollider.GetComponent<MeshCollider>().bounds) && !r.transform.gameObject.Equals(room.gameObject)) || r.transform.gameObject.Equals(room.gameObject))
            {
                continue;
            }
            else
            {
                Debug.LogError("Overlap Detected");
                return true;
            }
        }

        return false;
    }
}
