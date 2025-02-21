using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using AustenKinney.GameState;

public class StartGameButtonController : MonoBehaviour
{
    private Button button;
    private GameStateManager gameStateManager;

    // Start is called before the first frame update
    void Start()
    {
        button = GetComponent<Button>();
        gameStateManager = GameStateManager.Instance;

        button.onClick.AddListener(StartGame);
    }

    private void StartGame()
    {
        gameStateManager.LoadScene("TestArena", new GameplayState());
        //gameStateManager.ChangeState(new GameplayState());
    }

}
