using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AustenKinney.GameState
{
    public class PauseState : IGameState
    {
        public void EnterState(GameStateManager gameStateManager)
        {
            Debug.Log("Entered Pause State");
            Time.timeScale = 0; // Freeze game time
        }

        public void UpdateState(GameStateManager gameStateManager)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                gameStateManager.PopState(); // Resume game
            }
        }

        public void ExitState(GameStateManager gameStateManager)
        {
            Debug.Log("Exiting Pause State");
            Time.timeScale = 1; // Resume game time
        }
    }
}
