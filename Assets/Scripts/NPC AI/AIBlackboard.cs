using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(fileName = "AIBlackboard", menuName = "AI/Blackboard")]
public class AIBlackboard:ScriptableObject
{
    private Transform player;

    #region Getters & Setters

    public Transform Player 
    {
        get
        {
            if(player == null)
            {
                player = Player = GameObject.FindGameObjectWithTag("Player").transform;
            }

            return player;
        }

        set => player = value; 
    }

    #endregion
}
