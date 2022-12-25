using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lamplight.Stats
{
    [System.Serializable]
    public class StatData
    {
        [SerializeField] private int health;
        [SerializeField] private int maxHealth;
        [SerializeField] private float moveSpeedMod;
        [SerializeField] private float damageMod;
        [SerializeField] private float energy;
        [SerializeField] private float maxEnergy;


        #region Getters & Setters

        public int Health { get { return health; } set { health = value; } }
        public int MaxHealth { get { return maxHealth; } set { maxHealth = value; } }
        public float MoveSpeedMod { get { return moveSpeedMod; } set { moveSpeedMod = value; } }
        public float DamageMod { get { return damageMod; } set { damageMod = value; } }
        public float Energy { get { return energy; } set { energy = value; } }
        public float MaxEnergy { get { return maxEnergy; } set { maxEnergy = value; } }

        #endregion
    }
}
