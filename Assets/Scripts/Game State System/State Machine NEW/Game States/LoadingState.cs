using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace AustenKinney.GameState
{
    public class LoadingState : IGameState
    {
        private string sceneToLoad;
        private IGameState nextState;

        public static event Action<float> OnLoadingProgress;

        public LoadingState(string scene, IGameState next)
        {
            sceneToLoad = scene;
            nextState = next;
        }

        public void EnterState(GameStateManager gameStateManager)
        {
            Debug.Log($"Loading Scene: {sceneToLoad}");

            //gameStateManager.ChangeScene(sceneToLoad);

            gameStateManager.StartCoroutine(LoadSceneAsync(gameStateManager, sceneToLoad));
        }

        private IEnumerator LoadSceneAsync(GameStateManager gameStateManager, string sceneName)
        {
            AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneName);
            asyncOperation.allowSceneActivation = false;

            // Update progress while loading
            while (!asyncOperation.isDone)
            {
                float progress = Mathf.Clamp01(asyncOperation.progress / 0.9f);
                OnLoadingProgress?.Invoke(progress);  // Trigger event to update slider

                if (asyncOperation.progress >= 0.9f)
                {
                    // Once loading is complete, allow scene activation
                    asyncOperation.allowSceneActivation = true;
                }

                yield return null;
            }


            gameStateManager.ChangeState(nextState);
        }

        public void UpdateState(GameStateManager gameStateManager) { }
        public void ExitState(GameStateManager gameStateManager) { }
    }
}
