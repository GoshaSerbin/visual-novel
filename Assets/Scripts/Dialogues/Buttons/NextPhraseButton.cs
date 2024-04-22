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

    void Awake()
    {
        _nextPhraseButton = GetComponent<Button>();
        _nextPhraseButton.onClick.AddListener(FindObjectOfType<Narrator>().ContinueStory);
    }

    private void OnEnable()
    {
        // OnChoicesAppeared MUST be called before OnAITalkStarted 
        Narrator.OnChoicesAppeared += ShowOrHide;
        TalkManager.OnAITalkStarted += Hide;
        // Narrator.OnStoryCanNotContinue += Hide;
        // Narrator.OnStoryCanContinue += Show;

    }

    private void OnDisable()
    {
        Narrator.OnChoicesAppeared -= ShowOrHide;
        TalkManager.OnAITalkStarted -= Hide;
        // Narrator.OnStoryCanNotContinue -= Hide;
        // Narrator.OnStoryCanContinue -= Show;
    }

    private void ShowOrHide(List<Ink.Runtime.Choice> choices)
    {
        if (choices.Count > 0)
        {
            Hide();
        }
        else
        {
            Show();
        }
    }

    void Show()
    {
        // _nextPhraseButton.gameObject.SetActive(true);
        _nextPhraseButton.gameObject.transform.localScale = new Vector3(1, 1, 1);
    }

    void Hide()
    {
        _nextPhraseButton.gameObject.transform.localScale = new Vector3(0, 0, 0);
        // _nextPhraseButton.gameObject.SetActive(false);
    }
}
