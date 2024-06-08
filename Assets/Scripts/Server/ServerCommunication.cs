using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using System.Collections.Generic;
using System;
using System.IO;

public class ServerCommunication : MonoBehaviour
{
    private string serverURL = "http://";

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
    [SerializeField] private TextAsset textFile;

    private void Start()
    {
        serverURL = textFile.text.Trim();
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

    public class ImageResponse
    {
        public ImageResponse(string Data, bool Censored)
        {
            data = Data;
            censored = Censored;
        }
        public string data;
        public bool censored;
    }

    static public ImageResponse FromJSON(string json)
    {
        ImageResponse response = JsonUtility.FromJson<ImageResponse>(json);
        return response;
    }

    IEnumerator SendMessage(WWWForm form, Action<string> callback)
    {
        using (UnityWebRequest www = UnityWebRequest.Post(serverURL + "/talk", form))
        {
            www.SetRequestHeader("Access-Control-Allow-Origin", "*");
            www.timeout = 20; //sec
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                string response = TextProcessor.PostProccess(www.downloadHandler.text);
                Debug.Log("Server response: " + response);
                callback(response);
            }
            else
            {
                Debug.Log(www.error);
                callback("ОШИБКА СЕРВЕРА. Обратитесь к администратору."); // TO DO: return null and then handle it
            }
        }
    }

    public void SendImageRequestToServer(WWWForm form, Action<Sprite, bool> callback)
    {
        StartCoroutine(GetSprite(form, callback));
    }

    IEnumerator GetSprite(WWWForm form, Action<Sprite, bool> callback)
    {
        using (UnityWebRequest www = UnityWebRequest.Post(serverURL + "/image", form))
        {
            www.timeout = 300; //sec
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("Server received image");
                ImageResponse response = FromJSON(www.downloadHandler.text);
                byte[] rawImage = System.Convert.FromBase64String(response.data);
                bool status = response.censored;
                Debug.Log("Image censored? " + response.censored);
                Texture2D texture = new Texture2D(1, 1);
                texture.LoadImage(rawImage);
                var sprite = Sprite.Create(texture, new Rect(0.0f, 0.0f, texture.width, texture.height), new Vector2(0.5f, 0.5f), 100.0f);
                callback(sprite, status);
            }
            else
            {
                Debug.Log(www.error);
                callback(null, false); // TO DO: return null and then handle it
            }
        }
    }

}

