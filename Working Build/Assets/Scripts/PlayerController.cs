﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float rayLength = 0.75f;
    public float moveSpeed = 5;
    public float rotationSpeed = 3;

    public float dashSpeed = 3;
    public float dashWaitTime = 1f;
    private float dashTimer = 0;

    private string vInputAxis = "Vertical";
    private string hInputAxis = "Horizontal";
    private Rigidbody rb;

    public bool grounded = false;
    public LayerMask roomLayerMask;

    private void Start()
    {
        if (roomLayerMask == 0) roomLayerMask = LayerMask.GetMask("Everything");
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        dashTimer += Time.deltaTime;
        CheckGround();
        Move();
        FaceMouse();
    }

    private void CheckGround()
    {
        Ray ray = new Ray(transform.position, Vector3.down * rayLength);
        Debug.DrawRay(ray.origin, ray.direction * rayLength);

        if(Physics.Raycast(ray, rayLength, roomLayerMask))
        {
            grounded = true;
        }
        else
        {
            grounded = false;
        }
    }

    private void Move()
    {
        float vAxis = Input.GetAxisRaw(vInputAxis);
        float hAxis = Input.GetAxisRaw(hInputAxis);
        if ((Input.GetButton(vInputAxis) || Input.GetButton(hInputAxis)) && dashTimer > dashWaitTime && grounded)
        {
            //rb.isKinematic = false;
            Vector3 nextDir = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
            //transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(nextDir), rotationSpeed * Time.deltaTime);
            rb.velocity = new Vector3(nextDir.x * moveSpeed, rb.velocity.y, nextDir.z * moveSpeed);

            if (Input.GetButtonDown("Dash") && dashTimer > dashWaitTime)
            {
                rb.velocity = nextDir * dashSpeed;
                dashTimer = 0;
            }
        }
        else if (dashTimer > dashWaitTime && grounded)
        {
            //rb.isKinematic = true;
        }
    }

    private void FaceMouse()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        Physics.Raycast(ray, out hit);

        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(hit.point), rotationSpeed * Time.deltaTime);
    }
}
