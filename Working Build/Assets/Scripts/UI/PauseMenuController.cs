using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenuController : MonoBehaviour
{
    public GameObject canvasObject;
    public float pauseSlowSpeed = 0.02f;

    private PlayerController pc;

    // Start is called before the first frame update
    void Start()
    {
        canvasObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(pc == null)
        {
            pc = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        }

        if(Input.GetButtonDown("Cancel") && Time.timeScale == 1)
        {
            Pause();
        }
        else if (Input.GetButtonDown("Cancel") && Time.timeScale == pauseSlowSpeed)
        {
            Unpause();
        }
    }

    public void Pause()
    {
        Time.timeScale = pauseSlowSpeed;
        Time.fixedDeltaTime = Time.timeScale * pauseSlowSpeed;
        canvasObject.SetActive(true);
        pc.disableControl = true;
    }

    public void Unpause()
    {
        Time.timeScale = 1;
        Time.fixedDeltaTime = Time.deltaTime;
        canvasObject.SetActive(false);
        pc.disableControl = false;
    }
}
