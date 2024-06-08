using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderAttackTime : MonoBehaviour
{
    // Start is called before the first frame update
    Slider _slider;
    [SerializeField] float _sliderVelocity = 0.5f;
    bool _forward = true;
    float _target = -1;
    float _sliderVal;
    [SerializeField] RectTransform _targetZone;

    private void Awake()
    {
        _slider = GetComponent<Slider>();
    }
    void Start()
    {
        _slider.value = 0;
        PlayerTurnStart();
    }

    // Update is called once per frame
    float GenerateNewTarget()
    { 
        return Random.Range(0f, 1f);
    }

    public void PlayerTurnStart()
    {
        _target = GenerateNewTarget();
        _targetZone.anchoredPosition  = new Vector3(_target * _targetZone.rect.width, 0, 0);
    }

    public float SliderValue() { return _sliderVal; }
    public float TargetValue() { return _target; }
    void Update()
    {
            _slider.value += (_forward ? 1 : (-1)) * _sliderVelocity * Time.deltaTime;
            _sliderVal = _slider.value;
            if (_slider.value == 1 || _slider.value == 0)
            {
                _forward = !_forward;
            }
    }
}
