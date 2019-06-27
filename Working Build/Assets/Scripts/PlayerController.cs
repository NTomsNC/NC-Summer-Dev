using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float rayLength = 0.75f;
    public float moveSpeed = 5;
    public float rotationSpeed = 3;

    public DashType dashType = DashType.Forward;
    public float dashSpeed = 3;
    public float dashWaitTime = 1f;
    private float dashTimer = 0;

    private Rigidbody rb;
    public LayerMask roomLayerMask;
    private LineRenderer laser;

    public float laserHeight = 0.5f;

    public bool disableControl = false;

    //Start is called once object is created
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

    //Shoots a ray on selected ground layers and returns true if it hits anything in desire range
    private bool CheckGround()
    {
        Ray ray = new Ray(transform.position, Vector3.down * rayLength);
        Debug.DrawRay(ray.origin, ray.direction * rayLength);

        if (Physics.Raycast(ray, rayLength, roomLayerMask))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    //Makes character move by getting keyboard axis input. It than gets inputs and sets velocity based on input.
    private void Move()
    {
        if (!disableControl)
        {
            float vAxis = Input.GetAxisRaw("Horizontal");
            float hAxis = Input.GetAxisRaw("Vertical");

            Vector3 nextDir = new Vector3();

            if ((vAxis > 0 || vAxis < 0 || hAxis > 0 || hAxis < 0) && CheckGround())
            {
                //rb.isKinematic = false;
                nextDir = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
                //transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(nextDir), rotationSpeed * Time.deltaTime);
                rb.velocity = new Vector3(nextDir.x * moveSpeed, rb.velocity.y, nextDir.z * moveSpeed);
            }
            if (Input.GetButtonDown("Dash") && CheckGround())
            {
                StartCoroutine(Dash(nextDir));
            }
        }
    }

    //Makes character face mouse position based on mouse position to screen center
    private void FaceMouse()
    {
        if (!disableControl)
        {
            Vector3 mousePos = Input.mousePosition;
            Vector2 screenCenter = new Vector3(Screen.width / 2, Screen.height / 2, 0);
            float angle = Mathf.Atan2(mousePos.y - screenCenter.y, mousePos.x - screenCenter.x); // In rads now -Pi-Pi range    * 180 / Mathf.PI;

            Vector3 point = new Vector3(transform.position.x + Mathf.Cos(angle), transform.position.y, transform.position.z + Mathf.Sin(angle));
            //transform.LookAt(point, Vector3.up);  //Rotates character towards point instantly
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(point - transform.position), rotationSpeed * Time.deltaTime); //This allows for different speeds
        }

        CastLaserForward();
    }

    //Shoots a ray from character position forwards than sets a line render at the start and end points of that ray
    private void CastLaserForward()
    {
        //Modifies line renderer to show laser from player to aimed location
        Vector3 start = transform.position;
        start.y += laserHeight;
        Vector3 dir = transform.forward;

        //Shoots a ray
        Ray ray = new Ray(start, dir);
        RaycastHit hit;
        Physics.Raycast(ray, out hit);

        Debug.DrawRay(start, dir, Color.blue);

        //Sets laser positions
        Vector3[] laserPos = new Vector3[2];
        laserPos[0] = transform.position + new Vector3(0, laserHeight, 0);
        laserPos[1] = hit.point;

        //If no collision happened, set the end point 10 meters forward offscreen
        if (hit.collider == null)
        {
            laserPos[1] = start + dir * 10;
        }

        //Sends all positions to line renderer
        laser.SetPositions(laserPos);
    }

    //Used to start the dash velocity and disable control for a set time
    IEnumerator Dash(Vector3 direction)
    {
        if (dashType == DashType.Forward)
            rb.velocity = Vector3.Scale(transform.forward, dashSpeed * new Vector3((Mathf.Log(1f / (Time.deltaTime * rb.drag + 1)) / -Time.deltaTime), 0, (Mathf.Log(1f / (Time.deltaTime * rb.drag + 1)) / -Time.deltaTime)));
        else
            rb.velocity = new Vector3(direction.x * dashSpeed, rb.velocity.y, direction.z * dashSpeed);

        disableControl = true;
        yield return new WaitForSeconds(dashWaitTime);
        disableControl = false;
    }
}

public enum DashType // This is for a dropdown box
{
    Forward,
    MovementDirection
};