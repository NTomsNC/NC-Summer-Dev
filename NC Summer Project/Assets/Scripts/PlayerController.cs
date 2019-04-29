using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    // Movement direction
    public Vector2 movementDirection;
    public float movementSpeed;

    // Player Stats
    [Space(15)]
    public float speed = 10;

    // Game Components
    private Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        HandleInputs();
    }

    // Update the basic player direction
    private void HandleInputs()
    {
        movementDirection = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        movementSpeed = Mathf.Clamp(movementDirection.magnitude, 0.0f, 1.0f);

        movementDirection.Normalize();

        rb.velocity = (movementDirection * movementSpeed) * speed;

        if (rb.velocity != Vector2.zero)
            transform.rotation = Quaternion.LookRotation(movementDirection, Vector3.forward);
    }
}
