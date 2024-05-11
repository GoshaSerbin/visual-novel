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

    [SerializeField] private GameObject _CharacterNamePanel;

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
        if (replica.Name == "")
        {
            _CharacterNamePanel.gameObject.transform.localScale = new Vector3(0, 0, 0);
        }
        else
        {
            _CharacterNamePanel.gameObject.transform.localScale = new Vector3(1, 1, 1);
        }
    }

    public bool IsAnimatingText()
    {
        return _dialogueText.gameObject.GetComponent<TextEffects>().IsAnimatingText;
    }

    private void DisplayPhrase(string phrase)
    {
        _dialogueText.gameObject.GetComponent<TextEffects>().Display(phrase);
    }

    public void CompletePhrase()
    {
        _dialogueText.gameObject.GetComponent<TextEffects>().CompleteDisplay();
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
