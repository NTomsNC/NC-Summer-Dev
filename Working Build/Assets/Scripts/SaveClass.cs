using UnityEngine;

[System.Serializable]
public class SaveClass
{
    public int seed;
    public int level;

    [SerializeField]
    private int playerHealth = 0;
    [SerializeField]
    private Vector3 playerPos;
    [SerializeField]
    private Quaternion playerRotation;

    //---------------------------------------------------------------------
    //Used to save the state of the game.
    public void Save(int lvl, int s, Transform pTransform, int saveNum)
    {
        level = lvl;
        seed = s;
        playerPos = pTransform.position;
        playerRotation = pTransform.rotation;

        string jsonSave = JsonUtility.ToJson(this);
        string temp = "Save" + saveNum;
        PlayerPrefs.SetString("Save" + saveNum, jsonSave);

        PlayerPrefs.Save();
    }

    //---------------------------------------------------------------------
    //Used to load the state of the game.
    public void Load(int saveNum)
    {
        if (PlayerPrefs.HasKey("Save" + saveNum))
        {
            SaveClass temp = JsonUtility.FromJson<SaveClass>(PlayerPrefs.GetString("Save" + saveNum));

            if (temp == null)
            {
                Debug.LogError("Error: Save could not be loaded. Check save number. Making new save");
                temp = new SaveClass();
                temp.ResetAll(saveNum);
            }

            level = temp.level;
            seed = temp.seed;
            playerHealth = temp.playerHealth;
            playerPos = temp.playerPos;
            playerRotation = temp.playerRotation;
        }
        else
        {
            ResetAll(saveNum);
        }
    }

    public void ResetAll(int saveNum)
    {
        level = 1;
        seed = (int)System.DateTime.UtcNow.Ticks;

        Vector3 tempVector = new Vector3(0, 1, 0);

        Transform tempT = new GameObject().transform;
        tempT.position = tempVector;
        tempT.rotation = Quaternion.identity;

        Save(level, seed, tempT, saveNum);

        Object.Destroy(tempT.gameObject);
    }

    public Transform PlayerTransform()
    {
        Transform tempT = new GameObject().transform;
        tempT.position = playerPos;
        tempT.rotation = playerRotation;

        return tempT;
    }
}
