using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using AustenKinney.Essentials;
using AustenKinney.GameState;

public class UIManager : Singleton<UIManager>
{
    public static event Action OnShowLoadingScreen;
    public static event Action OnHideMainMenu;
    public static event Action OnShowGameOverScreen;

    private void OnEnable()
    {
        GameStateManager.OnSceneChange += ShowLoadingScreen;
        GameStateManager.OnSceneChange += HideMainMenu;
        GameOverState.OnGameOver += ShowGameOverScreen;
    }

    private void OnDisable()
    {
        GameStateManager.OnSceneChange -= ShowLoadingScreen;
        GameStateManager.OnSceneChange -= HideMainMenu;
        GameOverState.OnGameOver += ShowGameOverScreen;
    }

    private void ShowLoadingScreen(string sceneName)
    {
        if (OnShowLoadingScreen != null)
        {
            OnShowLoadingScreen.Invoke();
        }
    }

    private void HideMainMenu(string sceneName)
    {
        if (OnHideMainMenu != null)
        {
            OnHideMainMenu.Invoke();
        }
    }

    private void ShowGameOverScreen()
    {
        if(OnShowGameOverScreen != null)
        {
            OnShowGameOverScreen.Invoke();
        }
    }

}

