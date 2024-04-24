using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using Zenject;

[RequireComponent(typeof(ServerCommunication))]
public class AIManager : MonoBehaviour
{
    private ServerCommunication _server;

    private void Awake()
    {
        _server = GetComponent<ServerCommunication>();
    }
    private WWWForm GetWWWForm(List<ServerCommunication.Message> messages, int maxTokens, float temperature)
    {
        WWWForm form = new();
        string jsonMessages = ServerCommunication.ToJSON(messages);
        form.AddField("message", jsonMessages);
        form.AddField("max_tokens", maxTokens);
        form.AddField("temperature", Convert.ToString(temperature));
        return form;
    }

    private WWWForm GetImageWWWForm(string prompt, int width, int height, string style)
    {
        WWWForm form = new();
        form.AddField("prompt", prompt);
        form.AddField("width", width);
        form.AddField("height", height);
        form.AddField("style", style);
        return form;
    }

    // public static event Action<string> OnAIRecievedItem;
    // public static event Action<string> OnAIAffectedStory;

    public void GenerateText(string prompt, Action<string> callback, int maxTokens, float temperature)
    {
        prompt = prompt.Trim('\n');

        var messages = new List<ServerCommunication.Message>
                    {
                        new("user", prompt),
                    };
        var form = GetWWWForm(messages, maxTokens, temperature);
        _server.SendRequestToServer(form, callback);
        Debug.Log("AIManager sent request to server");
    }

    public void GenerateText(List<ServerCommunication.Message> messages, Action<string> callback, int maxTokens, float temperature)
    {
        var form = GetWWWForm(messages, maxTokens, temperature);
        _server.SendRequestToServer(form, callback);
        Debug.Log("AIManager sent request to server");
    }
    public void Answer(string system, string question, Action<string> callback, int maxTokens, float temperature)
    {
        question = question.TrimEnd('\n');
        Debug.Log($"AIManager started answering: {question}");

        var messages = new List<ServerCommunication.Message>
            {
                new("system", system),
                new("user", question),
            };

        var form = GetWWWForm(messages, maxTokens, temperature);

        _server.SendRequestToServer(form, callback);
        Debug.Log("AIManager sent request to server");
    }
    public void Show(string prompt, Action<Sprite> callback, int width = 1280, int height = 720, string style = "DEFAULT")
    {
        prompt = prompt.TrimEnd('\n');
        Debug.Log($"AIManager started showing: {prompt}");
        var form = GetImageWWWForm(prompt, width, height, style);
        _server.SendImageRequestToServer(form, callback);
        Debug.Log("AIManager sent image request to server");
    }

    public void IsReceived(string item, string npcAnswer, Action callback)
    {

        var systemMsg = new ServerCommunication.Message("system", "Персонаж в игре сказал игроку следующую фразу: \"" + npcAnswer + "\". Тебе нужно отвечать \"Да\" или \"Нет\" на вопросы, исходя из его фразы.");

        var messages = new List<ServerCommunication.Message>
            {
                systemMsg,
                new("user", "Следует ли напрямую из фразы, что игрок получил от персонажа предмет \"" + item + "\"?"),
            };
        var form = GetWWWForm(messages, 400, 0);
        _server.SendRequestToServer(form, (string response) =>
        {
            Debug.Log("ai answer to get item: " + response);
            if (response.StartsWith('Д'))
            {
                callback();
                // OnAIRecievedItem.Invoke(item);
            }
        });
    }

    public void IsAffected(string varName, string npcAnswer, string varDescription, Action callback)
    {

        var systemMsg = new ServerCommunication.Message("system", "Персонаж в игре сказал игроку следующую фразу: \"" + npcAnswer + "\". Тебе нужно отвечать \"Да\" или \"Нет\" на вопросы, исходя из его фразы.");

        var messages = new List<ServerCommunication.Message>
            {
                systemMsg,
                new("user", "Следует ли напрямую из фразы, что произошло событие \"" + varDescription + "\"?"),
            };
        var form = GetWWWForm(messages, 400, 0);
        _server.SendRequestToServer(form, (string response) =>
        {
            Debug.Log($"ai answer to affected story {varName}: {response}");
            if (response.Contains('Д'))
            {
                // OnAIAffectedStory.Invoke(varName);
                callback();
            }
        });
    }

    public void SaveSpriteToPNG(Sprite sprite, string name)
    {
        Texture2D texture = sprite.texture;
        byte[] bytes = texture.EncodeToPNG();

        string filePath = Application.persistentDataPath + "/" + name + ".png";
        File.WriteAllBytes(filePath, bytes);

        Debug.Log("Sprite saved to: " + filePath);
    }

    public void GenerateImage(string prompt, int w, int h, string style, string name)
    {
        prompt = prompt.Trim('\n');
        var form = GetImageWWWForm(prompt, w, h, style);
        _server.SendImageRequestToServer(form, (Sprite sprite) =>
        {
            if (sprite == null)
            {
                Debug.LogError("sprite is null");
            }
            else
            {
                SaveSpriteToPNG(sprite, name);
            }
        });
        Debug.Log("AIManager sent image request to server");
    }

    public static Sprite LoadSpriteFromPNG(string name)
    {
        string filePath = Application.persistentDataPath + "/" + name + ".png";
        byte[] bytes;
        try
        {
            bytes = File.ReadAllBytes(filePath);
        }
        catch (Exception e)
        {
            Debug.LogError(e);
            return null;
        }

        Texture2D texture = new Texture2D(2, 2);
        texture.LoadImage(bytes);

        Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));

        return sprite;
    }

}
