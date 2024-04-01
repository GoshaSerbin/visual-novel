using Ink.Runtime;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;
using Zenject;

public class Dialogues : MonoBehaviour
{
    private Story _inkStory;
    private UnityEngine.TextAsset _inkJson;

    private Button _nextPhraseButton;

    private CharactersManager _charactersManager;

    private ServerCommunication _server;
    private AIManager _aiManager;

    public bool IsPrewrittenDialoguePlay { get; private set; }

    public struct Replica
    {
        public string Name;
        public string Text;
        public CharacterEmotion.EmotionState Emotion;

        public Replica(string name, string text, CharacterEmotion.EmotionState emotion)
        {
            Name = name;
            Text = text;
            Emotion = emotion;
        }
    }
    public static event Action<string> OnBackgroundChanged;

    public static event Action<Replica> OnCharacterSaid;
    public static event Action OnDialogueStarted;
    public static event Action OnDialogueStoped;
    public static event Action<List<Choice>> OnStoryContinued;

    private Dictionary<string, string> _currentTags = new Dictionary<string, string>(){
            {"speaker", ""},
            {"emotion", "0"},
            {"temperature", "1"},
        };

    [Inject]
    public void Construct(DialoguesInstaller dialoguesInstaller)
    {
        _inkJson = dialoguesInstaller.inkJson;
        _nextPhraseButton = dialoguesInstaller.nextPhraseButton;
    }

    private void Awake()
    {
        _inkStory = new Story(_inkJson.text);
        _nextPhraseButton.onClick.AddListener(ContinueStory);
    }

    private void OnEnable()
    {
        AIManager.OnAITalkAnswered += AITalkAnswer;
        AIManager.OnAITalkStarted += AITalkStart;
        AIManager.OnAITalkStoped += AITalkStop;
    }

    private void OnDisable()
    {
        AIManager.OnAITalkAnswered -= AITalkAnswer;
        AIManager.OnAITalkStarted -= AITalkStart;
        AIManager.OnAITalkStoped -= AITalkStop;
    }

    private void AITalkStart()
    {
        Debug.Log("Disabling next phrase");
        IsPrewrittenDialoguePlay = false;
        _nextPhraseButton.gameObject.SetActive(false); // TO DO: move it to display
        OnCharacterSaid.Invoke(new Replica(
            _currentTags["speaker"],
            _inkStory.currentText,
            (CharacterEmotion.EmotionState)Convert.ToInt32(_currentTags["emotion"])
        ));
    }

    private void AITalkAnswer(string response)
    {
        IsPrewrittenDialoguePlay = false;
        OnCharacterSaid.Invoke(new Replica(
            _currentTags["speaker"],
            response,
            (CharacterEmotion.EmotionState)Convert.ToInt32(_currentTags["emotion"])
        ));
    }

    private void AITalkStop()
    {
        Debug.Log("AITalk stop");
        IsPrewrittenDialoguePlay = true;
        if (_inkStory.canContinue)
        {
            ContinueStory();
        }
    }

    void Start()
    {
        _server = FindObjectOfType<ServerCommunication>(); // must be single
        _aiManager = FindObjectOfType<AIManager>(); // must be single
        _charactersManager = FindObjectOfType<CharactersManager>(); // must be single
        StartDialogue();
    }

    public void StartDialogue()
    {
        OnDialogueStarted?.Invoke();
        IsPrewrittenDialoguePlay = true;
        ContinueStory();
    }

    public void ContinueStory()
    {
        if (_inkStory.canContinue)
        {
            _nextPhraseButton.gameObject.SetActive(true); // AIManager and Choice display can disable it
            _inkStory.Continue();
            UpdateCurrentTags();

            // reset characters
            _charactersManager.ResetCharacters(_currentTags["reset_characters"]);

            // update background
            OnBackgroundChanged.Invoke(_currentTags["background"]);

            if (_currentTags["AI"] != "")
            {
                Debug.Log(_currentTags["AI"]);
                HandleAI();
            }
            else
            {
                OnCharacterSaid.Invoke(new Replica(
                    _currentTags["speaker"],
                    _inkStory.currentText,
                    (CharacterEmotion.EmotionState)Convert.ToInt32(_currentTags["emotion"])
                ));

            }

            if (_currentTags["AI"] != "TALK")
            {
                Debug.Log("display choices");
                OnStoryContinued.Invoke(_inkStory.currentChoices);
            }

        }
        else
        {
            ExitDialogue();
        }
    }

    private void HandleAI()
    {
        switch (_currentTags["AI"])
        {
            case "TALK":
                {
                    _aiManager.TalkWith(_currentTags["system"]);
                    break;
                }
            // basically "describe" uses only user prompt and "answer" uses both system and user
            case "DESCRIBE":
                {
                    _aiManager.Describe(
                        _inkStory.currentText,
                        (string response) =>
                        {
                            OnCharacterSaid.Invoke(new Replica(
                                _currentTags["speaker"],
                                response,
                                (CharacterEmotion.EmotionState)Convert.ToInt32(_currentTags["emotion"])
                            ));
                        },
                        Convert.ToInt32(_currentTags["max_tokens"]),
                        Convert.ToSingle(_currentTags["temperature"])
                    );
                    break;
                }
            case "ANSWER":
                {
                    _aiManager.Answer(
                        _inkStory.currentText,
                        _currentTags["system"],
                        (string response) =>
                        {
                            OnCharacterSaid.Invoke(new Replica(
                                _currentTags["speaker"],
                                response,
                                (CharacterEmotion.EmotionState)Convert.ToInt32(_currentTags["emotion"])
                            ));
                        },
                        Convert.ToInt32(_currentTags["max_tokens"]),
                        Convert.ToSingle(_currentTags["temperature"])
                    );
                    break;
                }
            default:
                {
                    Debug.LogError("Unknown ai tag " + _currentTags["AI"]);
                    break;
                }
        }
    }

    private void UpdateCurrentTags()
    {
        // reset
        _currentTags["AI"] = "";
        _currentTags["max_tokens"] = "";
        foreach (string tag in _inkStory.currentTags)
        {

            string[] parts = tag.Split(':');
            if (parts.Count() != 2)
            {
                Debug.LogError($"Strange tag {tag}");
                continue;
            }

            string key = parts[0].Trim();
            string value = parts[1].Trim();
            _currentTags[key] = value;
            switch (key)
            {
                case "speaker":
                case "emotion":
                case "background":
                case "AI":
                case "system":
                case "max_tokens":
                case "reset_characters":
                case "temperature":
                    {
                        break;
                    }
                default:
                    {
                        Debug.LogError($"Unknown tag {key}!");
                        break;
                    }
            }
        }
    }

    public void ChoiceButtonAction(int choiceIndex)
    {
        Debug.Log("chosen story index" + choiceIndex);
        _inkStory.ChooseChoiceIndex(choiceIndex);
        ContinueStory();
    }

    private void ExitDialogue()
    {
        IsPrewrittenDialoguePlay = false;
        Debug.Log("End dialogue");
        OnDialogueStoped.Invoke();
        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
        if (nextSceneIndex < SceneManager.sceneCount)
        {
            SceneManager.LoadScene(nextSceneIndex);
        }
    }

}
