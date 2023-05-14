using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lamplight.Interaction;

public class ClimbingRopeAnchor : VolumeInteractable
{
    [SerializeField] private ClimbingRopeDestination climbingRopeDestination;

    public override void Interact(GameObject interactor)
    {
        base.Interact(interactor);

        if(climbingRopeDestination.gameObject.activeSelf == true)
        {

            CharacterController controller = interactor.GetComponent<CharacterController>();

            Vector3 destination = -(controller.transform.position - climbingRopeDestination.transform.position);

            controller.Move(destination);
        }
        else
        {
            climbingRopeDestination.gameObject.SetActive(true);
        }
    }

}
