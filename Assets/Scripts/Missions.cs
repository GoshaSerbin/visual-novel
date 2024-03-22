using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missions : MonoBehaviour
{
    private ServerCommunication _server;

    private void callback(string response)
    {
        Debug.Log(response);
    }
    // Start is called before the first frame update
    void Start()
    {
        _server = FindObjectOfType<ServerCommunication>(); // must be single on scene

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
