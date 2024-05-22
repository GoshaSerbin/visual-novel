using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class AskManager : MonoBehaviour
{
    [SerializeField] private TMP_InputField _inputField;
    [SerializeField] private GameObject _inputFieldPanel;
    [SerializeField] private Button _answerButton;

    public static event Action OnPlayerAnaswered;

    private string _varNameToSet;

    private void Start()
    {
        _answerButton.onClick.AddListener(Answer);
    }

    private void OnEnable()
    {
        Narrator.OnPlayerAsked += Ask;
    }

    private void OnDisable()
    {
        Narrator.OnPlayerAsked -= Ask;
    }

    private void Ask(string VarNameToSet)
    {
        _varNameToSet = VarNameToSet;
        Debug.Log("Showing input fieldPanel.");
        _inputFieldPanel.SetActive(true);
        _inputField.enabled = true;
        _inputField.text = "";
        _inputField.caretPosition = 0;
        _inputField.ActivateInputField();
    }
    public void Answer()
    {
        if (_inputField.text == "")
        {
            return;
        }
        FindObjectOfType<Narrator>().ChangeVariableState(_varNameToSet, _inputField.text);
        _inputFieldPanel.SetActive(false);
        OnPlayerAnaswered.Invoke();
        FindObjectOfType<Narrator>().ContinueStory();
    }
}
