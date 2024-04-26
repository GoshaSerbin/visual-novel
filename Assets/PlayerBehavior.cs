using System.Collections;
using System.Collections.Generic;
using Inventory;
using UnityEngine;

[RequireComponent(typeof(InventoryController))]
public class PlayerBehavior : MonoBehaviour
{

    private InventoryController _inventory;
    [SerializeField] private MessagesDisplay _messageDisplay;

    private void Awake()
    {
        _inventory = gameObject.GetComponent<InventoryController>();
    }
    private void OnEnable()
    {
        Narrator.OnItemReceived += ReceiveItem;
        TalkManager.OnItemReceived += ReceiveItem;
        Narrator.OnItemRemoved += RemoveItem;
    }

    private void OnDisable()
    {
        Narrator.OnItemReceived -= ReceiveItem;
        TalkManager.OnItemReceived -= ReceiveItem;
        Narrator.OnItemRemoved -= RemoveItem;
    }


    private void ReceiveItem(string name, int count)
    {
        int reminder = _inventory.AddItem(name, count);

        if (reminder == 0)
        {
            Sprite sprite = ItemsManager.GetItemByName(name).ItemImage;
            _messageDisplay.ShowMessage("Новый предмет: " + name, sprite);
        }
    }

    private void RemoveItem(string name, int count)
    {
        _inventory.RemoveItem(name, count);
    }
}
