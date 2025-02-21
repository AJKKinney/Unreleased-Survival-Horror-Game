using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

namespace AustenKinney.GameState
{
    public class GameOverState : IGameState
    {
        public static event Action OnGameOver;

        public void EnterState(GameStateManager gameStateManager)
        {
            Debug.Log("Entered Game Over State");

            if(OnGameOver != null)
            {
                OnGameOver.Invoke();
            }

            // Show game over screen
        }

        public void UpdateState(GameStateManager gameStateManager)
        {
            // Example: Restart game if player presses "R"
            if (Input.GetKeyDown(KeyCode.R))
            {
                gameStateManager.LoadScene("MainMenu", new MainMenuState());
            }
        }

        public void ExitState(GameStateManager gameStateManager)
        {
            Debug.Log("Exiting Game Over State");
            // Hide game over UI
        }
    }
}
