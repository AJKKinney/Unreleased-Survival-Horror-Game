using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lamplight.Stats
{
    [System.Serializable]
    public class AttributeData
    {
        [SerializeField] private int vigor = 0;
        [SerializeField] private int might = 0;
        [SerializeField] private int endurance = 0;
        [SerializeField] private int agility = 0;

        #region Getters & Setters

        public int Vigor { get { return vigor; } set { vigor = value; } }
        public int Might { get { return might; } set { might = value; } }
        public int Endurance { get { return endurance; } set { endurance = value; } }
        public int Agility { get { return agility; } set { agility = value; } }

        #endregion
    }
}
