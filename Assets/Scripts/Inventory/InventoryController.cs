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

        public List<InventoryItem> initialItems = new List<InventoryItem>();


        private void OnEnable()
        {
            TalkManager.OnItemReceived += AddItem;
            // Narrator.OnItemReceived += AddItem;
        }

        private void OnDisable()
        {
            TalkManager.OnItemReceived -= AddItem;
        }

        void Start()
        {
            PrepareUI();

            PrepareInventoryData();
        }

        private void PrepareInventoryData()
        {
            _inventoryData.Initialize();
            _inventoryData.OnInventoryUpdated += UpdateInventoryUI;
            foreach (InventoryItem item in initialItems)
            {
                if (item.IsEmpty)
                {
                    continue;
                }

                _inventoryData.AddItem(item);
            }
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
                item.name, description);
        }

        public void Show()
        {
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
            _inventoryUI.Hide();
        }

        [SerializeField] private ItemSO _item;
        [SerializeField] private MessagesDisplay _messageDisplay;

        private ItemSO GetItemByName(string itemName)
        {
            Debug.Log("getting item:" + itemName);
            return Resources.Load<ItemSO>("Items/" + itemName + ".asset");
        }

        [SerializeField]
        private Dictionary<string, string> _itemName2FileName = new Dictionary<string, string>(){
            {"таблетки", "Tablets"},
            {"деньги", "Money"},
            {"кофе", "Coffee"},
        };

        public void AddItem(string itemName, int amount = 1) // to do: add return
        {
            itemName = _itemName2FileName[itemName];
            ItemSO item = GetItemByName(itemName); // to do: _itemsManager.GetItemByName(itemName);
            int reminder = _inventoryData.AddItem(item, amount);
            if (reminder == 0)
            {
                Debug.Log("Added 2 items");
                _messageDisplay.ShowMessage("Новый предмет: " + item.Name, item.ItemImage);
            }
            else
            {
                Debug.Log($"{reminder} items can not be added because inventory is full");
            }
            // return reminder;
        }

        private void DropItem(int itemIndex, int quantity)
        {
            _inventoryData.RemoveItem(itemIndex, quantity);
            _inventoryUI.ResetSelection();
        }

        public int RemoveItem(string itemName, int amount = 1)
        {
            ItemSO item = GetItemByName(itemName); // to do: _itemsManager.GetItemByName(itemName);
            var items = _inventoryData.GetCurrentInventoryState();
            foreach (var (k, v) in items)
            {
                if (v.item.Name == itemName)
                {
                    DropItem(k, amount);
                    return 0;
                }
            }
            return 1;
        }


    }

}
