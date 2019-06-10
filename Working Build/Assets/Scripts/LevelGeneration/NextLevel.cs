using UnityEngine;
using UnityEngine.SceneManagement;

public class NextLevel : MonoBehaviour
{
    private SaveClass saveGame;
    private int currentSave;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            //Sets up data in preperation to save
            saveGame = GameObject.FindGameObjectWithTag("LevelBuilder").GetComponent<LevelBuilder>().saveGame;  //Grab savegame from levelbuilder
            currentSave = GameObject.FindGameObjectWithTag("LevelBuilder").GetComponent<LevelBuilder>().currentSave;    //Grab the saveID number
            int seed = SaveClass.GetSeed();

            //Creates a new transform and sets the player transform so player spawns properly in next level
            Transform pTransform = new GameObject().transform; 
            pTransform.position = new Vector3(0,1,0);

            saveGame.Save(saveGame.level + 1, seed, pTransform, currentSave); //Saves the game

            Destroy(pTransform.gameObject);
            SceneManager.LoadScene(0);
        }
    }
}
