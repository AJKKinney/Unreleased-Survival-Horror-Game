using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AustenKinney.GameState
{
    public class GameplayState : IGameState
    {
        public void EnterState(GameStateManager gameStateManager)
        {
            Debug.Log("Entered Gameplay State");
            // Initialize gameplay elements
        }

        public void UpdateState(GameStateManager gameStateManager)
        {
            // If player presses ESC, switch to Pause state
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                gameStateManager.PushState(new PauseState());
            }

            if (Input.GetKeyDown(KeyCode.G)) // Simulate Game Over
            {
                gameStateManager.ChangeState(new GameOverState());
            }
        }

        public void ExitState(GameStateManager gameStateManager)
        {
            Debug.Log("Exiting Gameplay State");
            // Cleanup gameplay elements if necessary
        }
    }
}
