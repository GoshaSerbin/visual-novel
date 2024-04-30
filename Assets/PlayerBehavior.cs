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
        TalkManager.OnItemReceived += AddItemWithoutCheck;
    }

    private void OnDisable()
    {
        TalkManager.OnItemReceived -= AddItemWithoutCheck;
    }


    public int AddItem(string name, int count)
    {
        int reminder = _inventory.AddItem(name, count);

        if (reminder == 0)
        {
            Sprite sprite = ItemsManager.GetItemByName(name).ItemImage;
            _messageDisplay.ShowMessage("Новый предмет: " + name, sprite);
        }

        return reminder;
    }

    public void AddItemWithoutCheck(string name, int count)
    {
        AddItem(name, count);
    }

    public int RemoveItem(string name, int count)
    {
        return _inventory.RemoveItem(name, count);
    }

    public int HowManyItems(string name)
    {
        return _inventory.HowMany(name);
    }
}
