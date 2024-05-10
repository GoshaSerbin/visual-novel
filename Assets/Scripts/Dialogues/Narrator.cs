using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ink.Runtime;
using Zenject;
using System;
using System.Linq;
using System.Globalization;
using Inventory;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;

// This class is responsible for reading the ink story and creating relevent events
public class Narrator : MonoBehaviour
{

    public struct Replica
    {
        public string Name;
        public string Text;
        public TalkAnimations.EmotionState Emotion;

        public Replica(string name, string text, TalkAnimations.EmotionState emotion)
        {
            Name = name;
            Text = text;
            Emotion = emotion;
        }
    }

    public static event Action<string> OnBackgroundChanged;

    public static event Action<Replica> OnCharacterSaid;
    public static event Action OnStoryStarted;
    public static event Action OnStoryEnded;

    public static event Action<List<Choice>> OnChoicesAppeared;
    public static event Action<string[]> OnCharactersReset;
    public static event Action<string> OnStoryContinued;
    public static event Action OnChoiceChosen;
    public static event Action<string, int> OnItemReceived;
    public static event Action<string, int> OnItemRemoved;
    public static event Action OnStoryAffected;

    public static event Action<string> OnSoundPlayed;

    private Story _inkStory;
    private UnityEngine.TextAsset _inkJson;
    private StoryParser _storyParser = new();
    private AIManager _aiManager;
    private TalkManager _talkManager;

    private PlayerBehavior _player; // TODO: rename

    private BarrierSynchronizer _synchronizer = new BarrierSynchronizer();

    [Inject]
    public void Construct(DialoguesInstaller dialoguesInstaller)
    {
        _inkJson = dialoguesInstaller.inkJson;
    }

    private void Awake()
    {
        _inkStory = new Story(_inkJson.text); // TO DO: can use different stories and reseting them dynamically
        _aiManager = FindObjectOfType<AIManager>();
        _talkManager = FindObjectOfType<TalkManager>();
        _player = FindObjectOfType<PlayerBehavior>();
    }

    void OnEnable()
    {
        BarrierSynchronizer.OnWaitEnded += ToldStoryAndContinue;
    }

    void OnDisable()
    {
        BarrierSynchronizer.OnWaitEnded -= ToldStoryAndContinue;
    }

    private void ToldStoryAndContinue()
    {
        ToldStory();
        ContinueStory();
    }

    public void ChangeVariableState(string varName, string value)
    {
        _inkStory.variablesState[varName] = value;
    }
    public void ChangeVariableState(string varName, int value)
    {
        _inkStory.variablesState[varName] = value;
    }

    public void AddItem(string name, int count)
    {
        OnItemReceived.Invoke(name, count);
    }

    private void BindAIFunctionality()
    {
        _inkStory.BindExternalFunction("AIGenerateText", (string varName, string prompt, int maxTokens) =>
        {
            float temperature = (float)_inkStory.variablesState["ai_temperature"];
            Debug.Log($"AI generating {varName} with {prompt}, {maxTokens}, {temperature}");
            _synchronizer.AddProcessingVariable(varName);
            _aiManager.GenerateText(
                prompt,
                (string value) =>
                {
                    ChangeVariableState(varName, value);
                    _synchronizer.RemoveProcessingVariable(varName);
                },
                maxTokens,
                temperature
            );
        });

        _inkStory.BindExternalFunction("AIAnswer", (string varName, string system, string prompt, int maxTokens) =>
        {
            float temperature = (float)_inkStory.variablesState["ai_temperature"];
            Debug.Log($"AI answering {varName} with system: {system}, prompt: {prompt}, {maxTokens}, {temperature}");
            _synchronizer.AddProcessingVariable(varName);
            _aiManager.Answer(
                system,
                prompt,
                (string value) =>
                {
                    ChangeVariableState(varName, value);
                    _synchronizer.RemoveProcessingVariable(varName);
                },
                maxTokens,
                temperature
            );
        });

        _inkStory.BindExternalFunction("AITalk", (string system, int maxTokens) =>
        {
            Debug.Log($"AI talk with system: {system}, tokens: {maxTokens}");
            _talkManager.StartAITalk(
                system,
                maxTokens,
                _storyParser.GetCurrentAffects(),
                _storyParser.GetMayReceiveItems());
        });

        _inkStory.BindExternalFunction("AIGenerateImage", (string varName, string prompt, int w, int h) =>
        {
            string style = (string)_inkStory.variablesState["ai_style"];
            Debug.Log($"AI generating image {varName} with prompt: {prompt}, {w}, {h}");
            _aiManager.GenerateImage(
                prompt,
                w,
                h,
                style,
                varName
            );
        });
    }

    private void BindUtilsFunctionality()
    {
        _inkStory.BindExternalFunction("GetChoice", (string choices, int choice) =>
        {
            Debug.Log($"GetChoice!!!!! {choices} ; {choice}");
            return TextProcessor.GetChoice(choices, choice);
        });
    }

