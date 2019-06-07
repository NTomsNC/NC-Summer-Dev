using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTrackPlayer : MonoBehaviour
{
    public Vector3 distanceFromPlayer = new Vector3(0,3,0);
    GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(player.transform.position.x + distanceFromPlayer.x, player.transform.position.y + distanceFromPlayer.y, player.transform.position.z + distanceFromPlayer.z);
        transform.LookAt(player.transform);
    }
}
