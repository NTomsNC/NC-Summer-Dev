using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//--------------------------------------------------------------
//Too be used with VisibilitySphere script and Dissolve material
public class DissolveDistance : MonoBehaviour
{
    [Tooltip("The distance in meters that material will start to dissolve.")]
    public float startDissolveDistance = 1;

    private GameObject sphere;
    private Renderer rend;

    // Start is called before the first frame update
    void Start()
    {
        rend = GetComponent<MeshRenderer>();
        rend.material.shader = Shader.Find("Custom/Dissolve");
        rend.material.SetFloat("_Dissolve", 1);

        sphere = GameObject.FindGameObjectWithTag("VisibilitySphere");
    }

    // Update is called once per frame
    void Update()
    {
        sphere = GameObject.FindGameObjectWithTag("VisibilitySphere");

        if (sphere != null)
        {            
            Vector3 p1 = sphere.GetComponent<SphereCollider>().bounds.ClosestPoint(transform.position);
            Vector3 p2 = GetComponent<MeshCollider>().bounds.ClosestPoint(sphere.transform.position);

            float distance = Vector3.Distance(p1, p2);

            {
                float dissolve = distance / startDissolveDistance;
                if (dissolve > 1) dissolve = 1;

                rend.material.SetFloat("_Dissolve", dissolve);
            }
        }
    }
}
