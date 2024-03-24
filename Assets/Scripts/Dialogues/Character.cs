using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Character : MonoBehaviour
{
    [System.Serializable]
    public enum EmotionState
    {
        Default,
        Sad,
        Angry
    }

    [SerializeField] private List<Sprite> emotions;

    [SerializeField] private string characterName = "";
    public string CharacterName { get { return characterName; } private set { characterName = value; } }

    private Vector3 defaultScale;
    public Vector3 DefaultScale => defaultScale;

    private void OnEnable()
    {
        defaultScale = transform.localScale;
    }
    public void ChangeEmotion(EmotionState currentExpression)
    {
        GetComponent<Image>().sprite = emotions[(int)currentExpression];
    }
    public void ChangeScale(float multiplier)
    {
        transform.localScale *= multiplier;
    }
    public void ResetScale()
    {
        transform.localScale = defaultScale;
    }
}