using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CustomSlider : Slider, IEndDragHandler
{
    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log("EndDrag");
        FindObjectOfType<AudioManager>().Play("NewQuest");
    }
}