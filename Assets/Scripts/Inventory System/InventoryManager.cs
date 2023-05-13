using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AustenKinney.Essentials;

public class InventoryManager : Singleton<InventoryManager>
{
    [Header("WeightManagement")]
    [Tooltip("Max Carry Weight sets the maximum item weight that the player can carry.")]
    [SerializeField] private float maxCarryWeight;

    private float carriedWeight;

    #region Getters & Setters

    public float MaxCarryWeight { get { return maxCarryWeight; } }
    public float CarriedWeight { get { return carriedWeight; } }

    #endregion

    //Stored Items
    private ItemData largePrey;
    private List<ItemData> smallPrey = new List<ItemData>();
    private List<ItemData> activeItems = new List<ItemData>();
    private List<ItemData> basicItems = new List<ItemData>();
    private List<ItemData> weapons = new List<ItemData>();

    public void AddItem(ItemData itemData)
    {
        switch(itemData.Category)
        {
            case (ItemCategory.LargePrey):
                largePrey = itemData;
                break;
            case (ItemCategory.SmallPrey):
                smallPrey.Add(itemData);
                break;
            case (ItemCategory.activeItem):
                activeItems.Add(itemData);
                break;
            case (ItemCategory.Basic):
                basicItems.Add(itemData);
                break;
            case (ItemCategory.Weapon):
                weapons.Add(itemData);
                break;
        }

        carriedWeight += itemData.ItemWeight;
    }
}
