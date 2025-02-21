using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using AustenKinney.Essentials;

namespace AustenKinney.GameState
{
    public class GameStateManager : Singleton<GameStateManager>
    {
        private IGameState currentState;

        private Stack<IGameState> stateStack = new Stack<IGameState>();

        public static event Action<string> OnSceneChange;

        private Dictionary<Type, List<Type>> allowedTransitions = new Dictionary<Type, List<Type>>()
        {
            { typeof(MainMenuState), new List<Type> { typeof(LoadingState), typeof(GameplayState) } },
            { typeof(LoadingState), new List<Type> { typeof(GameplayState), typeof(MainMenuState) } },
            { typeof(GameplayState), new List<Type> { typeof(PauseState), typeof(GameOverState) } },
            { typeof(PauseState), new List<Type> { typeof(GameplayState) } },
            { typeof(GameOverState), new List<Type> { typeof(LoadingState), typeof(MainMenuState) } }
        };

        private void Start()
        {
            ChangeState(new MainMenuState()); // Start in Main Menu
        }

        private void Update()
        {
            if (stateStack.Count > 0)
            {
                stateStack.Peek().UpdateState(this);
            }
        }

        public void ChangeState(IGameState newState)
        {
            if (stateStack.Count > 0)
            {
                IGameState currentState = stateStack.Peek();
                if (!allowedTransitions.ContainsKey(currentState.GetType()) ||
                    !allowedTransitions[currentState.GetType()].Contains(newState.GetType()))
                {
                    Debug.LogWarning($"Invalid transition: {currentState.GetType().Name} -> {newState.GetType().Name}");
                    return;
                }

                stateStack.Pop().ExitState(this);
            }

            stateStack.Push(newState);
            newState.EnterState(this);
        }


        public void PushState(IGameState newState)
        {
            if (stateStack.Count > 0)
            {
                stateStack.Peek().ExitState(this);
            }

            stateStack.Push(newState);
            newState.EnterState(this);
        }

        public void PopState()
        {
            if (stateStack.Count > 0)
            {
                stateStack.Pop().ExitState(this);
                if (stateStack.Count > 0)
                {
                    stateStack.Peek().EnterState(this);
                }
            }
        }

        public void LoadScene(string sceneName, IGameState nextState)
        {
            ChangeState(new LoadingState(sceneName, nextState));

            if (OnSceneChange != null)
            {
                OnSceneChange.Invoke(sceneName);
            }
        }

        public string GetCurrentStateName()
        {
            bool stateInStack = stateStack.Count > 0;

            if (stateInStack == true)
            {
                return stateStack.Peek().GetType().Name;
            }
            else
            {
                return "No Active State";
            }
        }
    }
}
