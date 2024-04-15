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


    private string _characterDescription;

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

    public void TalkWith(string characterDescription) // TO DO: Add history
    {
        _characterDescription = characterDescription;
        StartAITalk();

    }

    private void StartAITalk()
    {
        Debug.Log("Start AI Talk!");
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
        Answer(
            phrase,
            _characterDescription,
            (string answer) =>
                {
                    if (answer == null)
                    {
                        Debug.LogError("server returned error");
                        return;
                    }
                    OnAITalkAnswered?.Invoke(TextProcessor.PostProccess(answer));

                    _inputField.enabled = true;
                    _inputField.ActivateInputField();
                },
            100);
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

    public void Show(string prompt, Action<Sprite> callback, int width = 1024, int height = 1024, string style = "DEFAULT")
    {
        prompt = prompt.TrimEnd('\n');
        Debug.Log($"AIManager started showing: {prompt}");
        var form = GetImageWWWForm(prompt, width, height, style);
        _server.SendImageRequestToServer(form, callback);
        Debug.Log("AIManager sent image request to server");
    }

    public void IsRecieved(string item, string npcAnswer)
    {

        var systemMsg = new ServerCommunication.Message("system", "NPC в игре сказал следующую фразу: \"" + npcAnswer + "\". Тебе будут называть названия предметов, которые игрок может получить от персонажа после данной фразы. Твоя задача - отвечать \"Да\" или \"Нет\" в зависимости от того получил ли в игрок указанный предмет от NPC. Отвечай да, ТОЛЬКО если получение предмета НАПРЯМУЮ следует из фразы, иначе говори Нет");

        var messages = new List<ServerCommunication.Message>
            {
                systemMsg,
                new("user", "игрок получил " + item + "?"),
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

        var systemMsg = new ServerCommunication.Message("system", "NPC в игре сказал следующую фразу: " + npcAnswer + ". Тебе будут описывать события, которые могут произойти. Твоя задача - отвечать \"Да\" или \"Нет\" в зависимости от того произошло ли это событие, исходя из фразы. Отвечай да, только если это напрямую следует из фразы.");

        var messages = new List<ServerCommunication.Message>
            {
                systemMsg,
                new("user", varDescription),
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
