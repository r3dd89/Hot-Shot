using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class buttonFunctions : MonoBehaviour
{
    // Resumes the game by unpausing it
    public void resume()
    {
        gameManager.instance.stateUnpause();
    }

    // Restarts the game by reloading the current scene
    public void restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        gameManager.instance.stateUnpause();
    }

    // Opens the settings menu
    public void settings()
    {
        gameManager.instance.OpenSettingsMenu();
    }

    // Goes back to the previous menu
    public void back()
    {
        gameManager.instance.back();
    }

    // Toggles the Y-axis inversion setting
    public void onToggleChange()
    {
        gameManager.instance.toggleInvertY();
    }

    // Quits the game or stops play mode in the Unity Editor
    public void quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}