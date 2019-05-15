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
        Bounds bounds = new Bounds(RoomBounds.center, RoomBounds.size);
        bounds.Expand(-0.1f);
        Gizmos.DrawWireCube(bounds.center, bounds.size) ;
    }
}
