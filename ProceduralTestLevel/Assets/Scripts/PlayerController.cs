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

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        dashTimer += Time.deltaTime;

        float vAxis = Input.GetAxis(vInputAxis);
        float hAxis = Input.GetAxis(hInputAxis);

        if ((Input.GetButton(vInputAxis) || Input.GetButton(hInputAxis)) && dashTimer > 0.5f)
        {
            Vector3 nextDir = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
            transform.Translate(nextDir * moveSpeed * Time.deltaTime);
            mesh.transform.rotation = Quaternion.Lerp(mesh.transform.rotation, Quaternion.LookRotation(nextDir), rotationSpeed * Time.deltaTime);

            rb.velocity = Vector3.zero;

            if (Input.GetButtonDown("Dash") && dashTimer > dashWaitTime)
            {
                rb.velocity = nextDir * dashSpeed;
                dashTimer = 0;
            }          
        }        
    }
}
