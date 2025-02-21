using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace AustenKinney.GameState
{
    public class MainMenuState : IGameState
    {
        public void EnterState(GameStateManager gameStateManager)
        {
            Debug.Log("Entered Main Menu State");

            //Load Main Menu Scene
            if (SceneManager.GetActiveScene() != SceneManager.GetSceneByName("MainMenu") )
            {
                SceneManager.LoadScene("MainMenu");
            }
        }

        public void UpdateState(GameStateManager gameStateManager)
        {

        }

        public void ExitState(GameStateManager gameStateManager)
        {
            Debug.Log("Exiting Main Menu State");
        }
    }
}
