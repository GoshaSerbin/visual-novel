using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Zenject;

public class TalkDisplay : MonoBehaviour
{
    private GameObject _inputFieldPanel;
    private TMP_InputField _inputField;

    [Inject]
    public void Construct(DialoguesInstaller dialoguesInstaller)
    {
        _inputFieldPanel = dialoguesInstaller.inputFieldPanel;
        _inputField = dialoguesInstaller.inputField;
    }
    void OnEnable()
    {
        TalkManager.OnAITalkStarted += Show;
        TalkManager.OnPlayerPhraseSent += DisableInputField;
        TalkManager.OnCharacterAnswered += OnCharacterAnswered;
        TalkManager.OnAITalkStopped += Hide;
    }

    void OnDisable()
    {
        TalkManager.OnAITalkStarted -= Show;
        TalkManager.OnPlayerPhraseSent -= DisableInputField;
        TalkManager.OnCharacterAnswered -= OnCharacterAnswered;
        TalkManager.OnAITalkStopped -= Hide;
    }

    private void OnCharacterAnswered(string answer)
    {
        Show();
    }

    private void Show()
    {
        Debug.Log("Showing input fieldPanel.");
        _inputFieldPanel.SetActive(true);
        _inputField.enabled = true;
        _inputField.ActivateInputField();
    }

    private void DisableInputField()
    {
        _inputField.enabled = false;
    }

    private void Hide()
    {
        _inputFieldPanel.SetActive(false);
    }
}
