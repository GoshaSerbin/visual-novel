using System.Collections;
using System.Collections.Generic;
using Inventory.Model;
using UnityEngine;
using UnityEngine.UIElements;

public class ButtonAddItem : MonoBehaviour
{
    [SerializeField] private InventorySO _inventory;
    [SerializeField] private ItemSO _item;
    [SerializeField] private MessagesDisplay _messageDisplay;

    public void AddItem()
    {
        int reminder = _inventory.AddItem(_item, 1);
        if (reminder == 0)
        {
            Debug.Log("Added 2 items");
            _messageDisplay.ShowMessage("Новый предмет: " + _item.Name, _item.ItemImage);
        }
        else
        {
            Debug.Log($"{reminder} items can not be added because inventory is full");
        }
    }
}
