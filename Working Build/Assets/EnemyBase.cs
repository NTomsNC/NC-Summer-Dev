using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBase : MonoBehaviour
{
    GameObject target;
    RandomWalk random;
    NavMeshAgent agent;
    bool chasingPlayer = false;

    // Start is called before the first frame update
    void Start()
    {
        random = GetComponent<RandomWalk>();
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        if(random.enabled == false && chasingPlayer)
        {
            agent.SetDestination(target.transform.position);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        //If player gets close enough
        if (other.tag == "Player" && !chasingPlayer)
        {
            //Check to see if object can see player
            Vector3 start = transform.position;
            start.y += 1;

            Ray ray = new Ray(start, other.transform.position - start);
            RaycastHit hit;
            Debug.DrawRay(start, other.transform.position - start);

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.gameObject.tag == "Player")
                {
                    //If so, start moving toward player                    
                    chasingPlayer = true;
                    random.enabled = false;
                    target = other.transform.gameObject;
                }
            }
        }
    }
}