    private void BindInventoryFunctionality()
    {
        _inkStory.BindExternalFunction("AddToInventory", (string itemName, int count) =>
        {
            Debug.Log($"AddToInventory {count} {itemName}");
            return _player.AddItem(itemName, count);
        });

        _inkStory.BindExternalFunction("RemoveFromInventory", (string itemName, int count) =>
        {
            Debug.Log($"RemoveFromInventory {count} {itemName}");
            return _player.RemoveItem(itemName, count);
        });

        _inkStory.BindExternalFunction("HowManyItems", (string itemName) =>
        {
            Debug.Log($"ItemsNumInInventory {itemName}");
            return _player.HowManyItems(itemName);
        });
    }

    private void BindSoundFunctionality()
    {
        _inkStory.BindExternalFunction("PlaySound", (string name) =>
        {
            Debug.Log($"PlaySound {name}");
            OnSoundPlayed?.Invoke(name);
        });
    }

    private void BindPlayerPrefsFunctionality()
    {
        _inkStory.BindExternalFunction("SetInt", (string name, int value) =>
        {
            PlayerPrefs.SetInt(name, value);
        });
        _inkStory.BindExternalFunction("SetFloat", (string name, float value) =>
        {
            PlayerPrefs.SetFloat(name, value);
        });
        _inkStory.BindExternalFunction("SetString", (string name, string value) =>
        {
            PlayerPrefs.SetString(name, value);
        });
        _inkStory.BindExternalFunction("GetInt", (string name, int defaultValue) =>
        {
            return PlayerPrefs.GetInt(name, defaultValue);
        });
        _inkStory.BindExternalFunction("GetFloat", (string name, float defaultValue) =>
        {
            return PlayerPrefs.GetFloat(name, defaultValue);
        });
        _inkStory.BindExternalFunction("GetString", (string name, string defaultValue) =>
        {
            return PlayerPrefs.GetString(name, defaultValue);
        });
    }
    private void Start()
    {
        StartStory();
    }

    private void StartStory()
    {
        BindAIFunctionality();
        BindInventoryFunctionality();
        BindSoundFunctionality();
        BindPlayerPrefsFunctionality();
        BindUtilsFunctionality();
        OnStoryStarted?.Invoke();
        ContinueStory();
    }

    // have _inkStory.Continue();
    public void ContinueStory()
    {
        if (_inkStory.canContinue)
        {
            _inkStory.Continue();
            _storyParser.UpdateCurrentTags(_inkStory.currentTags);
            _storyParser.UpdateCurrentText(_inkStory.currentText);
            List<string> BlockingNames = _storyParser.GetBlockingNames(_inkStory.currentTags);
            if (BlockingNames.Count > 0)
            {
                foreach (var name in BlockingNames)
                {
                    Debug.Log("Will wait for var " + name);
                    _synchronizer.AddBlockingVariable(name);
                }
                if (_synchronizer.IsBlocked())
                {
                    _synchronizer.Barrier();
                    return;
                }
            }
            Debug.Log("No blocking variables currently");
            ToldStory();
        }
        else
        {
            if (_inkStory.currentChoices.Count() == 0)
            {
                Debug.Log("Story ended.");
                OnStoryEnded?.Invoke();
            }
            else
            {
                OnChoicesAppeared?.Invoke(_inkStory.currentChoices);
            }
        }
    }

    // doesnt have _inkStory.Continue();
    public void ToldStory()
    {
        Debug.Log("Start tolding story");
        if (_storyParser.IsCharactersReset())
        {
            Debug.Log("Characters reset.");
            OnCharactersReset?.Invoke(_storyParser.GetCurrentCharacterNames());
        }

        if (_storyParser.IsBackgroundChanged())
        {
            Debug.Log($"Background changed to {_storyParser.GetCurrentBackGround()}");
            OnBackgroundChanged?.Invoke(_storyParser.GetCurrentBackGround());
        }

        OnCharacterSaid?.Invoke(
            new Replica(
                _storyParser.GetCurrentSpeaker(),
                _storyParser.GetCurrentText(),
                (TalkAnimations.EmotionState)Convert.ToInt32(_storyParser.GetCurrentEmotion())
            )
        );

        OnChoicesAppeared?.Invoke(_inkStory.currentChoices);
        if (!_inkStory.canContinue && _inkStory.currentChoices.Count() == 0)
        {
            Debug.Log("Story ended.");
            OnStoryEnded?.Invoke();
        }
    }

    public void ChooseChoiceIndex(int choiceIndex)
    {
        Debug.Log($"On phrase \"{_inkStory.currentText}\" chosen story index {choiceIndex}");
        _inkStory.ChooseChoiceIndex(choiceIndex);
        OnChoiceChosen.Invoke();
        ContinueStory();
    }

}
