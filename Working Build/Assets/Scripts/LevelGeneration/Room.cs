using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    public Doorway[] doorways;
    public MeshCollider meshCollider;

    [Header("Please use placement prefabs.")]
    public bool randomizeObjectPlacement = false;
    public GameObject[] objectPrefabs1x1m;
    public GameObject[] objectPrefabs2x2m;
    public GameObject[] objectPrefabs3x3m;
    public GameObject[] gameplayPrefabs;

    private List<GameObject> objects1x1m = new List<GameObject>();
    private List<GameObject> objects2x2m = new List<GameObject>();
    private List<GameObject> objects3x3m = new List<GameObject>();
    private List<GameObject> gameObjects = new List<GameObject>();

    public Bounds RoomBounds
    {
        get { return meshCollider.bounds; }
    }

    private void OnDrawGizmos()
    {
        //Draw Bounds
        if (meshCollider != null)
        {
            Bounds bounds = new Bounds(RoomBounds.center, RoomBounds.size);
            bounds.Expand(-0.4f);
            Gizmos.DrawWireCube(bounds.center, bounds.size);
        }
      
        ////Displays most points for collision
        //float expansion = 0.4f;
        //Vector3 pt1 = new Vector3(RoomBounds.center.x - (RoomBounds.extents.x - expansion), RoomBounds.center.y + (RoomBounds.extents.y - expansion), RoomBounds.center.z - (RoomBounds.extents.z - expansion));
        //Vector3 pt2 = new Vector3(RoomBounds.center.x - (RoomBounds.extents.x - expansion), RoomBounds.center.y - (RoomBounds.extents.y - expansion), RoomBounds.center.z + (RoomBounds.extents.z - expansion));
        //Vector3 pt3 = new Vector3(RoomBounds.center.x - (RoomBounds.extents.x - expansion), RoomBounds.center.y + (RoomBounds.extents.y - expansion), RoomBounds.center.z + (RoomBounds.extents.z - expansion));
        //Vector3 pt4 = new Vector3(RoomBounds.center.x + (RoomBounds.extents.x - expansion), RoomBounds.center.y - (RoomBounds.extents.y - expansion), RoomBounds.center.z - (RoomBounds.extents.z - expansion));
        //Vector3 pt5 = new Vector3(RoomBounds.center.x + (RoomBounds.extents.x - expansion), RoomBounds.center.y + (RoomBounds.extents.y - expansion), RoomBounds.center.z - (RoomBounds.extents.z - expansion));
        //Vector3 pt6 = new Vector3(RoomBounds.center.x + (RoomBounds.extents.x - expansion), RoomBounds.center.y - (RoomBounds.extents.y - expansion), RoomBounds.center.z + (RoomBounds.extents.z - expansion));
        //Gizmos.DrawLine(RoomBounds.center, pt1);
        //Gizmos.DrawLine(RoomBounds.center, pt2);
        //Gizmos.DrawLine(RoomBounds.center, pt3);
        //Gizmos.DrawLine(RoomBounds.center, pt4);
        //Gizmos.DrawLine(RoomBounds.center, pt5);
        //Gizmos.DrawLine(RoomBounds.center, pt6);
    }

    private void Start()
    {
        GetChildObjects(transform);

        if (objects1x1m.Count > 0 || objects2x2m.Count > 0 || objects3x3m.Count > 0 || gameObjects.Count > 0)
        {
            PlaceItems();
        }
    }

    private void GetChildObjects(Transform parent)
    {
        for (int i = 0; i < parent.childCount; i++)
        {
            Transform child = parent.GetChild(i);
            if (child.tag == "1x1m")
            {
                objects1x1m.Add(child.gameObject);
                child.gameObject.SetActive(false);
            }
            else if (child.tag == "2x2m")
            {
                objects2x2m.Add(child.gameObject);
                child.gameObject.SetActive(false);
            }
            else if (child.tag == "3x3m")
            {
                objects3x3m.Add(child.gameObject);
                child.gameObject.SetActive(false);
            }
            else if(child.tag == "GameplayItem")
            {
                gameObjects.Add(child.gameObject);
                child.gameObject.SetActive(false);
            }
            if (child.childCount > 0)
            {
                GetChildObjects(child.transform);
            }
        }
    }

    private void PlaceItems()
    {
        //Temp is a 1 if randomize if off, 2 if on.
        int temp = randomizeObjectPlacement ? 2 : 1;

        if (objectPrefabs1x1m.Length > 0)
        {
            foreach (GameObject o in objects1x1m)
            {
                if (Random.Range(0, temp) == 0)
                {
                    int random = Random.Range(0, objectPrefabs1x1m.Length);

                    //Spawn object
                    GameObject obj = Instantiate(objectPrefabs1x1m[random]);
                    obj.transform.parent = transform;
                    obj.transform.position = o.transform.position;
                    //obj.transform.Rotate(Vector3.up, Random.Range(0, 360));
                }
            }
        }
        if (objectPrefabs2x2m.Length > 0)
        {
            foreach (GameObject o in objects2x2m)
            {
                if (Random.Range(0, temp) == 0)
                {
                    int random = Random.Range(0, objectPrefabs2x2m.Length);

                    //Spawn object
                    GameObject obj = Instantiate(objectPrefabs1x1m[random]);
                    obj.transform.parent = transform;
                    obj.transform.position = o.transform.position;
                    //obj.transform.Rotate(Vector3.up, Random.Range(0, 360));
                }
            }
        }
        if (objectPrefabs3x3m.Length > 0)
        {
            foreach (GameObject o in objects3x3m)
            {
                if (Random.Range(0, temp) == 0)
                {
                    int random = Random.Range(0, objectPrefabs3x3m.Length);

                    //Spawn object
                    GameObject obj = Instantiate(objectPrefabs3x3m[random]);
                    obj.transform.parent = transform;
                    obj.transform.position = o.transform.position;
                    //obj.transform.Rotate(Vector3.up, Random.Range(0, 360));
                }
            }
        }

        //Place gameplay items in gameplay spots
        if (gameplayPrefabs.Length > 0)
        {
            foreach (GameObject go in gameObjects)
            {
                //Will randomly choose between 0 and temp. Temp is a 1 if randomize if off, 2 if on.
                if (Random.Range(0, temp) == 0)
                {
                    int random = Random.Range(0, gameplayPrefabs.Length);

                    //Spawn object
                    GameObject obj = Instantiate(gameplayPrefabs[random]);
                    obj.transform.parent = transform;
                    obj.transform.position = go.transform.position;
                    //obj.transform.Rotate(Vector3.up, Random.Range(0, 360));
                }
            }
        }
    }
}
