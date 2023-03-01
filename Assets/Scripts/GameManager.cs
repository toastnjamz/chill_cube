using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;

// sets the script to be executed later than all default scripts
// this is helpful for UI, since other things may need to be initialized before setting the UI
[DefaultExecutionOrder(1000)]
public class GameManager : MonoBehaviour
{
    public GameObject pauseMenuUI;
    public GameObject pauseButton;
    public static bool GameIsPaused = false;
    public GameObject completeLevelUI;

    // TODO: might not need
    public float restartDelay = 1f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayGame()
    {
        SceneManager.LoadScene(1);
        // TODO: change to the player's last played scene (loaded from save file)
        //SceneManager.GetActiveScene();
    }

    public void PauseGame()
    {
        pauseMenuUI.SetActive(true);
        pauseButton.SetActive(false);
        Time.timeScale = 0f;
        GameIsPaused = true;
    }

    public void ResumeGame()
    {
        pauseMenuUI.SetActive(false);
        pauseButton.SetActive(true);
        Time.timeScale = 1f;
        GameIsPaused = false;
    }

    public void LoadMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }

    // TODO: change to shuffle cube back to starting point?
    public void Restart()
    {
        ///Invoke("Restart", restartDelay);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    
    public void QuitGame()
    {
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
        Application.Quit();
#endif
    }

    public void CompleteLevel()
    {
        completeLevelUI.SetActive(true);
    }
}
