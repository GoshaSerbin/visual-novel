using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// this class is responsible for talk interaction
public class TalkManager : MonoBehaviour
{
    // these will be uniqie for every startAITalk call
    private string _characterDescription;
    private int _maxTokensForSinglePhrase;
    private List<ServerCommunication.Message> _messageHistory = new();

    private Dictionary<string, string> _currentAffects = new();
    private List<string> _currentItems;


    public static event Action OnAITalkStarted;
    public static event Action OnAITalkStopped;
    public static event Action OnPlayerPhraseSent;
    public static event Action<string> OnCharacterAnswered;

    public static event Action OnStoryAffected;
    public static event Action<string, int> OnItemReceived;

    private AIManager _aiManager;
    private Narrator _narrator;

    void Awake()
    {
        _aiManager = FindObjectOfType<AIManager>();
        _narrator = FindObjectOfType<Narrator>();
    }

    // !!! Dictionary and list are passed by ref, so in the beggining they are empty, and then they will change during parsing tags.......
    public void StartAITalk(string characterDescription, int maxTokensForSinglePhrase, Dictionary<string, string> affects, List<string> items)
    {
        OnAITalkStarted.Invoke();
        Debug.Log("Start AI Talk!");
        _messageHistory = new();
        _characterDescription = characterDescription;
        _maxTokensForSinglePhrase = maxTokensForSinglePhrase;
        _currentAffects = affects;
        _currentItems = items;
        Debug.Log("currentAffects " + _currentAffects.Count());
        Debug.Log("currentItems " + _currentItems.Count());
        Debug.Log("maxTokensForSinglePhrase " + maxTokensForSinglePhrase);
    }

    public void SendPlayerPhrase(string playerPhrase)
    {
        if (playerPhrase == "")
        {
            return;
        }

        OnPlayerPhraseSent?.Invoke();
        GetCharacterPhrase(
            playerPhrase,
            OnGetCharacterPhraseCallback
        );
    }

    private void OnGetCharacterPhraseCallback(string answer)
    {
        if (answer == null)
        {
            Debug.LogError("server returned error");
            return;
        }
        _messageHistory.Add(new("assistant", answer));

        foreach (var (varName, eventDescription) in _currentAffects)
        {
            Debug.Log("Check varName " + varName);
            _aiManager.IsAffected(varName, answer, eventDescription, () =>
            {
                bool hasChanged = _narrator.ChangeVariableState(varName, 1);
                if (hasChanged)
                {
                    FindObjectOfType<AudioManager>()?.Play("Consequence");
                    OnStoryAffected?.Invoke();
                }
            });
        }
        foreach (var itemName in _currentItems)
        {
            Debug.Log("Check itemName " + itemName);
            _aiManager.IsReceived(itemName, answer, () =>
            {
                OnItemReceived?.Invoke(itemName, 1);
            });
        }
        OnCharacterAnswered?.Invoke(TextProcessor.PostProccess(answer));
    }

    private void GetCharacterPhrase(string question, Action<string> callback, float temperature = 1.2f)
    {
        question = question.Trim('\n');
        Debug.Log($"AIManager started answering: {question}");

        UpdateMessageHistory(question);
        var messages = GetUpdatedMessages(question);
        int maxTokens = _maxTokensForSinglePhrase * (1 + _messageHistory.Count() / 2);
        Debug.Log("max tokens " + maxTokens);

        _aiManager.GenerateText(messages, callback, maxTokens, temperature);
    }

    private void UpdateMessageHistory(string newQuestion)
    {
        _messageHistory.Add(new("user", newQuestion));
        if (_messageHistory.Count() > 3)
        {
            while (_messageHistory.Count() > 3)
            {
                _messageHistory.RemoveAt(0);
            }
        }
    }

    private List<ServerCommunication.Message> GetUpdatedMessages(string newQuestion)
    {
        var messages = new List<ServerCommunication.Message>
            {
                new("system", _characterDescription),
            };
        messages.InsertRange(1, _messageHistory);
        return messages;
    }



    public void StopAITalk()
    {
        OnAITalkStopped?.Invoke();
        _narrator.ContinueStory();
    }
}
