using System.Collections;
using System.Collections.Generic;
using Microsoft.Unity.VisualStudio.Editor;
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
    private void OnEnable()
    {
        Dialogues.OnBackgroundChanged += Updatebackground;
    }

    private void OnDisable()
    {
        Dialogues.OnBackgroundChanged -= Updatebackground;
    }

    private void Updatebackground(string name)
    {
        if (_currentBackground.Names.Contains(name))
        {
            return;
        }
        _currentBackground = FindBackgroundByName(name);
        _backgroundImage.sprite = _currentBackground.sprite;
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
