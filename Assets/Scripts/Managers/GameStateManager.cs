using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Contains the game state information of the game.
/// </summary>
public class GameStateManager : Singleton<GameStateManager>
{
    public GameState currentGameState;

    private PlayerActions playerActions;


    private void Start()
    {
        playerActions = InputManager.instance.playerActions;
        SetState(GameState.Gameplay);
    }


    private void Update()
    {
        if(currentGameState == GameState.Gameplay)
        {
            if(playerActions.GameActions.Menu.WasPressedThisFrame())
            {
                SetState(GameState.Paused);
            }
        }
        else if(currentGameState == GameState.Paused)
        {
            if (playerActions.GameActions.Menu.WasPressedThisFrame())
            {
                SetState(GameState.Gameplay);
            }
        }
    }


    public void SetState(GameState state)
    {
        if (state == GameState.Gameplay)
        {
            Time.timeScale = 1;

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            PauseMenuManager.instance.CloseAllPausePanels();

            Debug.Log("Game Started");
        }
        else if (state == GameState.Paused)
        {
            Time.timeScale = 0;

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            PauseMenuManager.instance.OpenPausePanel();

            Debug.Log("Game Paused");
        }
        else if (state == GameState.Title)
        {
            Debug.Log("Open Title Screen");
        }
        else if (state == GameState.Cutscene)
        {
            Debug.Log("Custcene Started");
        }
        else if(state == GameState.Quit)
        {
            Debug.Log("Quitting Game");

            Application.Quit();

            #if (UNITY_EDITOR)
                UnityEditor.EditorApplication.isPlaying = false;
            #endif
        }
        else
        {
            Debug.LogWarning("Game State has not been fully implemented yet: " + state.ToString());
        }

        currentGameState = state;
    }

    public void UnpauseGame()
    {
        SetState(GameState.Gameplay);
    }

    public void QuitGame()
    {
        SetState(GameState.Quit);
    }
}

public enum GameState
{
    Gameplay,
    Paused,
    Title,
    Cutscene,
    Quit
}
