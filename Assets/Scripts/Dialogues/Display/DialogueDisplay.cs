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

    [SerializeField] private float _secondsForOneSymbol = 0.02f;

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
        // _dialogueText.text = replica.Text;
        _characterNameText.text = replica.Name;
        StopAllCoroutines(); // is it ok?
        StartCoroutine(TypeSentence(replica.Text));
    }

    IEnumerator TypeSentence(string sentence)
    {
        _dialogueText.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            _dialogueText.text += letter;
            yield return new WaitForSeconds(_secondsForOneSymbol);
        }
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
