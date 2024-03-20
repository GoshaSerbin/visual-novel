// using System.Text.Json;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using System.Collections.Generic;

public class ServerCommunication : MonoBehaviour
{
    public string serverURL = "http://";
    public string messageToSend = "Hello, server!";

    public void SendRequestToServer()
    {
        StartCoroutine(SendMessage());
    }


    IEnumerator SendMessage()
    {
        WWWForm form = new WWWForm();
        form.AddField("message", messageToSend);
        Dictionary<string, string> dict = new();
        // string json = JsonSerializer.Serialize(dict);
        // Console.WriteLine(json);
        // form.AddField("prompt", )

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

