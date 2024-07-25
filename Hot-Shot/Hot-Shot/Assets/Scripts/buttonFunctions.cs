using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class buttonFunctions : MonoBehaviour
{
    public Slider mainSlider;
    // Resumes the game by unpausing it
    public void resume()
    {
        Debug.Log("Resume");
        gameManager.instance.stateUnpause();
    }

    // Restarts the game by reloading the current scene
    public void restart()
    {
        Debug.Log("Restart");
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        gameManager.instance.stateUnpause();
    }

    // Opens the settings menu
    public void settings()
    {
        Debug.Log("Settings");
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

    public void OnSliderValueChanged()
    {
        if (mainSlider != null)
        {
            float sliderVal = mainSlider.value;
            gameManager.instance.UpdateSliderValue(sliderVal);
        }
        else
        {
            Debug.LogWarning("mainSlider is not assigned.");
        }
    }
    // Quits the game or stops play mode in the Unity Editor
    public void quit()
    {
        Debug.Log("Quit");
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}