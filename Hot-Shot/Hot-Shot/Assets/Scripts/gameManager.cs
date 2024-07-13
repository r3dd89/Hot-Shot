using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI; //Add using UnityEngine.UI

public class gameManager : MonoBehaviour
{
    public static gameManager instance;

    [SerializeField] GameObject menuActive;
    [SerializeField] GameObject menuPause;
    [SerializeField] GameObject menuWin;
    [SerializeField] GameObject menuLose;
    [SerializeField] GameObject menuSettings;
    [SerializeField] private TMP_Dropdown dropdown;

   
    public GameObject player;
    public PlayerMovement playerScript;
    public PlayerVision playerVisionScript;
    public GameObject prevMenu;
    public Image playerHPBar; //Variable for player HP bar
    public GameObject damageFlashScreen; //Variable for damage flash

    public bool isPaused;
    public bool invertY;

    int enemyCount;

    // Start is called before the first frame update
    //order of openning, awake > start > other
    //awake for managers
    void Awake()
    {
        instance = this;
        player = GameObject.FindWithTag("Player");
        playerScript = player.GetComponent<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Cancel"))
            if (menuActive == null)
            {
                statePause();
                menuActive = menuPause;
                menuActive.SetActive(isPaused);
            }
            else if (menuActive == menuPause)
                stateUnpause();

    }
    public void statePause()
    {

        isPaused = !isPaused;
        Time.timeScale = 0;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;

    }

    public void stateUnpause()
    {

        isPaused = false;
        Time.timeScale = 1;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        if (menuActive != null)
        {
            menuActive.SetActive(false);
            menuActive = null;
        }
    }

    public void updateGameGoal(int amount)
    {
        enemyCount += amount;
        if (enemyCount <= 0)
        {
            statePause();
            menuActive = menuWin;
            menuActive.SetActive(isPaused);
        }
    }

    public void WinGame()
    {
        menuWin.SetActive(true);

        // Pauses the game
        //https://gamedevbeginner.com/the-right-way-to-pause-the-game-in-unity/
        Time.timeScale = 0;
    }

    public void LoseGame()
    {
        statePause();
        menuActive = menuLose;
        menuActive.SetActive(isPaused);
    }

    public void RestartGame()
    {
        Time.timeScale = 0;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void OpenSettingsMenu()
    {
        if(menuActive != null)
    {
            prevMenu = menuActive;
            menuActive.SetActive(false);
        }

        menuSettings.SetActive(true);
        menuActive = menuSettings;
        statePause();
    }
    public void back()
    {
        Debug.Log("Back pressed");
        if (menuActive != null)
        {
            Debug.Log("Hiding current active menu");
            menuActive.SetActive(false);
        }

        if (prevMenu != null)
        {
            Debug.Log("showing previous menu");
            prevMenu.SetActive(true);
            menuActive = prevMenu;
            prevMenu = null;
        }

        statePause();
    }
    public void toggleInvertY()
    {
        invertY = !invertY;
        if (playerVisionScript != null) 
            playerVisionScript.invertY = invertY;
    }
}