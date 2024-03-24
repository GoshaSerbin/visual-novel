using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    public void ChangeEmotion(EmotionState currentExpression)
    {
        GetComponent<Image>().sprite = emotions[(int)currentExpression];
    }
}
