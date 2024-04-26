using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
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
        Narrator.OnBackgroundChanged += Updatebackground;
        Narrator.OnBackgroundSpriteChanged += UpdatebackgroundSprite;
    }

    private void OnDisable()
    {
        Narrator.OnBackgroundChanged -= Updatebackground;
        Narrator.OnBackgroundSpriteChanged -= UpdatebackgroundSprite;
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
        Debug.Log(name);

        _currentBackground = FindBackgroundByName(name);
        if (_currentBackground.sprite == null)
        {
            _currentBackground.sprite = AIManager.LoadSpriteFromPNG(name); // TO DO: move from ai manager
        }

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
        Debug.Log($"Can not find background {name} among not generated backgrounds");
        return new Background(new List<string>() { }, null);
    }
}
