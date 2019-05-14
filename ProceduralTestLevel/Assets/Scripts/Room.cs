using UnityEngine;

public class Room : MonoBehaviour
{
    public Doorway[] doorways;
    public MeshCollider meshCollider;

    public Bounds RoomBounds
    {
        get { return meshCollider.bounds; }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(RoomBounds.center, RoomBounds.size) ;
    }
}
