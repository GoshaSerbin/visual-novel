using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

[RequireComponent(typeof(Button))]
public class SendPhraseButton : MonoBehaviour
{
    private Button _sendPhraseButton;
    private TalkManager _talkManager;
    private TMP_InputField _inputField;

    [Inject]
    public void Construct(DialoguesInstaller dialoguesInstaller)
    {
        _inputField = dialoguesInstaller.inputField;
    }

    void Awake()
    {
        _talkManager = FindObjectOfType<TalkManager>();
        _sendPhraseButton = GetComponent<Button>();
    }

    void Start()
    {
        _sendPhraseButton.onClick.AddListener(SendPhrase);
    }

    private void SendPhrase()
    {
        _talkManager.SendPlayerPhrase(_inputField.text);
    }
}
