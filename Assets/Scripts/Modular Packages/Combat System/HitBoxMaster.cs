using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace AustenKinney.CombatSystem
{
    public class HitBoxMaster : MonoBehaviour
    {
        public BoxCollider[] hitboxes;


        [ContextMenu("Get All Colliders")]
        private void GetHitboxes()
        {
            hitboxes = GetComponentsInChildren<BoxCollider>().ToArray();
        }
    }
}
