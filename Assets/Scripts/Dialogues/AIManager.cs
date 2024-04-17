using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using Zenject;

[RequireComponent(typeof(ServerCommunication))]
public class AIManager : MonoBehaviour
{

    public bool isAITalking { get; private set; }
    private ServerCommunication _server;

    // TO DO: separate display from ai manager
    private GameObject _inputFieldPanel;
    private TMP_InputField _inputField;
    private Button _stopTalkButton;
    private Button _continueTalkButton;


    [Inject]
    public void Construct(DialoguesInstaller dialoguesInstaller) // TO DO: may be there is no in in zenject =)
    {
        _inputFieldPanel = dialoguesInstaller.inputFieldPanel;
        _inputField = dialoguesInstaller.inputField;
        _stopTalkButton = dialoguesInstaller.stopTalkButton;
        _continueTalkButton = dialoguesInstaller.continueTalkButton;
    }


    // these will be uniqie for every startAITalk call. TO DO: refactor it
    private string _characterDescription;
    private int _maxTokens;
    private List<ServerCommunication.Message> _messageHistory = new();
    private void Start()
    {
        isAITalking = false;
        _server = GetComponent<ServerCommunication>();
        _stopTalkButton.onClick.AddListener(StopAITalk);
        _continueTalkButton.onClick.AddListener(ContinueAITalk);
    }

    private WWWForm GetWWWForm(List<ServerCommunication.Message> messages, int maxTokens, float temperature)
    {
        WWWForm form = new();
        string jsonMessages = ServerCommunication.ToJSON(messages);
        form.AddField("message", jsonMessages);
        form.AddField("max_tokens", maxTokens);
        form.AddField("temperature", Convert.ToString(temperature));
        return form;
    }

    private WWWForm GetImageWWWForm(string prompt, int width, int height, string style)
    {
        WWWForm form = new();
        form.AddField("prompt", prompt);
        form.AddField("width", width);
        form.AddField("height", height);
        form.AddField("style", style);
        return form;
    }

    public static event Action OnAITalkStarted;
    public static event Action OnAITalkStoped;

    public static event Action<string> OnAITalkAnswered;
    public static event Action<string> OnAIRecievedItem;
    public static event Action<string> OnAIAffectedStory;


    public void TalkWith(string characterDescription, int maxTokens = 100) // TO DO: Add history
    {
        _characterDescription = characterDescription;
        _maxTokens = maxTokens;
        StartAITalk();

    }

    private void StartAITalk()
    {
        Debug.Log("Start AI Talk!");
        _messageHistory = new();
        isAITalking = true;
        OnAITalkStarted.Invoke();
        _inputFieldPanel.SetActive(true);
        _inputField.ActivateInputField();
    }

    public void ContinueAITalk()
    {
        string phrase = _inputField.text;
        if (phrase == "")
        {
            return;
        }

        _inputField.enabled = false;
        ContinueAnswer(
            phrase,
            (string answer) =>
                {
                    if (answer == null)
                    {
                        Debug.LogError("server returned error");
                        return;
                    }
                    _messageHistory.Add(new("assistant", answer));
                    OnAITalkAnswered?.Invoke(TextProcessor.PostProccess(answer));

                    _inputField.enabled = true;
                    _inputField.ActivateInputField();
                });
    }

    private void StopAITalk()
    {
        isAITalking = false;
        _inputFieldPanel.SetActive(false);
        OnAITalkStoped?.Invoke();
    }

    public void Describe(string prompt, Action<string> callback, int maxTokens, float temperature = 1)
    {
        prompt = prompt.TrimEnd('\n');
        Debug.Log($"AIManager started describing: {prompt}");

        var messages = new List<ServerCommunication.Message>
                    {
                        new("user", prompt),
                    };
        var form = GetWWWForm(messages, maxTokens, temperature);
        _server.SendRequestToServer(form, callback);
        Debug.Log("AIManager sent request to server");
    }


    private void ContinueAnswer(string question, Action<string> callback, float temperature = 1)
    {
        question = question.TrimEnd('\n');
        Debug.Log($"AIManager started answering: {question}");

        _messageHistory.Add(new("user", question));
        if (_messageHistory.Count() > 6)
        {
            _messageHistory.RemoveAt(0);
        }

        var messages = new List<ServerCommunication.Message>
            {
                new("system", _characterDescription),
            };
        messages.InsertRange(1, _messageHistory);

        var form = GetWWWForm(messages, _maxTokens * (1 + _messageHistory.Count() / 2), temperature);

        _server.SendRequestToServer(form, callback);
        Debug.Log("AIManager sent request to server");
    }

    public void Answer(string question, string characterDescription, Action<string> callback, int maxTokens, float temperature = 1) //TO DO: add List<ServerCommunication.Message> history
    {
        question = question.TrimEnd('\n');
        Debug.Log($"AIManager started answering: {question}");

        var messages = new List<ServerCommunication.Message>
            {
                new("system", characterDescription),
                new("user", question),
            };

        var form = GetWWWForm(messages, maxTokens, temperature);

        _server.SendRequestToServer(form, callback);
        Debug.Log("AIManager sent request to server");
    }
    public void Show(string prompt, Action<Sprite> callback, int width = 1280, int height = 720, string style = "DEFAULT")
    {
        prompt = prompt.TrimEnd('\n');
        Debug.Log($"AIManager started showing: {prompt}");
        var form = GetImageWWWForm(prompt, width, height, style);
        _server.SendImageRequestToServer(form, callback);
        Debug.Log("AIManager sent image request to server");
    }

    public void IsRecieved(string item, string npcAnswer)
    {

        var systemMsg = new ServerCommunication.Message("system", "Персонаж в игре сказал игроку следующую фразу: \"" + npcAnswer + "\". Тебе нужно отвечать \"Да\" или \"Нет\" на вопросы, исходя из его фразы.");

        var messages = new List<ServerCommunication.Message>
            {
                systemMsg,
                new("user", "Следует ли напрямую из фразы, что игрок получил от персонажа предмет \"" + item + "\"?"),
            };
        var form = GetWWWForm(messages, 400, 0);
        _server.SendRequestToServer(form, (string response) =>
        {
            Debug.Log("ai answer to get item: " + response);
            if (response.StartsWith('Д'))
            {
                OnAIRecievedItem.Invoke(item);
            }
        });
    }

    public void IsAffected(string varName, string npcAnswer, string varDescription)
    {

        var systemMsg = new ServerCommunication.Message("system", "Персонаж в игре сказал игроку следующую фразу: \"" + npcAnswer + "\". Тебе нужно отвечать \"Да\" или \"Нет\" на вопросы, исходя из его фразы.");

        var messages = new List<ServerCommunication.Message>
            {
                systemMsg,
                new("user", "Следует ли напрямую из фразы, что произошло событие \"" + varDescription + "\"?"),
            };
        var form = GetWWWForm(messages, 400, 0);
        _server.SendRequestToServer(form, (string response) =>
        {
            Debug.Log($"ai answer to affected story {varName}: {response}");
            if (response.Contains('Д'))
            {
                OnAIAffectedStory.Invoke(varName);
            }
        });
    }

}
