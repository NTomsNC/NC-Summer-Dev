using UnityEngine;
using UnityEngine.SceneManagement;

public class NextLevel : MonoBehaviour
{
    private SaveClass saveGame;
    private int saveNum;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            saveGame = GameObject.FindGameObjectWithTag("LevelBuilder").GetComponent<LevelBuilder>().saveGame;
            saveNum = GameObject.FindGameObjectWithTag("LevelBuilder").GetComponent<LevelBuilder>().saveNum;
            Transform pTransform = new GameObject().transform;
            pTransform.position = new Vector3(0,1,0);

            saveGame.Save(saveGame.level + 1, System.DateTime.Now.Second, pTransform, saveNum);

            Object.Destroy(pTransform.gameObject);
            SceneManager.LoadScene(0);
        }
    }
}
