using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using System.Collections.Generic;
using System;
using System.Text;
using System.Linq;

public class ServerCommunication : MonoBehaviour
{
    [SerializeField] private string serverURL = "http://";

    public class Message
    {
        public Message(string Role, string Content)
        {
            role = Role;
            content = Content;
        }
        public string role;
        public string content;
    }

    public void SendRequestToServer(WWWForm form, Action<string> callback)
    {
        StartCoroutine(SendMessage(form, callback));
    }

    static public string ToJSON(List<Message> messages)
    {
        string jsonMessages = "[";
        for (int i = 0; i < messages.Count - 1; ++i)
        {
            string jsonMessage = JsonUtility.ToJson(messages[i]);
            jsonMessages += jsonMessage + ", ";
        }
        jsonMessages += JsonUtility.ToJson(messages[messages.Count - 1]) + "]";
        Debug.Log(jsonMessages);
        return jsonMessages;
    }

    IEnumerator SendMessage(WWWForm form, Action<string> callback)
    {
        using (UnityWebRequest www = UnityWebRequest.Post(serverURL, form))
        {
            www.timeout = 10; //sec
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                string response = www.downloadHandler.text;
                Debug.Log("Server response: " + response);
                callback(response);
            }
            else
            {
                Debug.Log(www.error);
                callback("заглушка"); // TO DO: return null and then handle it
            }
        }
    }

}

