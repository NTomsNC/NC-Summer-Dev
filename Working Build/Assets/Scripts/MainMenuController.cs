using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    [Header("Menu States")]
    public GameObject mainMenuCanvas;
    public GameObject playerSavesCanvas;
    public GameObject SettingsCanvas;

    public GameObject SaveStatePrefab;
    public List<GameObject> SaveStates = new List<GameObject>();
    public int numberOfSaves = 5;

    public enum MenuState { Main, Saves, Options };
    public MenuState menuState;

    // Singleton (Awake)
    #region Singleton Pattern
    public static MainMenuController Instance { get; private set; }
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

        mainMenuCanvas.SetActive(true);
        SettingsCanvas.SetActive(false);
        playerSavesCanvas.SetActive(false);
    }
    #endregion

    // Update is called once per frame
    void Update()
    {
        // Check for the menu state of Options
        if (menuState == MenuState.Options)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                menuState = MenuState.Main;

                mainMenuCanvas.SetActive(true);
                SettingsCanvas.SetActive(false);
            }
        }

        // Check for the menu state of Saves
        if (menuState == MenuState.Saves)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                menuState = MenuState.Main;

                mainMenuCanvas.SetActive(true);
                playerSavesCanvas.SetActive(false);
            }
        }
    }

    public void HandleOptionsBtn()
    {
        menuState = MenuState.Options;
        mainMenuCanvas.SetActive(false);
        SettingsCanvas.SetActive(true);
    }

    public void HandleSavesBtn()
    {
        menuState = MenuState.Saves;
        mainMenuCanvas.SetActive(false);
        playerSavesCanvas.SetActive(true);

        for (int i = 0; i < numberOfSaves; i++)
        {
            GameObject newSaveState;

            newSaveState = GameObject.Instantiate(SaveStatePrefab, new Vector3(playerSavesCanvas.transform.position.x, (120 * i) + 200, 0), Quaternion.identity, playerSavesCanvas.transform);
            newSaveState.GetComponent<SaveData>().saveSeed = i;
            newSaveState.GetComponent < SaveData>().saveDataID = i;

            SaveStates.Add(newSaveState);
        }
    }

    public void LoadGame(int saveNum)
    {
        //Used to load proper saves in levelBuilder
        PlayerPrefs.SetInt("CurrentSave", saveNum);
        PlayerPrefs.Save();

        SceneManager.LoadScene(0);
    }

    public void DeleteSave(int saveNum)
    {
        PlayerPrefs.DeleteKey("Save" + saveNum);
    }

    public void RemoveSaveState(int index)
    {
        // To do, Remove 
    }
}
