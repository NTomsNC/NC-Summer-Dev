using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///  This script shoots a ray towards an object and stores all objects into an array of objects. It then sets the objects material to true.
///  It will compare the array to a new list from the next ray. If a change has occured, set all materials dissolves to false and repopulate the array.
/// </summary>

public class VisionBlockedDetector : MonoBehaviour
{
    public float radius = 0.25f;
    public LayerMask layers;

    GameObject player;
    RaycastHit[] occluders;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void FixedUpdate()
    {
        Ray ray = new Ray(transform.position, (player.transform.position + new Vector3(0, 0.5f, 0)) - transform.position);
        RaycastHit[] hit = Physics.SphereCastAll(ray, radius, Vector3.Distance(transform.position, player.transform.position), layers);

        AdjustDissolve(hit);

        //If ray hit anything
        if (hit.Length > 0)
        {
            Debug.DrawRay(transform.position, (player.transform.position + new Vector3(0, 0.5f, 0)) - transform.position, Color.red);            
        }
        else
        {
            Debug.DrawRay(transform.position, (player.transform.position + new Vector3(0, 0.5f, 0)) - transform.position);
        }
    }

    private void AdjustDissolve(RaycastHit[] hit)
    {
        //Set dissolve to false on all objects
        if (occluders != null)
        {
            foreach (RaycastHit g in occluders)
            {
                if (g.collider.gameObject.GetComponent<MeshRenderer>().material.shader.name == "Custom/Dissolve")
                {
                    g.collider.gameObject.GetComponent<MeshRenderer>().material.SetInt("_DissolveEnabled", 0);
                }
            }
        }

        occluders = hit;

        //Set dissolve to true on all objects
        foreach (RaycastHit g in occluders)
        {
            if (g.collider.gameObject.GetComponent<MeshRenderer>().material.shader.name == "Custom/Dissolve")
            {
                g.collider.gameObject.GetComponent<MeshRenderer>().material.SetInt("_DissolveEnabled", 1);
            }
        }
    }
}
