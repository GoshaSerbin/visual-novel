using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryAnimation : MonoBehaviour
{

    [SerializeField] private CanvasGroup _inventoryBackground;
    [SerializeField] private Transform _inventoryPanel;

    private void Awake()
    {
        _inventoryPanel = transform;
        _inventoryPanel.localPosition = new Vector2(0, -Screen.height);
    }

    public void Open()
    {
        _inventoryBackground.gameObject.SetActive(true);
        _inventoryBackground.alpha = 0;
        _inventoryBackground.LeanAlpha(0.5f, 0.5f);

        _inventoryPanel.localPosition = new Vector2(0, -Screen.height);
        _inventoryPanel.LeanMoveLocalY(0, 0.5f).setEaseOutExpo().delay = 0.1f;
    }

    public void Close()
    {
        _inventoryBackground.LeanAlpha(0, 0.5f);
        _inventoryPanel.LeanMoveLocalY(-Screen.height, 0.5f).setEaseInExpo().setOnComplete(OnComplete);
    }

    private void OnComplete()
    {
        _inventoryBackground.gameObject.SetActive(false);
    }
}
