using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuController : MonoBehaviour
{
    private PlayerActions playerActions;

    [Header("Pause Menu")]
    public GameObject pausePanel;

    [Header("Debug Mode")]
    [SerializeField] bool debugMode;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        playerActions = InputManager.instance.playerActions;
    }

    // Update is called once per frame
    void Update()
    {
        if(playerActions.GameActions.Menu.WasPressedThisFrame())
        {
            ToggleMenu();
        }
    }

    private void ToggleMenu()
    {
        pausePanel.SetActive(!pausePanel.activeSelf);

        if(pausePanel.activeSelf == true)
        {
            Time.timeScale = 0;

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;


            if (debugMode == true)
            {
                Debug.Log("Game Paused");
            }
        }
        else
        {
            Time.timeScale = 1;

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;


            if (debugMode == true)
            {
                Debug.Log("Game Resumed");
            }
        }
    }
}
