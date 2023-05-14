using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lamplight.Interaction;

public class ClimbingRopeDestination : VolumeInteractable
{
    [SerializeField] private ClimbingRopeAnchor climbingRopeAnchor;

    public override void Interact(GameObject interactor)
    {
        base.Interact(interactor);

        CharacterController controller = interactor.GetComponent<CharacterController>();

        Vector3 destination = -(controller.transform.position - climbingRopeAnchor.transform.position);

        controller.Move(destination);
    }
}
