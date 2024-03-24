using System.Collections;
using System.Collections.Generic;
using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;
using UnityEngine.UI;

public class CharacterAnimations : MonoBehaviour
{

    private float _sizeMultiplier = 1.1f;
    private float _animationTime = 0.4f;
    private Vector3 _defaultScale;
    private Color _defaultColor;

    private CanvasGroup _canvas;

    private void Start()
    {
        _defaultScale = transform.localScale;
        _defaultColor = GetComponent<UnityEngine.UI.Image>().color;
        // _canvas = GetComponent<CanvasGroup>();
    }

    public void StartTalking()
    {
        // more curves https://codepen.io/jhnsnc/pen/LpVXGM
        transform.LeanScale(_sizeMultiplier * _defaultScale, _animationTime).setEaseOutBack();
        // LeanTweenExt.LeanAlpha(_canvas, 1f, 0.75);
        // LeanTween.value(img.gameObject, setColorCallback, fromColor, toColor, .25f)
    }

    public void StopTalking()
    {
        transform.LeanScale(_defaultScale, _animationTime).setEaseOutBack();
    }
}
