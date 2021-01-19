using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    enum Screen
    {
        Main,
        Settings,
        SelectLevelMenu
        
    }

    public CanvasGroup mainScreen;
    public CanvasGroup settingsScreen;
    public CanvasGroup selectLevelMenuScreen;


    void SetCurrentScreen(Screen screen)
    {
        Utility.SetCanvasGroupEnabled(mainScreen, screen == Screen.Main);
        Utility.SetCanvasGroupEnabled(settingsScreen, screen == Screen.Settings);
        Utility.SetCanvasGroupEnabled(selectLevelMenuScreen, screen == Screen.SelectLevelMenu);
    }

    void Start()
    {
        SetCurrentScreen(Screen.Main);
    }

    public void StartNewGame()
    {
        SetCurrentScreen(Screen.SelectLevelMenu);
    }

    public void BackToMenu()
    {
        SetCurrentScreen(Screen.Main);
    }

    public void Load_Level_1()
    {
        SceneManager.LoadScene("Level_1");
    }

    public void Load_Level_2()
    {
        SceneManager.LoadScene("Level_2");
    }


public void OpenSettings()
    {
        SetCurrentScreen(Screen.Settings);
    }

    public void CloseSettings()
    {
        SetCurrentScreen(Screen.Main);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
