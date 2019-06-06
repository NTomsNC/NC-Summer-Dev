using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageOnContact : MonoBehaviour
{
    public float damage = 5;
    public float pushBack = 20;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Health pHP = collision.gameObject.GetComponent<Health>();
            Rigidbody rb = collision.gameObject.GetComponent<Rigidbody>();

            pHP.Damage(damage);
            rb.velocity += (collision.gameObject.transform.position - transform.position) * pushBack;
        }
    }
}
