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
        using (UnityWebRequest www = UnityWebRequest.Post(serverURL, form))
        {
            www.timeout = 10; //sec
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

