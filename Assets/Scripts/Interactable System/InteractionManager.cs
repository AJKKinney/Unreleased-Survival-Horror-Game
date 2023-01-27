using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AustenKinney.Essentials;
using Lamplight.Input;

namespace Lamplight.Interaction
{
    public class InteractionManager : Singleton<InteractionManager>
    {
        private List<Interactable> interactables = new List<Interactable>();
        private Interactable focus;
        private GameObject player;

        #region Getters & Setters

        public List<Interactable> Interactables { get { return interactables; } }
        public Interactable Focus { get { return focus; } set { focus = value; } }

        #endregion

        private void Start()
        {
            InputProvider.playerActions.GameActions.Interact.started += _ => Interact();
            player = GameObject.FindGameObjectWithTag("Player").transform.root.gameObject;
        }

        public void SetFocus()
        {
            if (interactables.Count > 0)
            {
                focus = interactables[interactables.Count - 1];
            }
            else
            {
                focus = null;
            }
        }

        private void Interact()
        {
            if(focus == null)
            {
                return;
            }

            focus.Interact(player);
        }
    }
}