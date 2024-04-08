using System.Collections;
using System.Collections.Generic;
using Inventory.Model;
using UnityEngine;

public class ButtonAddItem : MonoBehaviour
{
    [SerializeField] private InventorySO _inventory;
    [SerializeField] private ItemSO _item;

    public void AddItem()
    {
        int reminder = _inventory.AddItem(_item, 2);
        if (reminder == 0)
        {
            Debug.Log("Added 2 items");
        }
        else
        {
            Debug.Log($"{reminder} items can not be added because inventory is full");
        }
    }
}
