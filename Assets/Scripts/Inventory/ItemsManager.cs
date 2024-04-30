using System.Collections;
using System.Collections.Generic;
using Inventory.Model;
using UnityEngine;

public class ItemsManager //TO DO: rename
{

    [SerializeField]
    private static Dictionary<string, string> _itemName2FileName = new Dictionary<string, string>(){
            {"таблетки", "Tablets"},
            {"деньги", "Money"},
            {"кофе", "Coffee"},
            {"жетон", "Token"}
        };


    [SerializeField] static private Dictionary<string, ItemSO> _items;

    static private void Init()
    {
        _items = new();
        ItemSO[] items = Resources.LoadAll<ItemSO>("Items/");

        foreach (var item in items)
        {
            _items[item.Name] = item;
        }
    }


    static public ItemSO GetItemByName(string itemName)
    {
        if (_items is null)
        {
            Debug.Log("items is null");
            Init();
        }
        Debug.Log("getting item:" + itemName);
        return _items[itemName];
    }
}
