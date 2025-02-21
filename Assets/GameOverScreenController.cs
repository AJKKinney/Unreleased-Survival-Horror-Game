using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AustenKinney.GameState;

public class GameOverScreenController : MonoBehaviour
{
    [SerializeField] private GameObject gameOverPanel;
    
    // Start is called before the first frame update
    void Start()
    {
        GameOverState.OnGameOver += ShowGameOverScreen;
    }

    // Update is called once per frame
    void OnDestroy()
    {
        GameOverState.OnGameOver -= ShowGameOverScreen;
    }

    private void ShowGameOverScreen()
    {
        gameOverPanel.SetActive(true);
    }
}
