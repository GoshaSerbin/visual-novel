using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Zenject;

public class DialogueDisplay : MonoBehaviour
{

    private TextMeshProUGUI _dialogueText;
    private TextMeshProUGUI _characterNameText;

    private GameObject _dialoguePanel;

    [Inject]
    public void Construct(DialoguesInstaller dialoguesInstaller)
    {
        _dialoguePanel = dialoguesInstaller.dialoguePanel;
        _characterNameText = dialoguesInstaller.nameText;
        _dialogueText = dialoguesInstaller.dialogueText;
    }

    private void OnEnable()
    {
        Narrator.OnCharacterSaid += DisplayReplica;
        Narrator.OnStoryStarted += ShowPanel;
        Narrator.OnStoryEnded += HidePanel;
        TalkManager.OnCharacterAnswered += DisplayPhrase;
    }
    private void OnDisable()
    {
        Narrator.OnCharacterSaid -= DisplayReplica;
        Narrator.OnStoryStarted -= ShowPanel;
        Narrator.OnStoryEnded -= HidePanel;
        TalkManager.OnCharacterAnswered -= DisplayPhrase;
    }

    private void DisplayReplica(Narrator.Replica replica)
    {
        _dialogueText.gameObject.GetComponent<TextEffects>().Display(replica.Text);
        _characterNameText.text = replica.Name;
    }

    private void DisplayPhrase(string phrase)
    {
        _dialogueText.gameObject.GetComponent<TextEffects>().Display(phrase);
    }

    private void ShowPanel()
    {
        _dialoguePanel.SetActive(true);
    }
    private void HidePanel()
    {
        _dialoguePanel.SetActive(false);
    }
}
