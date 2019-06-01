using System.Collections;
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

// LoadScene_Saves(), LoadScene_Options(), LoadScene_MainMenu() -> Load the corresponding menus and any options
// LoadScene_Game() -> Loads the game using save data
// LoadPlayerData() -> Called by LoadScene_Game(), it initializes PlayerPrefs
// RemoveSaveState() -> Removes a save state and repositions the remaining save states correctly

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
    public int numberOfSaves = 5;

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

        mainMenuCanvas.gameObject.SetActive(true);
        SettingsCanvas.gameObject.SetActive(false);
        playerSavesCanvas.gameObject.SetActive(false);
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
            }
        }
    }

    public void LoadScene_Saves()
    {
        gameStates = eGameStates.SAVE_GAME;
        mainMenuCanvas.SetActive(false);
        playerSavesCanvas.SetActive(true);

        for (int i = 0; i < numberOfSaves; i++)
        {
            GameObject newSaveState;

            newSaveState = GameObject.Instantiate(SaveStatePrefab, new Vector3(playerSavesCanvas.transform.position.x, (120 * i) + 200, 0), Quaternion.identity, playerSavesCanvas.transform);
            newSaveState.GetComponent<SaveData>().saveSeed = i;
            newSaveState.GetComponent<SaveData>().saveDataID = i;

            SaveStates.Add(newSaveState);
        }
    }
    public void LoadScene_Options()
    {
        gameStates = eGameStates.OPTIONS;
        mainMenuCanvas.SetActive(false);
        SettingsCanvas.SetActive(true);
    }

    // Loads the Main menu
    public void LoadScene_MainMenu()
    {
        SceneManager.LoadScene(1);
    }

    // Loads the Game with save data
    public void LoadScene_Game(int saveNum)
    {
        LoadPlayerData(saveNum);
        SceneManager.LoadScene(0);
    }

    private void LoadPlayerData(int saveNum)
    {
        PlayerPrefs.SetInt("CurrentSave", saveNum);
        PlayerPrefs.Save();
    }

    public void RemoveSaveState(int index)
    {
        // To do, Remove 
    }
}
