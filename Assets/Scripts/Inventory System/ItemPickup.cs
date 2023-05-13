using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lamplight.Interaction;

public class ItemPickup : VolumeInteractable
{
    [SerializeField] private ItemData itemData;

    private InventoryManager inventoryManager;

    protected void Start()
    {
        base.Start();
        inventoryManager = InventoryManager.instance;
    }

    override public void Interact(GameObject interactor)
    {
        inventoryManager.AddItem(itemData);
        Destroy(this.gameObject);
    }
}
