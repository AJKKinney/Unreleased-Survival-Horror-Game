using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lamplight.Interaction
{
    public class Interactable : MonoBehaviour
    {
        public virtual void Interact(GameObject interactor)
        {
            Debug.Log(interactor.name + " interacted with " + this.name);
        }
    }
}
