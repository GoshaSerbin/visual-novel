using Ink.Runtime;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
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
    public static event Action<string> OnItemRecieved;
    public static event Action OnStoryAffected;

    private Dictionary<string, string> _currentTags = new Dictionary<string, string>(){
            {"speaker", ""},
            {"emotion", "0"},
            {"temperature", "1.2"},
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
        AIManager.OnAIRecievedItem += AIRecieveItem;
        AIManager.OnAIAffectedStory += AIAffectStory;
    }

    private void OnDisable()
    {
        AIManager.OnAITalkAnswered -= AITalkAnswer;
        AIManager.OnAITalkStarted -= AITalkStart;
        AIManager.OnAITalkStoped -= AITalkStop;
        AIManager.OnAIRecievedItem -= AIRecieveItem;
        AIManager.OnAIAffectedStory -= AIAffectStory;
    }

    private void AIAffectStory(string varName)
    {
        Debug.Log("affected story: " + varName);
        _inkStory.variablesState[varName] = 1;
        OnStoryAffected?.Invoke();
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

    private string[] getItems(string itemsEnumeration)
    {
        string[] items = itemsEnumeration.Split(",");
        for (int i = 0; i < items.Count(); ++i)
        {
            items[i] = items[i].Trim(' ');
        }
        return items;
    }
    [SerializeField]
    private Dictionary<string, string> _itemName2FileName = new Dictionary<string, string>(){
            {"таблетки", "Tablets"},
            {"деньги", "Money"},
        };

    private void AIRecieveItem(string item)
    {
        Debug.Log("player gets item " + item);
        OnItemRecieved?.Invoke(_itemName2FileName[item]);
    }

    private void AITalkAnswer(string response)
    {
        if (_currentTags["may_recieve_items"] != "")
        {
            string[] items = getItems(_currentTags["may_recieve_items"]);
            for (int i = 0; i < items.Count(); ++i)
            {
                _aiManager.IsRecieved(items[i], response);
            }
        }
        if (_currentTags["may_affect_vars"] != "")
        {
            string[] vars = getItems(_currentTags["may_affect_vars"]);
            string[] descriptions = getItems(_currentTags["may_affect_descriptions"]);
            for (int i = 0; i < vars.Count(); ++i)
            {
                _aiManager.IsAffected(vars[i], response, descriptions[i]);
            }
        }
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
        else
        {
            if (_inkStory.currentChoices.Count > 0)
            {
                OnStoryContinued.Invoke(_inkStory.currentChoices);
            }
            else
            {
                ExitDialogue();
            }

        }
    }

    void Start()
    {
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
        CultureInfo ci = (CultureInfo)CultureInfo.CurrentCulture.Clone();
        ci.NumberFormat.CurrencyDecimalSeparator = ".";
        float temperature = float.Parse(_currentTags["temperature"], NumberStyles.Any, ci);
        Debug.Log("parsed " + temperature);

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
                        temperature
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
                        temperature
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
        _currentTags["may_recieve_items"] = "";
        _currentTags["may_affect_vars"] = "";
        _currentTags["may_affect_descriptions"] = "";
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
                case "may_recieve_items":
                case "may_affect_vars":
                case "may_affect_descriptions":
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
