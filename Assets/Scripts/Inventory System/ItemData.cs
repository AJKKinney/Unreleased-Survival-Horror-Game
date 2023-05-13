using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "New Item", menuName = "Create Item", order = 31)]
public class ItemData : ScriptableObject
{
    [SerializeField] private string itemName;
    [SerializeField] private float itemWeight;
    [SerializeField] private ItemCategory category;

    #region Getters & Setters

    public string ItemName { get { return itemName; }}
    public float ItemWeight { get { return itemWeight; }}
    public ItemCategory Category { get { return category; } }

    #endregion
}
