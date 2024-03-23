using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Missions : MonoBehaviour
{
    private ServerCommunication _server;
    private TMP_InputField _inputField;

    [SerializeField] private GameObject _storyPanel; // TO DO: replace serializefield with zenject
    [SerializeField] private TextMeshProUGUI _storyText;

    public void Construct(DialoguesInstaller dialoguesInstaller)
    {
        //zenject
    }

    private void RequestCallback(string response)
    {
        //must check if not null
        if (response == null)
        {
            Debug.LogError("server returned error");
            return;
        }
        Debug.Log(response.Length);
        Debug.Log(response);
        _storyText.text = response;
    }
    // Start is called before the first frame update
    void Start()
    {
        _server = FindObjectOfType<ServerCommunication>(); // must be single on scene
        _inputField = FindObjectOfType<TMP_InputField>(); // must be single
    }

    public void HandleAnswer()
    {
        string answer = _inputField.text;
        if (answer == "")
        {
            return;
        }
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
        _server.SendRequestToServer(form, RequestCallback);
    }
}
