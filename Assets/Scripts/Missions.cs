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

    private void callback(string response)
    {
        //must check if not null
        Debug.Log(response.Length);
        Debug.Log(response);
    }
    // Start is called before the first frame update
    void Start()
    {
        _server = FindObjectOfType<ServerCommunication>(); // must be single on scene
        _inputField = FindObjectOfType<TMP_InputField>(); // must be single
        if (_inputField)
        {
            Debug.Log("Find!!!!");
        }
    }

    public void HandleAnswer()
    {
        string answer = _inputField.text;
        if (answer == "")
        {
            return;
        }
        WWWForm form = new WWWForm();

        var message1 = new ServerCommunication.Message("system", "Ты безумен");
        var message2 = new ServerCommunication.Message("user", "Напиши историю");

        var messages = new List<ServerCommunication.Message>() { message1, message2 };

        string jsonMessages = "[";
        for (int i = 0; i < messages.Count - 1; ++i)
        {
            string jsonMessage = JsonUtility.ToJson(messages[i]);
            jsonMessages += jsonMessage + ", ";
        }
        jsonMessages += JsonUtility.ToJson(messages[messages.Count - 1]) + "]";
        Debug.Log(jsonMessages);
        form.AddField("message", jsonMessages);
        form.AddField("max_tokens", 50);
        _server.SendRequestToServer(form, callback);

    }
}
