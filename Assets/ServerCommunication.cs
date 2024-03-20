using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using System.Collections.Generic;

public class ServerCommunication : MonoBehaviour
{
    public string serverURL = "http://";
    public string messageToSend = "Hello, server!";

    public class Message
    {
        public Message(string Role, string Text)
        {
            role = Role;
            text = Text;
        }
        public string role;
        public string text;
    }

    public void SendRequestToServer()
    {
        StartCoroutine(SendMessage());
    }


    IEnumerator SendMessage()
    {
        WWWForm form = new WWWForm();

        Message message1 = new Message("system", "text");
        Message message2 = new Message("user", "text");

        var messages = new List<Message>() { message1, message2 };

        string jsonMessages = "{";
        foreach (Message message in messages)
        {
            string jsonMessage = JsonUtility.ToJson(message);
            jsonMessages += jsonMessage;
        }
        jsonMessages += "}";
        // var messages = new List<Dictionary<string, string>>();
        // // TODO: add class for {role, text} ?
        // messages.Add(new Dictionary<string, string>() { { "role", "system" }, { "text", messageToSend } });

        // string jsonMessages = JsonSerializer.Serialize(messages);
        Debug.Log(jsonMessages);
        form.AddField("message", jsonMessages);
        using (UnityWebRequest www = UnityWebRequest.Post(serverURL, form))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.Log(www.error);
            }
            else
            {
                Debug.Log("Server response: " + www.downloadHandler.text);
            }
        }
    }

    void Start()
    {
        SendRequestToServer();
    }

}

