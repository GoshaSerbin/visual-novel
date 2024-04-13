using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using System.Collections.Generic;
using System;

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
        using (UnityWebRequest www = UnityWebRequest.Post(serverURL + "/talk", form))
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

    public void SendImageRequestToServer(WWWForm form, Action<Sprite> callback)
    {
        StartCoroutine(GetSprite(form, callback));
    }

    IEnumerator GetSprite(WWWForm form, Action<Sprite> callback)
    {
        using (UnityWebRequest www = UnityWebRequest.Post(serverURL + "/image", form))
        {
            www.timeout = 100; //sec
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                byte[] rawImage = www.downloadHandler.data;
                Texture2D texture = new Texture2D(1, 1);
                texture.LoadImage(rawImage);
                var sprite = Sprite.Create(texture, new Rect(0.0f, 0.0f, texture.width, texture.height), new Vector2(0.5f, 0.5f), 100.0f);
                Debug.Log("Server received image");
                callback(sprite);
            }
            else
            {
                Debug.Log(www.error);
                callback(null); // TO DO: return null and then handle it
            }
        }
    }

}

