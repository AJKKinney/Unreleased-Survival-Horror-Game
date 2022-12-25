using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lamplight.Stats
{
    public class Character : MonoBehaviour
    {
        [SerializeField] private string characterName;
        [SerializeField] private StatData stats;
        [SerializeField] private AttributeData attributes;

        private const int baseHealth = 100;
        private const float baseEnergy = 300;
        private const int healthPerVigor = 10;
        private const float moveSpeedPerAgility = 0.2f;
        private const float damageModPerMight = 0.05f;
        private const float energyPerEndurance = 120;

        #region Getters & Setters

        public string CharacterName { get { return characterName; } }

        #endregion

        private void Awake()
        {
            if (stats == null)
            {
                stats = new StatData();
            }

            if(attributes == null)
            {
                attributes = new AttributeData();

                CalculateCharacterStats(true);
            }
            else
            {
                CalculateCharacterStats();
            }
        }


        public void TakeDamage(int damage)
        {
            stats.Health -= damage;
        }

        public void Heal(int health)
        {
            if(stats.Health + health > stats.MaxHealth)
            {
                health = stats.MaxHealth - stats.Health;
            }

            stats.Health += health;
        }

        private void CalculateCharacterStats(bool newCharacter = false)
        {
            stats.MaxHealth = baseHealth + (attributes.Vigor * healthPerVigor);
            stats.MoveSpeedMod = attributes.Agility * moveSpeedPerAgility;
            stats.DamageMod = attributes.Might * damageModPerMight;
            stats.MaxEnergy = baseEnergy + (attributes.Endurance * energyPerEndurance);

            if(newCharacter == true)
            {
                stats.Health = stats.MaxHealth;
                stats.Energy = stats.MaxEnergy;
            }
        }
    }
} 
