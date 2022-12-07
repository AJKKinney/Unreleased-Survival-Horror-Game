using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class HitBoxMaster : MonoBehaviour
{
    public BoxCollider[] hitboxes;


    [ContextMenu("Get All Hitboxes")]
    private void GetHitboxes()
    {
        hitboxes = GetComponentsInChildren<BoxCollider>().ToArray();
    }
}
