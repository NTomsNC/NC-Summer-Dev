using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//--------------------------------------------------------------
//Too be used with VisibilitySphere script and Dissolve material
public class DissolveDistance : MonoBehaviour
{
    [Tooltip("The distance in meters that material will start to dissolve.")]
    public float startDissolveDistance = 1;
    public float distance;

    private Renderer rend;
    public Ray visionRay;

    // Start is called before the first frame update
    void Start()
    {
        rend = GetComponent<MeshRenderer>();
        rend.material.shader = Shader.Find("Custom/Dissolve");
        rend.material.SetFloat("_Dissolve", 1);
    }

    // Update is called once per frame
    void Update()
    {
        visionRay = GameObject.FindGameObjectWithTag("Player").GetComponent<VisibilitySphere>().visionRay;

        Dissolve();
    }

    private void Dissolve()
    {
        distance = Vector3.Cross(visionRay.direction, transform.position - visionRay.origin).magnitude;

        float dissolve = distance / startDissolveDistance;
        if (dissolve > 1) dissolve = 1;

        rend.material.SetFloat("_Dissolve", dissolve);
    }
}
