using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lamplight.Interaction
{
    [RequireComponent(typeof(BoxCollider))]
    public class VolumeInteractable : Interactable
    {
        InteractionManager manager;

        private void Start()
        {
            manager = InteractionManager.instance;
        }

        private void OnTriggerEnter(Collider other)
        {
            if(other.CompareTag("Player"))
            {
                manager.Interactables.Add(this);
                manager.SetFocus();
                Debug.Log("Registered Interactable " + gameObject.name + ".");
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                manager.Interactables.Remove(this);
                manager.SetFocus();
                Debug.Log("Removed Interactable " + gameObject.name + " from Interaction Manager.");
            }
        }
    }
}
