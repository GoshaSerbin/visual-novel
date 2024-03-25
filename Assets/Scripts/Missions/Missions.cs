using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class Missions : MonoBehaviour
{
    private ServerCommunication _server;

    private Button _startButton;

    [SerializeField]
    private TextAsset _missionGenerationForSystem;
    [SerializeField]
    private TextAsset _missionGenerationForUser;
    private GameObject _storyPanel;
    private TextMeshProUGUI _storyText;

    private GameObject _choicePanel;
    private TMP_InputField _inputField;

    private GameObject _nonMissionPanel; // will not be active during mission

    [Inject]
    public void Construct(MissionsInstaller missionsInstaller)
    {
        _storyPanel = missionsInstaller.storyPanel;
        _storyText = missionsInstaller.storyText;
        _choicePanel = missionsInstaller.choicePanel;
        _nonMissionPanel = missionsInstaller.nonMissionPanel;
        _inputField = missionsInstaller.inputField;
        _startButton = missionsInstaller.startButton;
    }

    void Start()
    {
        _server = FindObjectOfType<ServerCommunication>(); // must be single on scene
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

        // TO DO: store in json right away
        var message1 = new ServerCommunication.Message("system", _missionGenerationForSystem.text); // TO DO: check if not null?
        var message2 = new ServerCommunication.Message("user", _missionGenerationForUser.text);
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
        _inputField.enabled = true;
        _inputField.ActivateInputField();
    }
    public void HandleAnswer()
    {
        string answer = _inputField.text;
        if (answer == "")
        {
            return;
        }
        _choicePanel.SetActive(false);
        _inputField.enabled = false;
        SendAnswer(answer);

    }
    private void SendAnswer(string answer)
    {
        WWWForm form = new WWWForm();

        var message1 = new ServerCommunication.Message("system", "Тебе дают историю и действие игрока. Твоя задача - придумать продолжение истории. Если игрок поступает обдуманно и разумно, то его ждет успех, иначе - неудача. История должна быть короткой (1-2 предложения)");
        var message2 = new ServerCommunication.Message("user", "История: " + _storyText.text + "\nДействие игрока:" + answer);

        var messages = new List<ServerCommunication.Message>() { message1, message2 };

        string jsonMessages = ServerCommunication.ToJSON(messages);
        form.AddField("message", jsonMessages);
        form.AddField("max_tokens", 200);
        form.AddField("temperature", 1);
        _server.SendRequestToServer(form, SendAnswerCallback);
    }
    private void SendAnswerCallback(string storyResolution)
    {
        //must check if not null
        if (storyResolution == null)
        {
            Debug.LogError("server returned error");
            return;
        }
        Debug.Log(storyResolution.Length);
        Debug.Log("received response" + storyResolution);
        string storyText = _storyText.text;
        string userAnswer = _inputField.text;
        _storyText.text = storyResolution;
        ReceiveReward(storyResolution);
    }

    private void ReceiveReward(string storyResolution)
    {
        WWWForm form = new WWWForm();

        var message1 = new ServerCommunication.Message("system", "Определи награду, которую получит игрок в данной истории. Наградой должно быть число от -100 до 100, где -100 - большая неудача, 100 - большой успех, 0 - отсутствие результата. В ответ укажи только само число");
        string request = "История: " + storyResolution;
        var message2 = new ServerCommunication.Message("user", request);

        var messages = new List<ServerCommunication.Message>() { message1, message2 };

        string jsonMessages = ServerCommunication.ToJSON(messages);
        form.AddField("message", jsonMessages);
        form.AddField("max_tokens", 100);
        form.AddField("temperature", 1);
        _server.SendRequestToServer(form, ReceiveRewardCallBack);
    }

    private void ReceiveRewardCallBack(string reward)
    {
        //must check if not null
        if (reward == null)
        {
            Debug.LogError("server returned error");
            return;
        }
        // попытаться преобразовать в int
        Debug.Log("received reward " + reward);
        _nonMissionPanel.SetActive(true);
    }
    // Start is called before the first frame update








}
