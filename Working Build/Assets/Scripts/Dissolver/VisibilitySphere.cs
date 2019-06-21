using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisibilitySphere : MonoBehaviour
{
    public Vector3 collisionCenter = new Vector3();
    public LayerMask layers;
    public float radius = 0.1f;
    public GameObject spherePrefab;
    public float speed = 50;

    private GameObject sphere;
    private bool debugTriggered = false;

    //for use outside script
    public GameObject Sphere
    {
        get { return sphere; }
    }

    // FixedUpdate is called once per physics frame
    void FixedUpdate()
    {
        DetermineCameraVisibility();
        DrawSphere();
    }

    private void DetermineCameraVisibility()
    {
        Vector3 offset = new Vector3(0, 0.5f, 0);
        Ray ray = new Ray(transform.position + offset, Camera.main.transform.position - transform.position);

        RaycastHit hitInfo = new RaycastHit();

        Debug.DrawRay(transform.position + offset, Camera.main.transform.position - transform.position);
        if (Physics.SphereCast( ray, radius, out hitInfo))
        {
            //Sets where the ray hit for dissolver code
            collisionCenter = hitInfo.point;
            //collisionCenter.y = 0.5f;
        }
        else
        {
            //Sets where ray hit to ungodly height
            collisionCenter = Camera.main.transform.position + new Vector3(0, 15, 0);
        }
    }

    private void DrawSphere()
    {
        if(spherePrefab != null)
        {
            if(sphere == null)
            {
                sphere = Instantiate(spherePrefab, collisionCenter, Quaternion.identity);
                sphere.tag = "VisibilitySphere";
            }
            else
            {
                //sphere.transform.position = position;
                sphere.transform.position = Vector3.MoveTowards(sphere.transform.position, collisionCenter, speed * Time.deltaTime);
            }
        }
        else
        {
            if(!debugTriggered)
            {
                Debug.LogError("Sphere Prefab Missing. Please input prefab.");
            }
        }
    }
}
