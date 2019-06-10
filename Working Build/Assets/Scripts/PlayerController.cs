using System.Collections;
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

    private LineRenderer laser;
    public float laserHeight = 0.5f;
    private Vector3 lastPos;

    private void Start()
    {
        if (roomLayerMask == 0) roomLayerMask = LayerMask.GetMask("Everything");
        rb = GetComponent<Rigidbody>();

        laser = GetComponent<LineRenderer>();
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

        if (Physics.Raycast(ray, rayLength, roomLayerMask))
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
        Vector3 mousePos = Input.mousePosition;
        Vector2 screenCenter = new Vector3(Screen.width / 2, Screen.height / 2, 0);
        float angle = Mathf.Atan2(mousePos.y - screenCenter.y, mousePos.x - screenCenter.x); // In rads now -Pi-Pi range    * 180 / Mathf.PI;

        Vector3 point = new Vector3(transform.position.x + Mathf.Cos(angle), transform.position.y, transform.position.z + Mathf.Sin(angle));
        //transform.LookAt(point, Vector3.up);  //Rotates character towards point instantly
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(point - transform.position), rotationSpeed * Time.deltaTime); //This allows for different speeds

        CastLaserForward();
    }

    private void CastLaserForward()
    {
        //Modifies line renderer to show laser from player to aimed location
        Vector3 start = transform.position;
        start.y += laserHeight;
        Vector3 dir = transform.forward;

        Ray ray = new Ray(start, dir);
        RaycastHit hit;
        Physics.Raycast(ray, out hit);

        Debug.DrawRay(start, dir, Color.blue);

        Vector3[] laserPos = new Vector3[2];
        laserPos[0] = transform.position + new Vector3(0, laserHeight, 0);
        laserPos[1] = hit.point;

        if (hit.collider == null)
        {
            laserPos[1] = start + dir * 10;
        }

        lastPos = laserPos[1];  

        laser.SetPositions(laserPos);
    }
}
