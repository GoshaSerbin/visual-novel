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


    public static ItemSO GetItemByName(string itemName)
    {
        itemName = _itemName2FileName[itemName];
        Debug.Log("getting item:" + itemName);
        return Resources.Load<ItemSO>("Items/" + itemName);
    }
}
