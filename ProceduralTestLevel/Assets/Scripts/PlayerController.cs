using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5;
    public float rotationSpeed = 3;

    public float dashSpeed = 3;
    public float dashWaitTime = 1f;
    private float dashTimer = 0;

    public GameObject mesh;

    private string vInputAxis = "Vertical";
    private string hInputAxis = "Horizontal";
    private Rigidbody rb;

    public bool grounded = false;
    public LayerMask roomLayerMask;

    private void Start()
    {
        if (roomLayerMask == 0) roomLayerMask = LayerMask.GetMask("Rooms");
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        dashTimer += Time.deltaTime;

        float vAxis = Input.GetAxis(vInputAxis);
        float hAxis = Input.GetAxis(hInputAxis);

        CheckGround();

        if ((Input.GetButton(vInputAxis) || Input.GetButton(hInputAxis)) && dashTimer > dashWaitTime && grounded)
        {
            rb.isKinematic = false;
            Vector3 nextDir = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
            mesh.transform.rotation = Quaternion.Lerp(mesh.transform.rotation, Quaternion.LookRotation(nextDir), rotationSpeed * Time.deltaTime);
            rb.velocity = new Vector3(nextDir.x * moveSpeed, rb.velocity.y, nextDir.z * moveSpeed);

            if (Input.GetButtonDown("Dash") && dashTimer > dashWaitTime)
            {
                rb.velocity = nextDir * dashSpeed;
                dashTimer = 0;
            }          
        }
        else if(dashTimer > dashWaitTime && grounded)
        {
            rb.isKinematic = true;
        }
    }

    private void CheckGround()
    {
        Ray ray = new Ray(transform.position, Vector3.down * 0.5f);
        Debug.DrawRay(ray.origin, ray.direction * 0.5f);

        if(Physics.Raycast(ray, roomLayerMask))
        {
            grounded = true;
        }
        else
        {
            grounded = false;
        }
    }
}
