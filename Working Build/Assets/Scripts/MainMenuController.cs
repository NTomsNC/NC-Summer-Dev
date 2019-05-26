using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuController : MonoBehaviour
{
    [Header("Menu States")]
    public GameObject mainMenuCanvas;
    public GameObject playerSavesCanvas;
    public GameObject SettingsCanvas;

    public enum MenuState { Main, Saves, Options};
    public MenuState menuState;

    public GameObject levelController;

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
        if (menuState == MenuState.Options) {
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
    }

    public void LoadGame()
    {
        GameObject go = GameObject.Instantiate(levelController);
        playerSavesCanvas.SetActive(false);
    }
}
