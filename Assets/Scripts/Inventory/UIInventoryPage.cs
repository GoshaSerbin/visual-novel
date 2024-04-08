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

    public Sprite image;
    public int quantity;
    public string title, description;


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

    private void HandleShowItemActions(UIInventoryItem item)
    {
        throw new NotImplementedException();
    }

    private void HandleSwap(UIInventoryItem item)
    {
        _mouseFollower.Toggle(false);
    }

    private void HandleEndDrag(UIInventoryItem item)
    {
        _mouseFollower.Toggle(false);
    }

    private void HandleBeginDrag(UIInventoryItem item)
    {
        _mouseFollower.Toggle(true);
        _mouseFollower.SetData(image, quantity);
    }

    private void HandleItemSelection(UIInventoryItem item)
    {
        _itemDescription.SetDescription(image, title, description);
        _uiInventoryItems[0].Select();
    }

    public void Show()
    {
        gameObject.SetActive(true);
        _itemDescription.ResetDescription();
        _uiInventoryItems[0].SetData(image, quantity);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
