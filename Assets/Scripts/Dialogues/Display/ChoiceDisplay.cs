using Ink.Runtime;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class ChoiceDisplay : MonoBehaviour
{

    [HideInInspector] public GameObject _choiceButtonsPanel;

    private Button _nextPhraseButton;
    private GameObject _choiceButton;
    private List<TextMeshProUGUI> _choicesText = new();

    [Inject]
    public void Construct(DialoguesInstaller dialoguesInstaller)
    {
        _choiceButtonsPanel = dialoguesInstaller.choiceButtonsPanel;
        _choiceButton = dialoguesInstaller.choiceButton;
        _nextPhraseButton = dialoguesInstaller.nextPhraseButton;
    }

    private void OnEnable()
    {
        Dialogues.OnStoryContinued += DisplayChoice;
        AIManager.OnAITalkStarted += HideChoice;
    }
    private void OnDisable()
    {
        Dialogues.OnStoryContinued -= DisplayChoice;
        AIManager.OnAITalkStarted -= HideChoice;
    }

    private void DisplayChoice(List<Choice> choices)
    {
        _choiceButtonsPanel.SetActive(choices.Count != 0);
        if (choices.Count <= 0)
        {
            return;
        }
        _nextPhraseButton.gameObject.SetActive(false);
        _choiceButtonsPanel.transform.Cast<Transform>().ToList().ForEach(child => Destroy(child.gameObject));
        _choicesText.Clear();
        for (int i = 0; i < choices.Count; i++)
        {
            GameObject choice = Instantiate(_choiceButton, _choiceButtonsPanel.transform);
            choice.GetComponent<ButtonAction>().index = i;

            TextMeshProUGUI choiceText = choice.GetComponentInChildren<TextMeshProUGUI>();
            choiceText.text = choices[i].text;
            _choicesText.Add(choiceText);
        }
    }

    private void HideChoice()
    {
        _choiceButtonsPanel.SetActive(false);
    }

}
