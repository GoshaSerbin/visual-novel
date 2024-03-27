using Ink.Runtime;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using Zenject;

public class Dialogues : MonoBehaviour
{
    private Story _currentStory;
    private TextAsset _inkJson;

    private GameObject _dialoguePanel;
    private TextMeshProUGUI _dialogueText;
    private TextMeshProUGUI _nameText;

    [HideInInspector] public GameObject _choiceButtonsPanel;
    private GameObject _inputFieldPanel;
    private TMP_InputField _inputField;
    private GameObject _choiceButton;
    private List<TextMeshProUGUI> _choicesText = new();
    private GameObject[] _characters;

    private ServerCommunication _server;

    public bool DialogPlay { get; private set; }

    [Inject]
    public void Construct(DialoguesInstaller dialoguesInstaller)
    {
        _inkJson = dialoguesInstaller.inkJson;
        _dialoguePanel = dialoguesInstaller.dialoguePanel;
        _dialogueText = dialoguesInstaller.dialogueText;
        _nameText = dialoguesInstaller.nameText;
        _choiceButtonsPanel = dialoguesInstaller.choiceButtonsPanel;
        _inputFieldPanel = dialoguesInstaller.inputFieldPanel;
        _inputField = dialoguesInstaller.inputField;
        _choiceButton = dialoguesInstaller.choiceButton;
    }
    private void Awake()
    {
        _currentStory = new Story(_inkJson.text);
    }

    private string PostProccess(string aiAnswer)
    {
        var charsToRemove = new string[] { "\n" };
        foreach (var c in charsToRemove)
        {
            aiAnswer = aiAnswer.Replace(c, string.Empty);
        }
        return aiAnswer;
    }
    void Start()
    {
        _characters = GameObject.FindGameObjectsWithTag("Character");
        _server = FindObjectOfType<ServerCommunication>(); // must be single
        StartDialogue();
    }
    public void StartDialogue()
    {
        DialogPlay = true;
        _dialoguePanel.SetActive(true);
        ContinueStory();
    }
    public void ContinueStory(bool choiceBefore = false)
    {
        if (_currentStory.canContinue)
        {
            string story = _currentStory.Continue();
            Debug.Log("story is " + story);
            if (_currentStory.currentTags.Any("AI_DESCRIBE".Contains))
            {
                Debug.Log("Describing!");
                string prompt = story.TrimEnd('\n');
                _nameText.text = "";
                WWWForm form = new WWWForm();

                var messages = new List<ServerCommunication.Message>
                    {
                        new ServerCommunication.Message("user", prompt)
                    };
                string jsonMessages = ServerCommunication.ToJSON(messages);
                form.AddField("message", jsonMessages);
                form.AddField("max_tokens", 100);
                form.AddField("temperature", 1);
                // TO DO: disable input?
                _server.SendRequestToServer(form, (string response) =>
                {
                    if (response == "")
                    {
                        // handle it
                        _dialogueText.text = "ошибочка";
                        return;
                    }
                    _dialogueText.text = PostProccess(response);
                });
                // ContinueStory(choiceBefore);
            }
            else
            {

                if (_currentStory.currentTags.Any("AI_ANSWER".Contains))
                {
                    string question = story.TrimEnd('\n');
                    string name = (string)_currentStory.variablesState["characterName"];
                    Debug.Log(name + " is answering!");

                    int talkingIndex = GetCharacterIndexByName(_characters, name);
                    Debug.Log(name + "his index is " + talkingIndex);
                    var ai = _characters[talkingIndex].GetComponent<CharacterAI>();
                    ai.Ask(question, (string answer) =>
                    {
                        Debug.Log("He answered");
                        // _currentStory.ChooseChoiceIndex(choiceIndex);
                        ShowDialogue(PostProccess(answer));
                        ShowChoiceButtons();
                        // _currentStory.Continue();
                    });
                }
                else
                {
                    ShowDialogue(story);
                    if (_currentStory.currentTags.Any("AI_TALK".Contains))
                    {
                        ShowInputField();
                        Debug.Log("talking to AI!");

                        //disable clicking
                        DialogPlay = false;
                        Debug.Log("current text: " + _currentStory.currentText);
                        Debug.Log("current choice count: " + _currentStory.currentChoices.Count);
                        // var talkingIndex = GetCharacterIndexByName(_characters, _nameText.text);
                        // _characters[talkingIndex].GetComponent<CharacterAI>().ChangeEmotion(newEmotion);
                    }
                    else
                    {
                        _inputFieldPanel.SetActive(false);
                        ShowChoiceButtons();

                    }
                }

            }

        }
        else if (!choiceBefore)
        {
            Debug.Log("current story cant continue");
            ExitDialogue();
        }
    }

