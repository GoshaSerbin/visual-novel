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

    private GameObject _sendPhraseButton;
    private GameObject _loadingIcon;

    [Inject]
    public void Construct(DialoguesInstaller dialoguesInstaller)
    {
        _inputFieldPanel = dialoguesInstaller.inputFieldPanel;
        _inputField = dialoguesInstaller.inputField;
        _sendPhraseButton = dialoguesInstaller.sendPhraseButton;
        _loadingIcon = dialoguesInstaller.loadingIcon;
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
        _loadingIcon.SetActive(false);
        _sendPhraseButton.transform.localScale = new Vector3(1, 1, 1);
    }

    private void DisableInputField()
    {
        _inputField.enabled = false;
        _loadingIcon.SetActive(true);
        _sendPhraseButton.transform.localScale = new Vector3(0, 0, 0);
    }

    private void Hide()
    {
        _inputFieldPanel.SetActive(false);
    }
}
