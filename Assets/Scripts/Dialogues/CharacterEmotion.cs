using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class CharacterEmotion : MonoBehaviour
{
    [System.Serializable]
    public enum EmotionState
    {
        Default,
        Sad,
        Angry
    }

    [SerializeField] private List<Sprite> emotions;

    private Image _image;

    private void Start()
    {
        _image = GetComponent<Image>();
    }

    public void ChangeEmotion(EmotionState currentExpression)
    {
        _image.sprite = emotions[(int)currentExpression];
    }
}
