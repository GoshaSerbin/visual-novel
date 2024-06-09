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
using UnityEngine.EventSystems;

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
    public static event Action OnStoryStartedFromSave;
    public static event Action OnStoryEnded;

    public static event Action<List<Choice>> OnChoicesAppeared;
    public static event Action<string[]> OnCharactersReset;
    public static event Action OnChoiceChosen;
    public static event Action<string, int> OnItemReceived;

    public static event Action<string> OnSoundPlayed;
    public static event Action<string> OnMusicPlayed;
    public static event Action OnMusicStoped;
    public static event Action OnSoundStoped;
    public static event Action<string> OnPlayerAsked;

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

    private bool fromSave = false;

    private void Awake()
    {
        _talkManager = FindObjectOfType<TalkManager>();
        _player = FindObjectOfType<PlayerBehavior>();
        string SavedJsonStory = PlayerPrefs.GetString(SceneManager.GetActiveScene().name, "");

        if (_inkJson == null)
        {
            _inkJson = Resources.Load<TextAsset>("Dialogues/Stories/" + SceneManager.GetActiveScene().name);
        }
        _inkStory = new Story(_inkJson.text);
        if (SavedJsonStory != "")
        {
            _inkStory.state.LoadJson(SavedJsonStory);
            string StoryParserState = PlayerPrefs.GetString("StoryParserState");
            Debug.Log(StoryParserState);
            Debug.Log("From Save!");
            fromSave = true;
        }
        else
        {
            fromSave = false;
        }
    }

    void OnEnable()
    {
        BarrierSynchronizer.OnWaitEnded += ContinueStory;
    }

    void OnDisable()
    {
        BarrierSynchronizer.OnWaitEnded -= ContinueStory;
    }

    public static event Action OnStorySaved;

    public void SaveStoryProgress()
    {
        OnStorySaved?.Invoke();
        string savedJson = _inkStory.state.ToJson();
        PlayerPrefs.SetString(SceneManager.GetActiveScene().name, savedJson);
        PlayerPrefs.SetString("SavedScene", SceneManager.GetActiveScene().name);
        _storyParser.Save();
        PlayerPrefs.SetInt("SavedStoryProgress", 1);
    }

    public void ChangeVariableState(string varName, string value)
    {
        _inkStory.variablesState[varName] = value;
    }
    //returns whether the value change its previous value
    public bool ChangeVariableState(string varName, int value)
    {
        int oldValue = (int)_inkStory.variablesState[varName];
        _inkStory.variablesState[varName] = value;
        return oldValue != value;
    }

    public void AddItem(string name, int count)
    {
        OnItemReceived.Invoke(name, count);
    }

    private void BindAIFunctionality()
    {
        Debug.Log(_aiManager);
        Debug.Log(_aiManager._server);
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
            Debug.Log(_aiManager);
            Debug.Log(_aiManager._server);
            string style = (string)_inkStory.variablesState["ai_style"];
            bool allowCensored = ((int)_inkStory.variablesState["ai_allow_censored_images"]) != 0;
            Debug.Log("Parsed allowCensored : " + allowCensored);
            Debug.Log($"AI generating image {varName} with prompt: {prompt}, {w}, {h}, {style}");
            _aiManager.GenerateImage(
                prompt,
                w,
                h,
                style,
                varName,
                allowCensored
            );
        });


        _inkStory.BindExternalFunction("AIChangeBackground", (string varName, string prompt, int w, int h) =>
        {
            string style = (string)_inkStory.variablesState["ai_style"];
            bool allowCensored = ((int)_inkStory.variablesState["ai_allow_censored_images"]) != 0;
            Debug.Log($"AIChangeBackground with prompt: {prompt}, {w}, {h}, {style}");
            _aiManager.GenerateImage(
                prompt,
                w,
                h,
                style,
                varName,
                allowCensored,
                () =>
                {
                    _storyParser.SetCurrentBackGround(varName);
                    OnBackgroundChanged?.Invoke(_storyParser.GetCurrentBackGround());
                }
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

        _inkStory.BindExternalFunction("PlaySound", (string name) =>
        {
            Debug.Log($"PlaySound {name}");
            OnSoundPlayed?.Invoke(name);
        });

        _inkStory.BindExternalFunction("StopSounds", () =>
        {
            OnSoundStoped?.Invoke();
        });
        _inkStory.BindExternalFunction("StopMusic", () =>
        {
            OnMusicStoped?.Invoke();
        });
        _inkStory.BindExternalFunction("PlayMusic", (string name) =>
        {
            OnMusicPlayed?.Invoke(name);
        });

        _inkStory.BindExternalFunction("AskPlayer", (string name) =>
        {
            Debug.Log($"PlayerAsk {name}");
            OnPlayerAsked?.Invoke(name);
        });

        _inkStory.BindExternalFunction("Fight", () =>
        {
            DisableAllThatNeededForAdditiveLoading();
            var lvlLoader = FindObjectOfType<LvlLoader>();
            var CombatScene = SceneManager.GetSceneByName("Combat");

            if (lvlLoader != null)
            {
                //lvlLoader.LoadScene("Combat", LoadSceneMode.Additive);
                StartCoroutine(AsyncSceneLoad("Combat"));
            }
            else
            {
                StartCoroutine(AsyncSceneLoad("Combat"));
            }
        });
    }

    private IEnumerator AsyncSceneLoad(string sceneName)
    {
        Scene currentScene = SceneManager.GetActiveScene();
        //you might show the Loading Screen UI here
        //start loading of new scene
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);


        //return here every frame until isDone is true meaning its finished loading
        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        //unload old scene or not
        // SceneManager.UnloadSceneAsync(currentScene);

        //or set the new scene active (you can still unload old one or keep it around IDK exactly your requirments
        Scene newScene = SceneManager.GetSceneByName(sceneName);

        if (SceneManager.SetActiveScene(newScene))
        {
            //scene acive and loaded
        }
        else
        {
            //scene not loaded yet
        }


        yield break;
    }

    [SerializeField] private Camera _cam;
    [SerializeField] private EventSystem _eventSys;
    [SerializeField] private AudioListener _listener;


    private void DisableAllThatNeededForAdditiveLoading()
    {
        _cam.enabled = false;
        _eventSys.enabled = false;
        _listener.enabled = false;
    }

    private void EnableAllThatNeededForAdditiveLoading()
    {
        _cam.enabled = true;
        _eventSys.enabled = true;
        _listener.enabled = true;
    }

    private void LoadNextScene()
    {
        //delete savings if it was => will start this scene from the begining if player came here after a while
        PlayerPrefs.SetString(SceneManager.GetActiveScene().name, "");

        string name = (string)_inkStory.variablesState["NEXT_SCENE_NAME"];

        var lvlLoader = FindObjectOfType<LvlLoader>();
        if (lvlLoader != null)
        {
            lvlLoader.LoadScene(name);
        }
        else
        {
            SceneManager.LoadScene(name, LoadSceneMode.Single);
        }
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
        _aiManager = AIManager.Instance;
        StartStory();
        SceneManager.sceneUnloaded += OnSceneUnloaded;
    }

    private void OnSceneUnloaded(Scene current)
    {
        if (current == SceneManager.GetSceneByName("Combat"))
        {
            EnableAllThatNeededForAdditiveLoading();
        }
    }
    private void StartStory()
    {
        BindAIFunctionality();
        BindInventoryFunctionality();
        BindPlayerPrefsFunctionality();
        BindUtilsFunctionality();
        OnStoryStarted?.Invoke();
        Debug.Log("StartingStory!!");
        if (fromSave)
        {
            OnStoryStartedFromSave?.Invoke();
            Debug.Log("fromSave");
            _storyParser.Load();
            ForcedDisplayUpdate();
            TellStory();
        }
        else
        {
            Debug.Log("not from save");
            ContinueStory();
        }
    }

    private void ForcedDisplayUpdate()
    {
        Debug.Log("ForcedDisplayUpdate!");
        OnCharactersReset?.Invoke(_storyParser.GetCurrentCharacterNames());
        Debug.Log(_storyParser.GetCurrentBackGround());
        OnBackgroundChanged?.Invoke(_storyParser.GetCurrentBackGround());
    }

    // have _inkStory.Continue(); 
    // ForcedDisplayUpdate - to update display after loading
    public void ContinueStory()
    {
        if (_inkStory.canContinue)
        {
            _inkStory.Continue();
            Debug.Log("current tags " + _inkStory.currentTags);
            Debug.Log("current text " + _inkStory.currentText);
            _storyParser.UpdateCurrentTags(_inkStory.currentTags);
            _storyParser.UpdateCurrentText(_inkStory.currentText);
            Debug.Log("No blocking variables currently");
            TellStory();
            List<string> BlockingNames = _storyParser.GetBlockingNames();
            if (BlockingNames.Count > 0)
            {
                foreach (var name in BlockingNames)
                {
                    Debug.Log("Will wait for var " + name);
                    _synchronizer.AddBlockingVariable(name);
                }
                _synchronizer.Barrier();
            }
        }
        else
        {
            if (_inkStory.currentChoices.Count() == 0)
            {
                Debug.Log("Story ended.");
                OnStoryEnded?.Invoke();
                LoadNextScene();
            }
            else
            {
                OnChoicesAppeared?.Invoke(_inkStory.currentChoices);
            }
        }
    }

    // doesnt have _inkStory.Continue();
    public void TellStory()
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

        Debug.Log(_storyParser.GetCurrentSpeaker() + " SAYS " + _storyParser.GetCurrentText());
        OnCharacterSaid?.Invoke(
            new Replica(
                _storyParser.GetCurrentSpeaker(),
                _storyParser.GetCurrentText(),
                (TalkAnimations.EmotionState)Convert.ToInt32(_storyParser.GetCurrentEmotion())
            )
        );

        OnChoicesAppeared?.Invoke(_inkStory.currentChoices);
        // if (!_inkStory.canContinue && _inkStory.currentChoices.Count() == 0)
        // {
        //     Debug.Log("Story ended.");
        //     OnStoryEnded?.Invoke();
        //     LoadNextScene();
        // }
    }

    public void ChooseChoiceIndex(int choiceIndex)
    {
        Debug.Log($"On phrase \"{_inkStory.currentText}\" chosen story index {choiceIndex}");
        _inkStory.ChooseChoiceIndex(choiceIndex);
        OnChoiceChosen.Invoke();
        ContinueStory();
    }

}
