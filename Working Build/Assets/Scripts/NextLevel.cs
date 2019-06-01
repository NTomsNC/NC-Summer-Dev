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
<<<<<<< HEAD
            PlayerPrefs.SetInt("Level", level + 1);
            PlayerPrefs.SetInt("Seed", System.DateTime.Now.Second);
            PlayerPrefs.Save();
=======
            saveGame = GameObject.FindGameObjectWithTag("LevelBuilder").GetComponent<LevelBuilder>().saveGame;
            saveNum = GameObject.FindGameObjectWithTag("LevelBuilder").GetComponent<LevelBuilder>().saveNum;
            Transform pTransform = new GameObject().transform;
            pTransform.position = new Vector3(0,1,0);

            saveGame.Save(saveGame.level + 1, System.DateTime.Now.Second, pTransform, saveNum);
>>>>>>> parent of 0db3f975... DateTime.Now.Seconds changed to DateTime.UTCTime.ticks

            SceneManager.LoadScene(0);
        }
    }
}
