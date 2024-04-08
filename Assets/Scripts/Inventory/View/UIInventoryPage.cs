using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class UIInventoryPage : MonoBehaviour
{

    [SerializeField] private UIInventoryItem _itemPrefab;

    [SerializeField]
    private RectTransform _contentPanel;

    [SerializeField] private UIInventoryDescription _itemDescription;

    [SerializeField] private MouseFollower _mouseFollower;

    List<UIInventoryItem> _uiInventoryItems = new List<UIInventoryItem>();

    public event Action<int> OnDescriptionRequested,
    OnItemActionRequested,
     OnStartDragging;

    public event Action<int, int> OnSwapItems;

    private int currentlyDraggedItemIndex = -1;

    private void Awake()
    {
        Hide();
        _mouseFollower.Toggle(false);
        _itemDescription.ResetDescription();
    }

    public void InitializeInventoryUI(int inventorySize)
    {
        for (int i = 0; i < inventorySize; ++i)
        {
            UIInventoryItem uiItem = Instantiate(_itemPrefab, Vector3.zero, Quaternion.identity);
            uiItem.transform.SetParent(_contentPanel);
            uiItem.transform.localScale = new Vector3(1, 1, 1);
            _uiInventoryItems.Add(uiItem);

            uiItem.OnItemClicked += HandleItemSelection;
            uiItem.OnItemBeginDrag += HandleBeginDrag;
            uiItem.OnItemDroppedOn += HandleSwap;
            uiItem.OnItemEndDrag += HandleEndDrag;
            uiItem.OnRightMouseBtnClick += HandleShowItemActions;
        }
    }

    public void UpdateData(int itemIndex, Sprite itemImage, int itemQuantity)
    {
        if (_uiInventoryItems.Count > itemIndex)
        {
            _uiInventoryItems[itemIndex].SetData(itemImage, itemQuantity);
        }
    }

    private void HandleShowItemActions(UIInventoryItem item)
    {

    }

    private void HandleSwap(UIInventoryItem item)
    {
        int index = _uiInventoryItems.IndexOf(item);
        if (index == -1)
        {
            return;
        }
        OnSwapItems?.Invoke(currentlyDraggedItemIndex, index);
    }

    private void ResetDraggedItem()
    {
        _mouseFollower.Toggle(false);
        currentlyDraggedItemIndex = -1;
    }

    private void HandleEndDrag(UIInventoryItem item)
    {
        ResetDraggedItem();
    }

    private void HandleBeginDrag(UIInventoryItem item)
    {
        int index = _uiInventoryItems.IndexOf(item);
        if (index == -1)
        {
            return;
        }
        currentlyDraggedItemIndex = index;
        HandleItemSelection(item);
        OnStartDragging?.Invoke(index);

    }

    public void CreateDraggedItem(Sprite sprite, int quantity)
    {
        _mouseFollower.Toggle(true);
        _mouseFollower.SetData(sprite, quantity);
    }

    private void HandleItemSelection(UIInventoryItem item)
    {
        int index = _uiInventoryItems.IndexOf(item);
        if (index == -1)
        {
            return;
        }
        OnDescriptionRequested?.Invoke(index);
    }

    public void Show()
    {
        gameObject.SetActive(true);

        ResetSelection();
    }

    private void ResetSelection()
    {
        _itemDescription.ResetDescription();
        DeselectAllItems();
    }

    private void DeselectAllItems()
    {
        foreach (var item in _uiInventoryItems)
        {
            item.Deselect();
        }
    }

    public void Hide()
    {
        gameObject.SetActive(false);
        ResetDraggedItem();
    }

}
