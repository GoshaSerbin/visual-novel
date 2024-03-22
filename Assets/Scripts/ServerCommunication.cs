using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using System.Collections.Generic;
using System;
using System.Text;

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

    public void SendRequestToServer(WWWForm form, Action<string> callback)
    {
        StartCoroutine(SendMessage(form, callback));
    }

    IEnumerator SendMessage(WWWForm form, Action<string> callback)
    {
        using (UnityWebRequest www = UnityWebRequest.Post(serverURL, form))
        {
            www.timeout = 10; //sec
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.Log(www.error);
                callback(null);
            }
            else
            {
                string response = www.downloadHandler.text;
                Debug.Log("Server response: " + response);
                callback(response);
            }
        }
    }

}

