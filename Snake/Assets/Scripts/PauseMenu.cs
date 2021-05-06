using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{

    SnakeControls controls;
    
    public GameObject pauseMenuUI;
    public static bool GamePaused = false;

    void Awake() {
        controls = new SnakeControls();
        controls.PlayerControls.PauseGame.performed += x => {TogglePause();};
    }

    void Start() {
        pauseMenuUI.SetActive(false);
    }

    void OnEnable() {
        controls.Enable();
    }
    void OnDisable() {
        controls.Disable();
    }

    public void TogglePause() {
        if(GamePaused) { 
            Resume();
        } 
        else { 
            Pause();
        }
    }

    public void Resume() {
        pauseMenuUI.SetActive(false);
        GamePaused = false;
        Time.timeScale = 1;
    }

    public void Pause() {
        pauseMenuUI.SetActive(true);
        GamePaused = true;
        Time.timeScale = 0;
    }

    public void LoadMenu() {
        SceneManager.LoadScene(0);
    }

    public void QuitGame() {
        Application.Quit();
    }

}

