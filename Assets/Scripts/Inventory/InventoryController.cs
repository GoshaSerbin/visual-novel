using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryController : MonoBehaviour
{
    [SerializeField] private UIInventoryPage _inventoryUI;

    int inventorySize = 15;
    // Start is called before the first frame update
    void Start()
    {
        _inventoryUI.InitializeInventoryUI(inventorySize);
    }


    public void ShowOrHide()
    {
        if (_inventoryUI.isActiveAndEnabled == false)
        {
            _inventoryUI.Show();
        }
        else
        {
            _inventoryUI.Hide();
        }
    }

}
