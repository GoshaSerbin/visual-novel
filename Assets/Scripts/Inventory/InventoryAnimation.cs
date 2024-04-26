using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryAnimation : MonoBehaviour
{

    [SerializeField] private CanvasGroup _inventoryBackground;
    [SerializeField] private Transform _inventoryPanel;

    private Vector3 _hiddenPosition;

    private float _animationTime = 0.5f;

    private void Awake()
    {
        _inventoryPanel = transform;
        Vector3 screenPos = new Vector3(0.5f, -0.5f, 10);
        _hiddenPosition = Camera.main.ViewportToWorldPoint(screenPos);
        _inventoryPanel.transform.position = _hiddenPosition;
    }

    public void Open()
    {
        Debug.Log("Invetory opened");
        _inventoryBackground.gameObject.SetActive(true);
        _inventoryBackground.alpha = 0;
        _inventoryBackground.LeanAlpha(0.5f, _animationTime);

        _inventoryPanel.transform.position = _hiddenPosition;
        _inventoryPanel.LeanMoveLocalY(0, _animationTime).setEaseOutExpo().delay = 0.1f;
    }

    public void Close()
    {
        Debug.Log("Invetory closed");
        _inventoryBackground.LeanAlpha(0, _animationTime);
        _inventoryPanel.LeanMoveY(_hiddenPosition.y, _animationTime).setEaseInExpo().setOnComplete(OnComplete);
    }

    private void OnComplete()
    {
        _inventoryBackground.gameObject.SetActive(false);
    }
}
