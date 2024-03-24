using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;
using UnityEngine.UI;

public class CharacterAnimations : MonoBehaviour
{

    private float _sizeMultiplier = 1.1f;
    private float _darkMultiplier = 0.6f;
    private float _animationTime = 0.4f;
    private Vector3 _defaultScale;
    private Color _defaultColor;

    private CanvasGroup _canvas;
    private UnityEngine.UI.Image _image;

    private void Start()
    {
        _defaultScale = transform.localScale;
        _image = GetComponent<UnityEngine.UI.Image>();
        _defaultColor = _image.color;
        _canvas = GetComponent<CanvasGroup>();
    }

    public void StartTalking()
    {
        // more curves https://codepen.io/jhnsnc/pen/LpVXGM
        transform.LeanScale(_sizeMultiplier * _defaultScale, _animationTime).setEaseOutBack();

        Color fromColor = _image.color;
        Color toColor = _defaultColor;
        LeanTween.value(_image.gameObject, setColorCallback, fromColor, toColor, _animationTime);
    }

    private void setColorCallback(Color color)
    {
        _image.color = color;
    }

    public void StopTalking()
    {
        transform.LeanScale(_defaultScale, _animationTime).setEaseOutBack();

        Color fromColor = _image.color;
        Color toColor = new Color(_defaultColor.r * _darkMultiplier, _defaultColor.g * _darkMultiplier, _defaultColor.b * _darkMultiplier, 1);
        LeanTween.value(_image.gameObject, setColorCallback, fromColor, toColor, _animationTime);
    }
}
