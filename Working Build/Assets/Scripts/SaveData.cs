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
    // Used to remove the Save Data
    public int saveDataID;

    // The Save data seed to load
    public int saveSeed;

    // The Scene to load
    public int loadSceneID = 0;

    public bool GenerateRandomKey = false;

    public GameObject txtName;

    // Called on Start
    private void Start()
    {
        // Gets a randome seed
        if(GenerateRandomKey)
            saveSeed = Random.Range(0, 1000);

        // Sets the saves name
        txtName.GetComponent<Text>().text = "Save #" + saveDataID + ", With seed of (" + saveSeed + ")" ;
    }

    // Called but the Load btn
    public void LoadWithSaveData()
    {
        PlayerPrefs.SetInt("CurrentSave", saveSeed);
        PlayerPrefs.Save();

        SceneManager.LoadScene(loadSceneID);
    }

    // Called but the Delete btn
    public void RemoveSave()
    {
        MenuController.Instance.RemoveSaveState(saveDataID);
    }
}
