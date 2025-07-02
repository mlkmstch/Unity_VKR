using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class PauseMenu : MonoBehaviour
{
    public static bool PauseGame;
    public GameObject pauseGameMenu;
    public GameObject gameMenu;
    public GameObject helpMenu;
    public GameObject mainMenu;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (PauseGame)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Resume()
    {
        pauseGameMenu.SetActive(false);
        Time.timeScale = 1f;
        PauseGame = false;
        Player.Instance.EnableInput();
    }
    
    public void Pause()
    {
        Time.timeScale = 0f;
        pauseGameMenu.SetActive(true);
        PauseGame = true;
        Player.Instance.DisabeInput();
    }

    public void LoadMenu()
    {
        //pauseGameMenu.SetActive(false);
        Time.timeScale = 1f;
        mainMenu.SetActive(true);
    }
    public void GameMenuOpen()
    {
        Time.timeScale = 0f;
        gameMenu.SetActive(true);
        PauseGame = true;
        Player.Instance.DisabeInput();
    }

    public void GameMenuClose()
    {
        gameMenu.SetActive(false);
        Time.timeScale = 1f;
        PauseGame = false;
        Player.Instance.EnableInput();
    }
    public void HelpMenuOpen()
    {
        helpMenu.SetActive(true);
        Time.timeScale = 0f;
        PauseGame = true;
        Player.Instance.DisabeInput();
    }
    public void HelpMenuClose()
    {
        helpMenu.SetActive(false);
        Time.timeScale = 1f;
        PauseGame = false;
        Player.Instance.EnableInput();
    }
}
