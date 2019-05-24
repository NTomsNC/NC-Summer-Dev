using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    public Doorway[] doorways;
    public MeshCollider meshCollider;

    public GameObject[] objectPrefabs;
    private List<GameObject> objectPlacements = new List<GameObject>();

    public Bounds RoomBounds
    {
        get { return meshCollider.bounds; }
    }

    private void OnDrawGizmos()
    {
        //Draw Bounds
        Bounds bounds = new Bounds(RoomBounds.center, RoomBounds.size);
        bounds.Expand(-0.4f);
        Gizmos.DrawWireCube(bounds.center, bounds.size);

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

        if(objectPlacements.Count > 0)
        {
            PlaceItems();
        }
    }

    private void GetChildObjects(Transform parent)
    {
        for (int i = 0; i < parent.childCount; i++)
        {
            Transform child = parent.GetChild(i);
            if (child.tag == "ObjectPlacement")
            {
                objectPlacements.Add(child.gameObject);
            }
            if(child.childCount > 0)
            {
                GetChildObjects(child.transform);
            }
        }
    }

    private void PlaceItems()
    {
        foreach(GameObject o in objectPlacements)
        {
            int random = Random.Range(0, objectPrefabs.Length);

            //Spawn object
            GameObject obj = Instantiate(objectPrefabs[random]);
            obj.transform.parent = transform;
            obj.transform.position = o.transform.position;
            //obj.transform.Rotate(Vector3.up, Random.Range(0, 360));
        }
    }
}
