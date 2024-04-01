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
        Dialogues.OnCharacterSaid += DisplayReplica;
        Dialogues.OnDialogueStarted += ShowPanel;
        Dialogues.OnDialogueStoped += HidePanel;
    }
    private void OnDisable()
    {
        Dialogues.OnCharacterSaid -= DisplayReplica;
        Dialogues.OnDialogueStarted -= ShowPanel;
        Dialogues.OnDialogueStoped -= HidePanel;
    }

    private void DisplayReplica(Dialogues.Replica replica)
    {
        _dialogueText.gameObject.GetComponent<TextEffects>().Display(replica.Text);
        _characterNameText.text = replica.Name;
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
