using UnityEngine;

[System.Serializable]
public class SaveClass
{
    //All data to be stored
    public int seed;
    public int level;
    private int playerHealth = 0;

    private Vector3 playerPos;
    public Vector3 PlayerPos { get { return playerPos; } }

    private Quaternion playerRotation;
    public Quaternion PlayerRotation { get { return playerRotation; } }

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

    //---------------------------------------------------------------------
    //Used to populate a save with new information. Used when making a new save file if one cannot be loaded
    public void ResetAll(int saveNum)
    {
        level = 1;
        seed = GetSeed();

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

    //---------------------------------------------------------------------
    //Deletes the save from the system
    static public void DeleteSave(int saveNum)
    {
        PlayerPrefs.DeleteKey("Save" + saveNum);
    }

    //---------------------------------------------------------------------
    //Sets the save file to load when opening levelBuilder
    static public void SetSaveUsed(int saveNum)
    {
        PlayerPrefs.SetInt("CurrentSave", saveNum);
        PlayerPrefs.Save();
    }
    
    //---------------------------------------------------------------------
    //Returns a new seed
    static public int GetSeed()
    {
        System.DateTime epochStart = new System.DateTime(1970, 1, 1, 0, 0, 0, System.DateTimeKind.Utc);
        return (int)(System.DateTime.UtcNow - epochStart).TotalSeconds;
    }
}
