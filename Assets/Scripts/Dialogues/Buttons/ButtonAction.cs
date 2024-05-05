using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ButtonAction : MonoBehaviour
{
    public int index;
    private Button _button;
    private Narrator _narrator;
    private UnityAction _clickAction;
    void Start()
    {
        _button = GetComponent<Button>();
        _narrator = FindObjectOfType<Narrator>(); // must be single on scene
        _clickAction = new UnityAction(() =>
        {
            FindObjectOfType<AudioManager>()?.Play("ChoosePhrase");
            _narrator.ChooseChoiceIndex(index);
        });
        _button.onClick.AddListener(_clickAction);
    }
}
