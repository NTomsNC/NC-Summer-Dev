using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

// Variables
// Save Data holds the savedataID -> Index in the save data array in the menu controller
// Save Data holds the saveSeed -> The Seed for the level loader to load 
// Save Data holds the loadScene -> This is the scene to load(normally the main game scene)

// Functions
// LoadWithSaveData() -> Loads the level with the save data seed
// RemoveSave() -> Tells the menu conmtroller to remove this save data from its list of save data

public class SaveData : MonoBehaviour
{
    // The Scene to load
    static public int loadSceneID = 0;

    public int saveDataID;
    public GameObject txtName;

    // Called on Start
    private void Start()
    {
        //Temp save class to load up information for the save.
        SaveClass save = new SaveClass();
        save.Load(saveDataID);

        // Sets the saves name
        txtName.GetComponent<Text>().text = "Save #" + saveDataID + " with seed #" + save.seed;
    }

    // Called but the Load btn
    public void LoadSave()
    {
        SaveClass.SetSaveUsed(saveDataID);

        SceneManager.LoadScene(loadSceneID);
    }

    // Called but the Delete btn
    public void RemoveSave()
    {
        MenuController.Instance.RemoveSaveState(saveDataID);

        //This deletes the data involved with the save
        SaveClass.DeleteSave(saveDataID);
    }
}
