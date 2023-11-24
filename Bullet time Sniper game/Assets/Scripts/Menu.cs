using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public GameObject startMenu;
    public GameObject pauseMenu;
    public GameObject settingsMenu;
    public GameObject gameOverMenu;
    
    public bool isPaused;
    FPS_Camera cameraScript;
    
    void Start()
    {
        startMenu = transform.Find("Start Menu").gameObject;
        pauseMenu = transform.Find("Pause Menu").gameObject;
        settingsMenu = transform.Find("Settings Menu").gameObject;
        cameraScript = GameObject.FindWithTag("Player").GetComponent<FPS_Camera>();
        isPaused = true;
        Time.timeScale = 0;
        startMenu.SetActive(true);
        pauseMenu.SetActive(false);
        settingsMenu.SetActive(false);
        gameOverMenu.SetActive(false);
        
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.P) && !isPaused)
        {
            PauseGame();
        }
    }

    public void StartGame()
    {
        startMenu.SetActive(false);

        isPaused = false;
        Time.timeScale = 1;
    }

    public void PauseGame()
    {
        pauseMenu.SetActive(true);
        startMenu.SetActive(false);
        settingsMenu.SetActive(false);
        gameOverMenu.SetActive(false);
    
        isPaused = true;
        Time.timeScale = 0;
    }

    public void ResumeGame()
    {
        pauseMenu.SetActive(false);
        startMenu.SetActive(false);
        settingsMenu.SetActive(false);
        gameOverMenu.SetActive(false);
    
        isPaused = false;
        Time.timeScale = 1;
    }

    public void SettingsMenu()
    {
        settingsMenu.SetActive(true);
        startMenu.SetActive(false);
        pauseMenu.SetActive(false);
        gameOverMenu.SetActive(false);

        isPaused = true;
    }

    public void GameOverMenu()
    {
        settingsMenu.SetActive(false);
        startMenu.SetActive(false);
        pauseMenu.SetActive(false);
        gameOverMenu.SetActive(true);

        Time.timeScale = 0;
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(0);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
