using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Missions : MonoBehaviour
{
    private ServerCommunication _server;
    private TMP_InputField _inputField;

    [SerializeField] private Button _startButton;

    [TextArea(3, 10)][SerializeField] private string generateMissionSystemText;
    [TextArea(3, 10)][SerializeField] private string generateMissionUserText;

    [SerializeField] private GameObject _storyPanel; // TO DO: replace serializefield with zenject
    [SerializeField] private TextMeshProUGUI _storyText;

    [SerializeField] private GameObject _choicePanel;
    [SerializeField] private GameObject _nonMissionPanel; // will not be active during mission

    public void Construct(DialoguesInstaller dialoguesInstaller)
    {
        //zenject
    }

    void Start()
    {
        _server = FindObjectOfType<ServerCommunication>(); // must be single on scene
        _inputField = FindObjectOfType<TMP_InputField>(); // must be single
        _choicePanel.SetActive(false);
    }

    public void StartMission()
    {
        _nonMissionPanel.SetActive(false);
        GenerateMission();
    }
    private void GenerateMission()
    {
        WWWForm form = new WWWForm();

        var message1 = new ServerCommunication.Message("system", generateMissionSystemText);
        var message2 = new ServerCommunication.Message("user", generateMissionUserText);
        var messages = new List<ServerCommunication.Message>() { message1, message2 };
        string jsonMessages = ServerCommunication.ToJSON(messages);

        form.AddField("message", jsonMessages);
        form.AddField("max_tokens", 800);
        form.AddField("temperature", 1);
        _server.SendRequestToServer(form, GenerateMissionCallBack);
    }

    private void GenerateMissionCallBack(string response)
    {
        //must check if not null
        if (response == null)
        {
            Debug.LogError("server returned error");
            return;
        }
        _storyText.text = response;
        Debug.Log("received mission:" + response);
        _choicePanel.SetActive(true);
    }
    public void HandleAnswer()
    {
        string answer = _inputField.text;
        if (answer == "")
        {
            return;
        }
        _choicePanel.SetActive(false);
        SendAnswer(answer);

    }
    private void SendAnswer(string answer)
    {
        WWWForm form = new WWWForm();

        var message1 = new ServerCommunication.Message("system", "Ты безумен");
        var message2 = new ServerCommunication.Message("user", "Напиши историю");

        var messages = new List<ServerCommunication.Message>() { message1, message2 };

        string jsonMessages = ServerCommunication.ToJSON(messages);
        form.AddField("message", jsonMessages);
        form.AddField("max_tokens", 50);
        form.AddField("temperature", 1);
        _server.SendRequestToServer(form, SendAnswerCallback);
    }
    private void SendAnswerCallback(string response)
    {
        //must check if not null
        if (response == null)
        {
            Debug.LogError("server returned error");
            return;
        }
        Debug.Log(response.Length);
        Debug.Log("received response" + response);
        string storyText = _storyText.text;
        string userAnswer = _inputField.text; // can be not what player sent to server
        _storyText.text = response;
        ReceiveReward(_storyText.text, userAnswer, response);
    }

    private void ReceiveReward(string story, string userAnswer, string response)
    {
        WWWForm form = new WWWForm();

        var message1 = new ServerCommunication.Message("system", "Определи награду");
        string request = "История: " + story + "\nОтвет: " + userAnswer + "\nРезультат:" + response;
        var message2 = new ServerCommunication.Message("user", request);

        var messages = new List<ServerCommunication.Message>() { message1, message2 };

        string jsonMessages = ServerCommunication.ToJSON(messages);
        form.AddField("message", jsonMessages);
        form.AddField("max_tokens", 50);
        form.AddField("temperature", 1);
        _server.SendRequestToServer(form, ReceiveRewardCallBack);
    }

    private void ReceiveRewardCallBack(string response)
    {

        // попытаться преобразовать в int
        Debug.Log("received reward" + response);
        _nonMissionPanel.SetActive(true);
    }
    // Start is called before the first frame update








}
