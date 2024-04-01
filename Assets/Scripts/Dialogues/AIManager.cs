using System;
using System.Collections;
using System.Collections.Generic;
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

    public static event Action OnAITalkStarted;
    public static event Action OnAITalkStoped;

    public static event Action<string> OnAITalkAnswered;

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

}
