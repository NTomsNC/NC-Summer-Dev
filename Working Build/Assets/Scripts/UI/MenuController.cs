﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// Variables
// The MenuController, gameStates -> Used to know what menu logic should be used
// The MenuController, mainMenuCanvas, playerSavesCanvas, SettingsCanvas -> hold the graphi to display/load
// The MenuController, SaveStatePrefab -> Used to instantiate the game save prefab when loading a game
// The MenuController, numberOfSaves -> The max number of saves the game should load
// The MenuController, SaveStates -> A list of save state gameobjects

// Functions
// OnAwake() -> Sets up a Singleton, use MenuController.Instance to use any public functions without having to reference this class
// Update()-> Checks to see if the player has pressed esc, if true then return to the main menu (as of now)

// LoadScene_Saves(), LoadScene_Option done s(), LoadScene_MainMenu() -> Load the corresponding menus and any options
// LoadScene_Game() -> Loads the game using save data
// LoadPlayerData() -> Called by LoadScene_Game(), it initializes PlayerPrefs
// RemoveSaveState() -> Removes a save state and repositions the remaining save states correctly

[System.Serializable]
public class MenuController : MonoBehaviour
{
    // Game State
    public enum eGameStates { MAIN_MENU, OPTIONS, SAVE_GAME, LOAD_GAME, PLAYING }

    [Header("Game State -------------------------------------------------")]
    public eGameStates gameStates;

    // Canvases
    [Header("Menu Canvases ----------------------------------------------")]
    public GameObject mainMenuCanvas;
    public GameObject playerSavesCanvas;
    public GameObject SettingsCanvas;

    // Save Data
    [Header("Save States ------------------------------------------------")]
    public GameObject SaveStatePrefab;
    public int numberOfSaves = 0;
    [SerializeField]
    List<int> numbers;

    private List<GameObject> SaveStates = new List<GameObject>();

    // Singleton (Awake)
    #region Singleton Pattern
    public static MenuController Instance { get; private set; }
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        if(mainMenuCanvas != null) mainMenuCanvas.gameObject.SetActive(true);
        if(SettingsCanvas != null) SettingsCanvas.gameObject.SetActive(false);
        if(playerSavesCanvas != null) playerSavesCanvas.gameObject.SetActive(false);

        //Gets amount of saves
        numberOfSaves = PlayerPrefs.GetInt("SaveCount", 0);

        //numbers = JsonUtility.FromJson<List<int>>(PlayerPrefs.GetString("SaveNumbers"));

        //sets blank data
        if (numbers.Count == 0)
        {
            for (int i = 0; i < numberOfSaves; i++)
            {
                numbers.Add(i);
            }
        }

        SaveAll();
    }
    #endregion

    // Update is called once per frame
    void Update()
    {
        // Check for the menu state of Options
        if (gameStates == eGameStates.OPTIONS)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                gameStates = eGameStates.MAIN_MENU;

                mainMenuCanvas.SetActive(true);
                SettingsCanvas.SetActive(false);
            }
        }

        // Check for the menu state of Saves
        if (gameStates == eGameStates.SAVE_GAME)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                gameStates = eGameStates.MAIN_MENU;

                mainMenuCanvas.SetActive(true);
                playerSavesCanvas.SetActive(false);

                for (int i = 0; i < numberOfSaves; i++)
                {
                    Destroy(SaveStates[i]);
                }
            }
        }
    }

    //Hides other object trees and shows saves tree
    public void LoadScene_Saves()
    {
        gameStates = eGameStates.SAVE_GAME;
        mainMenuCanvas.SetActive(false);
        playerSavesCanvas.SetActive(true);

        for (int i = 0; i < numberOfSaves; i++)
        {
            GameObject newSaveState;

            newSaveState = GameObject.Instantiate(SaveStatePrefab, new Vector3(playerSavesCanvas.transform.position.x, playerSavesCanvas.transform.position.y - (120 * i), 0), Quaternion.identity, playerSavesCanvas.transform);
            newSaveState.GetComponent<SaveData>().saveDataID = numbers[i];
            SaveStates[i].GetComponent<SaveData>().saveArrayPos = i;

            SaveStates.Add(newSaveState);
        }
    }

    //Hides other object trees and shows options tree
    public void LoadScene_Options()
    {
        gameStates = eGameStates.OPTIONS;
        mainMenuCanvas.SetActive(false);
        SettingsCanvas.SetActive(true);
    }

    //Loads the Main menu
    public void LoadScene_MainMenu()
    {
        LevelBuilder lvlBuilder = GameObject.FindGameObjectWithTag("LevelBuilder").GetComponent<LevelBuilder>();
        Transform t = new GameObject().transform;
        t = GameObject.FindGameObjectWithTag("Player").transform;

        lvlBuilder.saveGame.Save(lvlBuilder.saveGame.level, lvlBuilder.saveGame.seed, t, lvlBuilder.currentSave);

        SceneManager.LoadScene(1);
    }

    //Deletes a save file that the player chooses
    public void RemoveSaveState(int index)
    {
        // To do, Remove 
        Destroy(SaveStates[index]);
        SaveStates.RemoveAt(index);

        //Decreases and saves save count
        numberOfSaves--;

        numbers.Remove(numbers[index]);
        SaveAll();

        for (int i = 0; i < SaveStates.Count; i++)
        {
            SaveStates[i].transform.position = new Vector3(playerSavesCanvas.transform.position.x, playerSavesCanvas.transform.position.y - (120 * i), 0);
            SaveStates[i].GetComponent<SaveData>().saveDataID = numbers[i];
            SaveStates[i].GetComponent<SaveData>().saveArrayPos = i;
        }
    }

    //Creates a new Save file that the player sees in the save file UI
    public void AddSaveState()
    {
        //Increases and saves save count
        numberOfSaves++;

        //Create a new matching save
        SaveClass save = new SaveClass();
        save.ResetAll(numberOfSaves);
        numbers.Add(numberOfSaves);

        SaveStates.Add(GameObject.Instantiate(SaveStatePrefab, Vector3.zero, Quaternion.identity, playerSavesCanvas.transform));
    
        for (int i = 0; i < SaveStates.Count; i++)
        {
            SaveStates[i].transform.position = new Vector3(playerSavesCanvas.transform.position.x, playerSavesCanvas.transform.position.y - (120 * i), 0);
            SaveStates[i].GetComponent<SaveData>().saveDataID = numbers[i];
            SaveStates[i].GetComponent<SaveData>().saveArrayPos = i;
        }
        SaveAll();
    }

    //Saves all needed data
    private void SaveAll()
    {
        //save numbers
        string temp = JsonUtility.ToJson(numbers);
        PlayerPrefs.SetString("SaveNumbers", JsonUtility.ToJson(numbers));

        //number of saves - will be phased out
        PlayerPrefs.SetInt("SaveCount", numberOfSaves);
        PlayerPrefs.Save();
    }
}