using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(UnityEngine.UI.Image))]
// [RequireComponent(typeof(CharacterData))]
public class TalkAnimations : MonoBehaviour
{

    [SerializeField] private float _sizeMultiplier = 1.07f; // how big when talking
    [SerializeField] private float _darkMultiplier = 0.65f; // how bright when not talking
    [SerializeField] private float _animationTime = 0.5f;
    private Vector3 _defaultScale;
    private Color _defaultColor;
    private UnityEngine.UI.Image _image;
    private CharacterData _characterData;

    [System.Serializable]
    public enum EmotionState
    {
        Default,
        Sad,
        Angry
    }
    private void Awake()
    {
        _defaultScale = transform.localScale;
        _image = GetComponent<UnityEngine.UI.Image>();
        _characterData = GetComponent<CharacterData>();
        _defaultColor = new Color(_image.color.r * _darkMultiplier, _image.color.g * _darkMultiplier, _image.color.b * _darkMultiplier, 1);
        _image.color = _defaultColor;

    }

    public void StartTalking()
    {
        // more curves https://codepen.io/jhnsnc/pen/LpVXGM
        // https://youtu.be/YqMpVCPX2ls?si=9i9fF6VOTOgh7CFp
        transform.LeanScale(_sizeMultiplier * _defaultScale, _animationTime).setEaseOutBack();

        Color fromColor = _image.color;
        Color toColor = new Color(_defaultColor.r / _darkMultiplier, _defaultColor.g / _darkMultiplier, _defaultColor.b / _darkMultiplier, 1);
        LeanTween.value(_image.gameObject, SetColorCallback, fromColor, toColor, _animationTime);
    }

    private void SetColorCallback(Color color)
    {
        _image.color = color;
    }

    public void StopTalking()
    {
        transform.LeanScale(_defaultScale, _animationTime).setEaseOutBack();

        Color fromColor = _image.color;
        Color toColor = _defaultColor;
        LeanTween.value(_image.gameObject, SetColorCallback, fromColor, toColor, _animationTime);
    }

    public void ChangeEmotion(Sprite sprite)
    {
        _image.sprite = sprite;
    }
}
