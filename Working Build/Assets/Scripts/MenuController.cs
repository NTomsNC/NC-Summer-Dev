using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    //[Header("Menu States")]
    //public GameObject mainMenuCanvas;
    //public GameObject playerSavesCanvas;
    //public GameObject SettingsCanvas;

    public enum MenuState { Main, Saves, Options };
    public MenuState menuState;

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadScene_MainMenu()
    {
        SceneManager.LoadScene(1);
    }

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
}
