using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquadGroup : MonoBehaviour
{
    [SerializeField] private AIType squadtype;

    private GameObject[] squadMembers;
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
