using Ink.Runtime;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class ChoiceDisplay : MonoBehaviour
{

    private GameObject _choiceButtonsPanel;
    private CanvasGroup _group;

    private GameObject _choiceButton;
    private List<TextMeshProUGUI> _choicesText = new();

    [Inject]
    public void Construct(DialoguesInstaller dialoguesInstaller)
    {
        _choiceButtonsPanel = dialoguesInstaller.choiceButtonsPanel;
        _choiceButton = dialoguesInstaller.choiceButton;
    }

    private void Awake()
    {
        _group = _choiceButtonsPanel.gameObject.GetComponent<CanvasGroup>();
    }

    private void OnEnable()
    {
        Narrator.OnChoicesAppeared += DisplayChoice;
        TalkManager.OnAITalkStarted += HideChoice;
    }
    private void OnDisable()
    {
        Narrator.OnChoicesAppeared -= DisplayChoice;
        TalkManager.OnAITalkStarted -= HideChoice;
    }

    private void DisplayChoice(List<Choice> choices)
    {
        if (choices.Count <= 0)
        {
            _choiceButtonsPanel.gameObject.transform.localScale = new Vector3(0, 0, 0);
            _group.LeanAlpha(0, 0.5f);
            return;
        }
        else
        {
            _group.LeanAlpha(1, 0.5f);
            _choiceButtonsPanel.gameObject.transform.localScale = new Vector3(1, 1, 1);
        }
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
        Debug.Log("Hiding choice panel.");
        _choiceButtonsPanel.gameObject.transform.localScale = new Vector3(0, 0, 0);
    }

}
