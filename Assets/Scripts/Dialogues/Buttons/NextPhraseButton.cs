using System;
using System.Collections;
using System.Collections.Generic;
using Ink.Parsed;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class NextPhraseButton : MonoBehaviour
{

    private Button _nextPhraseButton;
    private DialogueDisplay _dialogueDisplay;

    void Awake()
    {
        _nextPhraseButton = GetComponent<Button>();
        _nextPhraseButton.onClick.AddListener(ContinueStoryOrCompletePhrase);
        _dialogueDisplay = FindAnyObjectByType<DialogueDisplay>();
    }

    void ContinueStoryOrCompletePhrase()
    {
        if (_dialogueDisplay.IsAnimatingText())
        {
            _dialogueDisplay.CompletePhrase();
        }
        else
        {
            FindObjectOfType<Narrator>().ContinueStory();
        }
    }

    private void OnEnable()
    {
        Narrator.OnChoicesAppeared += ShowOrHide;
        Narrator.OnChoiceChosen += Show;
        TalkManager.OnAITalkStarted += Hide;
        TalkManager.OnAITalkStopped += Show;
    }

    private void OnDisable()
    {
        Narrator.OnChoicesAppeared -= ShowOrHide;
        Narrator.OnChoiceChosen -= Show;
        TalkManager.OnAITalkStarted -= Hide;
        TalkManager.OnAITalkStopped -= Show;
    }

    private void ShowOrHide(List<Ink.Runtime.Choice> choices)
    {
        if (choices.Count > 0)
        {
            Hide();
        }
    }

    void Show()
    {
        _nextPhraseButton.gameObject.transform.localScale = new Vector3(1, 1, 1);
    }

    void Hide()
    {
        _nextPhraseButton.gameObject.transform.localScale = new Vector3(0, 0, 0);
    }
}
