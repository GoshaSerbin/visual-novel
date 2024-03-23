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

    public string Decode(byte[] bytes, string encodingName)
    {
        Debug.Log(bytes.Count());
        Encoding encoding = Encoding.GetEncoding(encodingName);
        return encoding.GetString(bytes);
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
                string response = Decode(www.downloadHandler.data, "windows-1251");
                // string response = Encoding.UTF8.GetString(www.downloadHandler.nativeData);
                // string response = Encoding.UTF7.GetString(www.downloadHandler.data);
                // string response = Encoding.ASCII.GetString(www.downloadHandler.data);
                // string response = Encoding.UTF32.GetString(www.downloadHandler.data);
                Debug.Log("Server response: " + response);
                callback(response);
            }
        }
    }

}

