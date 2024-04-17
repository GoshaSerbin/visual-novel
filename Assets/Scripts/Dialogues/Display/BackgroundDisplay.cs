using System.Collections;
using System.Collections.Generic;
using Microsoft.Unity.VisualStudio.Editor;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class BackgroundDisplay : MonoBehaviour
{

    [System.Serializable]
    public struct Background
    {
        public List<string> Names;
        public Sprite sprite;
        public Background(List<string> names, Sprite backgroundImage)
        {
            Names = names;
            sprite = backgroundImage;
        }
    }

    private Background _currentBackground = new Background(new List<string>() { }, null);

    [SerializeField] private List<Background> _backgrounds;

    [SerializeField] private UnityEngine.UI.Image _backgroundImage;

    [SerializeField] private float _animationTime;

    private void OnEnable()
    {
        Dialogues.OnBackgroundChanged += Updatebackground;
        Dialogues.OnBackgroundSpriteChanged += UpdatebackgroundSprite;
    }

    private void OnDisable()
    {
        Dialogues.OnBackgroundChanged -= Updatebackground;
        Dialogues.OnBackgroundSpriteChanged -= UpdatebackgroundSprite;
    }

    private void UpdatebackgroundSprite(Sprite sprite)
    {
        _currentBackground = new Background(new(), sprite);
        LeanTween.value(_backgroundImage.gameObject, SetColorCallback, _backgroundImage.color, new Color(0, 0, 0), _animationTime).setOnComplete(UpdateSpriteAndFadeOut);
    }

    private void Updatebackground(string name)
    {
        if (_currentBackground.Names.Contains(name))
        {
            return;
        }
        _currentBackground = FindBackgroundByName(name);
        LeanTween.value(_backgroundImage.gameObject, SetColorCallback, _backgroundImage.color, new Color(0, 0, 0), _animationTime).setOnComplete(UpdateSpriteAndFadeOut);
    }

    private void SetColorCallback(Color color)
    {
        _backgroundImage.color = color;
    }

    private void UpdateSpriteAndFadeOut()
    {
        // TO DO: не оч красиво сделано
        _backgroundImage.sprite = _currentBackground.sprite;
        LeanTween.value(_backgroundImage.gameObject, SetColorCallback, _backgroundImage.color, new Color(1, 1, 1), _animationTime);
    }

    private Background FindBackgroundByName(string name)
    {
        foreach (var background in _backgrounds)
        {
            if (background.Names.Contains(name))
            {
                return background;
            }
        }
        Debug.LogError($"Can not find background {name} among backgrounds");
        return _currentBackground;
    }
}
