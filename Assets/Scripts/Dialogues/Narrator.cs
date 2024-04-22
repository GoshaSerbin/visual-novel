using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ink.Runtime;
using Zenject;
using System;
using System.Linq;
using System.Globalization;
using Inventory;

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
    public static event Action<Sprite> OnBackgroundSpriteChanged;

    public static event Action<Replica> OnCharacterSaid;
    public static event Action OnStoryStarted;
    public static event Action OnStoryEnded;

    public static event Action<List<Choice>> OnChoicesAppeared;
    public static event Action<string[]> OnCharactersReset;
    public static event Action<string> OnStoryContinued;
    public static event Action OnChoiceChosen;
    public static event Action<string, int> OnItemReceived;
    public static event Action OnStoryAffected;

    private Story _inkStory;
    private UnityEngine.TextAsset _inkJson;
    private StoryParser _storyParser = new();
    private AIManager _aiManager;
    private TalkManager _talkManager;

    private InventoryController _inventory;

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
        _inventory = FindObjectOfType<InventoryController>(); // TO DO: can use different inventories and reseting them dynamically
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
            _aiManager.GenerateText(
                prompt,
                (string value) => { ChangeVariableState(varName, value); },
                maxTokens,
                temperature
            );
        });

        _inkStory.BindExternalFunction("AIAnswer", (string varName, string system, string prompt, int maxTokens) =>
        {
            float temperature = (float)_inkStory.variablesState["ai_temperature"];
            Debug.Log($"AI answering {varName} with system: {system}, prompt: {prompt}, {maxTokens}, {temperature}");
            _aiManager.Answer(
                system,
                prompt,
                (string value) => { ChangeVariableState(varName, value); },
                maxTokens,
                temperature
            );
        });

        _inkStory.BindExternalFunction("AITalk", (string system, int maxTokens) =>
        {
            Debug.Log($"AI talk with system: {system}, tokens: {maxTokens}");
            Debug.Log("TRANSMIT ");
            foreach (var s in _inkStory.currentTags)
            {
                Debug.Log("s " + s);
            }
            _talkManager.StartAITalk(
                system,
                maxTokens,
                _storyParser.GetCurrentAffects(),
                _storyParser.GetMayReceiveItems());
        });

        _inkStory.BindExternalFunction("AIGenerateImage", (string varName, string prompt, int h, int w) =>
        {
            string style = (string)_inkStory.variablesState["ai_style"];
            Debug.Log($"AI generating image {varName} with prompt: {prompt}, {h}, {w}");
            _aiManager.GenerateImage(
                prompt,
                w,
                h,
                style,
                varName
            );
        });

    }

    private void BindInventoryFunctionality() // TO DO
    {
        _inkStory.BindExternalFunction("AddToInventory", (string itemName, int count) =>
        {
            Debug.Log($"AddToInventory {count} {itemName}");
            _inventory.AddItem(itemName);
            // return _inventory.AddItem(itemName);
            // OnItemRecieved?.Invoke(itemName, count);
        });

        _inkStory.BindExternalFunction("RemoveFromInventory", (string itemName, int count) =>
        {
            Debug.Log($"RemoveFromInventory {count} {itemName}");
            return _inventory.RemoveItem(itemName, count);
            // OnItemRemoved?.Invoke(itemName, count);
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
        OnStoryStarted?.Invoke();
        ContinueStory();
    }

    public void ContinueStory()
    {
        if (_inkStory.canContinue)
        {
            _inkStory.Continue();
            _storyParser.UpdateCurrentTags(_inkStory.currentTags);
            _storyParser.UpdateCurrentText(_inkStory.currentText);

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
            // OnStoryContinued?.Invoke(_inkStory.currentChoices);
            // if (_currentTags["AI"] != "TALK")
            // {
            //     Debug.Log("display choices");
            //     OnStoryContinued.Invoke(_inkStory.currentChoices);
            // }
            if (_inkStory.canContinue && true) // && not ai talk or what ever
            {
                // OnStoryCanContinue?.Invoke();
            }
            else // if choices appeared
            {
                // OnStoryCanNotContinue?.Invoke();/////// blyat game over
            }
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

    public void ChooseChoiceIndex(int choiceIndex)
    {
        Debug.Log($"On phrase \"{_inkStory.currentText}\" chosen story index {choiceIndex}");
        _inkStory.ChooseChoiceIndex(choiceIndex);
        OnChoiceChosen.Invoke();
        ContinueStory();
    }

}
