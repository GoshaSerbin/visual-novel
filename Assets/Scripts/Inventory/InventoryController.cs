using System;
using System.Collections;
using System.Collections.Generic;
using Inventory.Model;
using Inventory.UI;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;


namespace Inventory
{
    public class InventoryController : MonoBehaviour
    {
        [SerializeField] private UIInventoryPage _inventoryUI;

        [SerializeField]
        private InventorySO _inventoryData;

        // public List<InventoryItem> initialItems = new List<InventoryItem>();


        void OnEnable()
        {
            _inventoryData.OnInventoryUpdated += UpdateInventoryUI;
            Narrator.OnStorySaved += SaveInventory;
            Narrator.OnStoryStartedFromSave += LoadInventory;
        }

        void OnDisable()
        {
            _inventoryData.OnInventoryUpdated -= UpdateInventoryUI;
            Narrator.OnStorySaved -= SaveInventory;
            Narrator.OnStoryStartedFromSave -= LoadInventory;
        }

        void Start()
        {
            PrepareUI();

            PrepareInventoryData();
            UpdateInventoryUI(_inventoryData.GetCurrentInventoryState());
        }

        private void PrepareInventoryData()
        {
            // _inventoryData.Initialize();

            // foreach (InventoryItem item in initialItems)
            // {
            //     if (item.IsEmpty)
            //     {
            //         continue;
            //     }

            //     _inventoryData.AddItem(item);
            // }
        }

        private void UpdateInventoryUI(Dictionary<int, InventoryItem> inventoryState)
        {
            _inventoryUI.ResetAllItems();
            foreach (var item in inventoryState)
            {
                _inventoryUI.UpdateData(item.Key, item.Value.item.ItemImage,
                    item.Value.quantity);
            }
        }

        private void PrepareUI()
        {
            _inventoryUI.InitializeInventoryUI(_inventoryData.Size);
            _inventoryUI.OnDescriptionRequested += HandleDescriptionRequest;
            _inventoryUI.OnSwapItems += HandleSwapItems;
            _inventoryUI.OnStartDragging += HandleDragging;
            _inventoryUI.OnItemActionRequested += HandleItemActionRequest;
        }


        private void HandleItemActionRequest(int itemIndex)
        {
            // InventoryItem inventoryItem = inventoryData.GetItemAt(itemIndex);
            // if (inventoryItem.IsEmpty)
            //     return;

            // IItemAction itemAction = inventoryItem.item as IItemAction;
            // if (itemAction != null)
            // {

            //     inventoryUI.ShowItemAction(itemIndex);
            //     inventoryUI.AddAction(itemAction.ActionName, () => PerformAction(itemIndex));
            // }

            // IDestroyableItem destroyableItem = inventoryItem.item as IDestroyableItem;
            // if (destroyableItem != null)
            // {
            //     inventoryUI.AddAction("Drop", () => DropItem(itemIndex, inventoryItem.quantity));
            // }

        }

        private void HandleDragging(int itemIndex)
        {
            InventoryItem inventoryItem = _inventoryData.GetItemAt(itemIndex);
            if (inventoryItem.IsEmpty)
            {
                return;
            }

            _inventoryUI.CreateDraggedItem(inventoryItem.item.ItemImage, inventoryItem.quantity);
        }

        private void HandleSwapItems(int itemIndex_1, int itemIndex_2)
        {
            _inventoryData.SwapItems(itemIndex_1, itemIndex_2);
        }

        private void HandleDescriptionRequest(int itemIndex)
        {
            InventoryItem inventoryItem = _inventoryData.GetItemAt(itemIndex);
            if (inventoryItem.IsEmpty)
            {
                _inventoryUI.ResetSelection();
                return;
            }
            ItemSO item = inventoryItem.item;
            // string description = PrepareDescription(inventoryItem);
            string description = item.Description;
            _inventoryUI.UpdateDescription(itemIndex, item.ItemImage,
                item.Name, description);
        }

        public void Show()
        {
            FindObjectOfType<AudioManager>()?.Play("ButtonClick");
            _inventoryUI.Show();
            foreach (var item in _inventoryData.GetCurrentInventoryState())
            {
                _inventoryUI.UpdateData(item.Key,
                    item.Value.item.ItemImage,
                    item.Value.quantity);
            }
        }

        public void Hide()
        {
            FindObjectOfType<AudioManager>()?.Play("ButtonClick");
            _inventoryUI.Hide();
        }




        public int AddItem(string itemName, int amount = 1) // to do: add return
        {
            ItemSO item = ItemsManager.GetItemByName(itemName);
            int reminder = _inventoryData.AddItem(item, amount);
            if (reminder == 0)
            {
                Debug.Log("Added 2 items");
            }
            else
            {
                Debug.Log($"{reminder} items can not be added because inventory is full");
            }
            return reminder;
        }

        private void DropItem(int itemIndex, int quantity)
        {
            _inventoryData.RemoveItem(itemIndex, quantity);
            _inventoryUI.ResetSelection();
        }

        public int RemoveItem(string itemName, int amount = 1)
        {
            ItemSO item = ItemsManager.GetItemByName(itemName);
            var items = _inventoryData.GetCurrentInventoryState();
            int reminder = amount;
            foreach (var (k, v) in items)
            {
                if (v.item.Name == itemName)
                {
                    reminder = Math.Max(0, reminder - v.quantity);
                    DropItem(k, amount);
                    if (reminder == 0)
                    {
                        return 0;
                    }
                }
            }
            return 1;
        }

        public int HowMany(string itemName)
        {
            var items = _inventoryData.GetCurrentInventoryState();
            int result = 0;
            foreach (var (k, v) in items)
            {
                if (v.item.Name == itemName)
                {
                    result += v.quantity;
                }
            }
            return result;
        }

        public void SaveInventory()
        {
            removeInventorySaves();
            var items = _inventoryData.GetCurrentInventoryState();
            foreach (var (k, v) in items)
            {

                PlayerPrefs.SetInt("Inventory" + v.item.Name, HowMany(v.item.Name));
            }
        }

        private void removeInventorySaves()
        {
            Dictionary<string, ItemSO> items = ItemsManager.GetAllItems();
            foreach (var (k, v) in items)
            {
                PlayerPrefs.DeleteKey("Inventory" + k);
            }
        }

        public void LoadInventory()
        {
            _inventoryData.Initialize(); // removes all items
            Dictionary<string, ItemSO> items = ItemsManager.GetAllItems();
            foreach (var (k, v) in items)
            {
                // RemoveItem(v.Name, 10000); // to do: remove all items
                int quantity = PlayerPrefs.GetInt("Inventory" + v.Name, -1);
                if (quantity > 0)
                {
                    AddItem(v.Name, quantity);
                }
            }
        }


    }

}
