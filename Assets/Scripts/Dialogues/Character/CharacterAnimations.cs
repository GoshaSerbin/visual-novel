using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(UnityEngine.UI.Image))]
[RequireComponent(typeof(CharacterInfo))]
[RequireComponent(typeof(CharacterEmotion))]
public class CharacterAnimations : MonoBehaviour
{

    private float _sizeMultiplier = 1.07f;
    private float _darkMultiplier = 0.65f;
    private float _animationTime = 0.5f;
    private Vector3 _defaultScale;
    private Color _defaultColor;
    private UnityEngine.UI.Image _image;
    private CharacterInfo _characterInfo;
    private CharacterEmotion _characterEmotion;

    private void Awake()
    {
        _defaultScale = transform.localScale;
        _image = GetComponent<UnityEngine.UI.Image>();
        _characterInfo = GetComponent<CharacterInfo>();
        _characterEmotion = GetComponent<CharacterEmotion>();
        _defaultColor = new Color(_image.color.r * _darkMultiplier, _image.color.g * _darkMultiplier, _image.color.b * _darkMultiplier, 1);
        _image.color = _defaultColor;

    }

    private void OnEnable()
    {
        Dialogues.OnCharacterSaid += Animate;
    }
    private void OnDisable()
    {
        Dialogues.OnCharacterSaid -= Animate;
    }

    public void Appear(bool fromLeft)
    {
        // Vector3 initPosition = transform.position;
        // Vector3 delta = new Vector3(0, 0, 0);
        // if (fromLeft)
        // {
        //     delta *= -1;
        // }
        // transform.position = initPosition + delta;
        // transform.LeanMoveLocalX(initPosition.x, 5);
    }

    public void Dissapear()
    {

    }

    private void Animate(Dialogues.Replica replica)
    {
        if (_characterInfo.CharacterNames.Contains(replica.Name))
        {
            StartTalking();
            _characterEmotion.ChangeEmotion(replica.Emotion);
        }
        else
        {
            StopTalking();
        }
    }

    public void StartTalking()
    {
        // more curves https://codepen.io/jhnsnc/pen/LpVXGM
        // https://youtu.be/YqMpVCPX2ls?si=9i9fF6VOTOgh7CFp
        Debug.Log("Start talking");
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
        Debug.Log("Stop talking");
        transform.LeanScale(_defaultScale, _animationTime).setEaseOutBack();

        Color fromColor = _image.color;
        Color toColor = _defaultColor;
        LeanTween.value(_image.gameObject, SetColorCallback, fromColor, toColor, _animationTime);
    }
}
