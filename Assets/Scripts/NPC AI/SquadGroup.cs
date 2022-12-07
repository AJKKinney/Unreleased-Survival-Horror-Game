using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquadGroup : MonoBehaviour
{
    [SerializeField] private AIType squadtype;

    private GameObject[] squadMembers;

    // Start is called before the first frame update
    void Start()
    {

    }
}

public enum AIType
{
    VillagerLoiter,
    VillagerBusy,
    Merchant,
    Speaker,
    Ally,
    PatrolEnemy,
    SentryEnemy,
    TrackingEnemy
}
