using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI; //Add using UnityEngine.UI

public class gameManager : MonoBehaviour
{
    public static gameManager instance; // Singleton instance

    [SerializeField] GameObject menuActive; // The currently active menu
    [SerializeField] GameObject menuPause; // The pause menu
    [SerializeField] GameObject menuWin; // The win menu
    [SerializeField] GameObject menuLose; // The lose menu
    [SerializeField] GameObject menuSettings; // The settings menu

    public GameObject player; // The player GameObject
    public PlayerMovement playerScript; // Reference to the PlayerMovement script
    public PlayerVision playerVisionScript; // Reference to the PlayerVision script
    public GameObject prevMenu; // The previous menu before the current one
    public GameObject damageFlashScreen; // The screen that flashes when the player takes damage

    public GameObject healthLowAlert; // The screen that flashes when the players health is low
    private bool healthLowAlertShown;
    [SerializeField] Material healthLowMaterial;
    [SerializeField] float fadeDuration = 1.0f;

    private Coroutine healthLowCoroutine;

    public bool isPaused; // Whether the game is currently paused
    public bool invertY; // Whether the Y-axis is inverted

    int enemyCount; // The number of enemies in the game

    // Awake is called before Start and is used to initialize any variables or game state before the game starts
    void Awake()
    {
        // Set the singleton instance to this object
        instance = this;

        // Find the player GameObject and get its PlayerMovement script
        player = GameObject.FindWithTag("Player");
        playerScript = player.GetComponent<PlayerMovement>();
        //playerVisionScript = player.GetComponent<PlayerVision>(); // Initialize playerVisionScript

        // Assign the damage flash image to the player health script
        PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
        if (playerHealth != null)
        {
            playerHealth.damageFlashImage = damageFlashScreen;
        }

        // Ensure the shader material is initially transparent
        SetMaterialAlpha(0f);
    }

    // Update is called once per frame
    void Update()
    {
        // Check if the Cancel button (usually Esc) is pressed
        if (Input.GetButtonDown("Cancel"))
        {
            // If no menu is active, pause the game and show the pause menu
            if (menuActive == null)
            {
                statePause();
                menuActive = menuPause;
                menuActive.SetActive(isPaused);
            }
            // If the pause menu is active, unpause the game
            else if (menuActive == menuPause)
            {
                stateUnpause();
            }
        }
    }


    // Pauses the game
    public void statePause()
    {
        isPaused = !isPaused; // Toggle the paused state
        Time.timeScale = 0; // Stop the game time
        Cursor.visible = true; // Show the cursor
        Cursor.lockState = CursorLockMode.Confined; // Confine the cursor to the game window
    }

    // Unpauses the game
    public void stateUnpause()
    {
        isPaused = false; // Set paused state to false
        Time.timeScale = 1; // Resume the game time
        Cursor.visible = false; // Hide the cursor
        Cursor.lockState = CursorLockMode.Locked; // Lock the cursor to the center of the screen

        if (menuActive != null)
        {
            menuActive.SetActive(false); // Hide the active menu
            menuActive = null; // Clear the active menu reference
        }
    }

    // Updates the game goal by adjusting the enemy count
    public void updateGameGoal(int amount)
    {
        enemyCount += amount; // Adjust the enemy count

        // If all enemies are defeated, show the win menu
        if (enemyCount <= 0)
        {
            statePause();
            menuActive = menuWin;
            menuActive.SetActive(isPaused);
        }
    }

    // Shows the win game menu and pauses the game
    public void WinGame()
    {
        menuWin.SetActive(true);
        Time.timeScale = 0; // Pause the game time
    }

    // Shows the lose game menu and pauses the game
    public void LoseGame()
    {
        statePause();
        menuActive = menuLose;
        menuActive.SetActive(isPaused);
    }

    // Restarts the game by reloading the current scene
    public void RestartGame()
    {
        Time.timeScale = 0; // Pause the game time
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // Reload the current scene
    }

    // Opens the settings menu
    public void OpenSettingsMenu()
    {
        if (menuActive != null)
        {
            prevMenu = menuActive; // Save the current active menu
            menuActive.SetActive(false); // Hide the current active menu
        }

        menuSettings.SetActive(true); // Show the settings menu
        menuActive = menuSettings; // Set the settings menu as the active menu
        statePause(); // Pause the game
    }

    // Goes back to the previous menu
    public void back()
    {
        Debug.Log("Back pressed");
        if (menuActive != null)
        {
            Debug.Log("Hiding current active menu");
            menuActive.SetActive(false); // Hide the current active menu
        }

        if (prevMenu != null)
        {
            Debug.Log("showing previous menu");
            prevMenu.SetActive(true); // Show the previous menu
            menuActive = prevMenu; // Set the previous menu as the active menu
            prevMenu = null; // Clear the previous menu reference
        }

        statePause(); // Pause the game
    }

    // Toggles the Y-axis inversion
    public void toggleInvertY()
    {
        invertY = !invertY; // Toggle the invertY variable
        if (playerVisionScript != null)
            playerVisionScript.invertY = invertY; // Update the PlayerVision script
    }
    public void UpdateSliderValue(float sens)
    {
        int sensitivity = Mathf.RoundToInt(sens);
        if (playerVisionScript != null)
            playerVisionScript.SetSensitivity(sensitivity);
    }
    // Handles the health low alert
    public void HandleLowHealthAlert(bool isLowHealth)
    {
        if (isLowHealth)
        {
            if (healthLowCoroutine == null)
            {
                healthLowCoroutine = StartCoroutine(FadeHealthLowAlert());
            }
        }
        else
        {
            if (healthLowCoroutine != null)
            {
                StopCoroutine(healthLowCoroutine);
                healthLowCoroutine = null;
                SetMaterialAlpha(0f);
            }
        }
    }

    IEnumerator FadeHealthLowAlert()
    {
        while (true)
        {
            yield return StartCoroutine(FadeTo(1f, fadeDuration));
            yield return StartCoroutine(FadeTo(0f, fadeDuration));
        }
    }

    IEnumerator FadeTo(float targetAlpha, float duration)
    {
        float startAlpha = healthLowMaterial.GetFloat("_Alpha");
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(startAlpha, targetAlpha, elapsedTime / duration);
            SetMaterialAlpha(alpha);
            yield return null;
        }

        SetMaterialAlpha(targetAlpha);
    }

    void SetMaterialAlpha(float alpha)
    {
        healthLowMaterial.SetFloat("_Alpha", alpha);
    }
}