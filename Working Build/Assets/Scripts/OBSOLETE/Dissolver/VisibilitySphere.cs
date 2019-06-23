using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Place on character to allow dissolve shader to work with dissolve code. This casts a ray towards main camera to allow the dissolving objects to know how far from the ray they are.
public class VisibilitySphere : MonoBehaviour
{
    //public Vector3 collisionCenter = new Vector3();
    public Ray visionRay;
    public LayerMask rayLayers;

    // FixedUpdate is called once per physics frame
    void FixedUpdate()
    {
        DetermineCameraVisibility();
    }

    private void DetermineCameraVisibility()
    {
        Vector3 offset = new Vector3(0, 0.5f, 0);
        visionRay = new Ray(transform.position + offset, Camera.main.transform.position - transform.position);
        Debug.DrawRay(transform.position + offset, Camera.main.transform.position - transform.position);
    }
}