    public void ContinueAITalk()
    {
        string phrase = _inputField.text;
        _inputField.enabled = false;

        if (phrase == "")
        {
            // handle it
            return;
        }
        var index = GetCharacterIndexByName(_characters, _nameText.text); // TO DO: get active player
        var ai = _characters[index].GetComponent<CharacterAI>();
        ai.Ask(phrase, AIAnswerCallback);
    }
    private void ShowInputField()
    {
        _choiceButtonsPanel.SetActive(false);
        _inputFieldPanel.SetActive(true);
        _inputField.ActivateInputField();
    }

    private void AIAnswerCallback(string answer)
    {
        //must check if not null
        if (answer == null)
        {
            Debug.LogError("server returned error");
            return;
        }
        _dialogueText.text = PostProccess(answer);

        _inputField.enabled = true;
        _inputField.ActivateInputField();
    }

    private int GetCharacterIndexByName(GameObject[] characters, string name)
    {
        for (int i = 0; i < characters.Count(); ++i)
        {
            var chNames = characters[i].GetComponent<CharacterInfo>().CharacterName;
            for (int j = 0; j < chNames.Length; ++j)
            {
                if (chNames[j] == name)
                {
                    return i;
                }
            }
        }
        Debug.LogError($"Can not find name {name} among characters");
        return -1;
    }
    private void ShowDialogue(string dialogue)
    {
        _dialogueText.text = dialogue;
        _nameText.text = (string)_currentStory.variablesState["characterName"];

        int talkingIndex;
        if (_nameText.text == "")
        { // narrator
            talkingIndex = -1;
        }
        else
        {
            talkingIndex = GetCharacterIndexByName(_characters, _nameText.text);
            var newEmotion = (CharacterEmotion.EmotionState)_currentStory.variablesState["characterExpression"];
            _characters[talkingIndex].GetComponent<CharacterEmotion>().ChangeEmotion(newEmotion);
        }

        AnimateCharacters(talkingIndex);
    }
    private void AnimateCharacters(int talkingIndex)
    {
        for (int i = 0; i < _characters.Count(); ++i)
        {
            if (i == talkingIndex)
            {
                _characters[i].GetComponent<CharacterAnimations>().StartTalking();
            }
            else
            {
                _characters[i].GetComponent<CharacterAnimations>().StopTalking();
            }
        }
    }
    private void ShowChoiceButtons()
    {
        List<Choice> currentChoices = _currentStory.currentChoices;
        _choiceButtonsPanel.SetActive(currentChoices.Count != 0);
        if (currentChoices.Count <= 0) { return; }
        _choiceButtonsPanel.transform.Cast<Transform>().ToList().ForEach(child => Destroy(child.gameObject));
        _choicesText.Clear();
        for (int i = 0; i < currentChoices.Count; i++)
        {
            GameObject choice = Instantiate(_choiceButton, _choiceButtonsPanel.transform);
            choice.GetComponent<ButtonAction>().index = i;

            TextMeshProUGUI choiceText = choice.GetComponentInChildren<TextMeshProUGUI>();
            choiceText.text = currentChoices[i].text;
            _choicesText.Add(choiceText);
        }
    }
    public void ChoiceButtonAction(int choiceIndex)
    {
        Debug.Log("prev story is " + _currentStory.currentText);
        Debug.Log("chosen story index" + choiceIndex);
        _currentStory.ChooseChoiceIndex(choiceIndex);
        ContinueStory(true);
    }
    private void ExitDialogue()
    {
        DialogPlay = false;
        _dialoguePanel.SetActive(false);
        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
        if (nextSceneIndex < SceneManager.sceneCount)
        {
            SceneManager.LoadScene(nextSceneIndex);
        }
    }
    public void StopAITalkAndContinueStory()
    {
        Debug.Log("stop talking to ai");
        Debug.Log("current text: " + _currentStory.currentText);
        Debug.Log("current choice count: " + _currentStory.currentChoices.Count);
        DialogPlay = true;
        _inputFieldPanel.SetActive(false);
        ShowChoiceButtons();
        ContinueStory(_currentStory.currentChoices.Count > 0); // TO DO: rewrite code
    }

}
