using UnityEngine;
using UnityEngine.SceneManagement;

public class NextLevel : MonoBehaviour
{
    public LevelBuilder levelBuilder;
    int level;

    private void Start()
    {
        levelBuilder = GameObject.FindGameObjectWithTag("LevelBuilder").GetComponent<LevelBuilder>();
        level = levelBuilder.Level;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            PlayerPrefs.SetInt("Level", level + 1);
            PlayerPrefs.SetInt("Seed", System.DateTime.Now.Second);
            PlayerPrefs.Save();

            SceneManager.LoadScene(0);
        }
    }
}
