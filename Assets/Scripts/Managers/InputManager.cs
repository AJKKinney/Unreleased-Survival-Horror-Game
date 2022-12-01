using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : Singleton<InputManager>
{
    public PlayerActions playerActions;

    public override void Init()
    {
        playerActions = new PlayerActions();
        playerActions.Enable();
    }
}
