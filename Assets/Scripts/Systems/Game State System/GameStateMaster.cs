using UnityEngine;
using AustenKinney.Essentials;


namespace AustenKinney.GameState
{
    /// <summary>
    /// Contains the game state information of the game.
    /// </summary>
    public static class GameStateMaster
    {
        public static GameState currentGameState;

        public delegate void SetGameStateGameplay();
        public static event SetGameStateGameplay OnSetGameStateGameplay = delegate { };

        public delegate void SetGameStatePaused();
        public static event SetGameStatePaused OnSetGameStatePaused = delegate { };

        public delegate void SetGameStateTitle();
        public static event SetGameStateTitle OnSetGameStateTitle = delegate { };

        public delegate void SetGameStateCutscene();
        public static event SetGameStateCutscene OnSetGameStateCutscene = delegate { };

        public delegate void SetGameStateQuit();
        public static event SetGameStateQuit OnSetGameStateQuit = delegate { };

        static GameStateMaster()
        {
            SetState(GameState.Gameplay);
        }

        public static void SetState(GameState state)
        {
            if (state == GameState.Gameplay)
            {
                Time.timeScale = 1;

                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;

                OnSetGameStateGameplay();

                Debug.Log("Game Started");
            }
            else if (state == GameState.Paused)
            {
                Time.timeScale = 0;

                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;

                OnSetGameStatePaused();

                Debug.Log("Game Paused");
            }
            else if (state == GameState.Title)
            {
                OnSetGameStateTitle();

                Debug.Log("Title Screen");
            }
            else if (state == GameState.Cutscene)
            {
                OnSetGameStateCutscene();

                Debug.Log("Custcene");
            }
            else if (state == GameState.Quit)
            {
                OnSetGameStateQuit();

                Debug.Log("Quitting Game");

                Application.Quit();

#if (UNITY_EDITOR)

                UnityEditor.EditorApplication.isPlaying = false;
#endif
            }
            else
            {
                Debug.LogWarning("Game State has not been implemented yet: " + state.ToString());
            }

            currentGameState = state;
        }
    }
}
