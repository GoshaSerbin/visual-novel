using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Text;
using System.Linq;
public class CharacterAI : MonoBehaviour
{

    [TextArea(3, 10)][SerializeField] private string _characterDescription = "";
    [SerializeField] private float _talkativeness = 0.5f;
    private int _maxTokens;
    private ServerCommunication _server;

    // Start is called before the first frame update
    void Start()
    {
        _talkativeness = System.Math.Clamp(_talkativeness, 0, 1);
        _server = FindObjectOfType<ServerCommunication>(); // must be single on scene
        _maxTokens = _characterDescription.Length + Convert.ToInt32(150 * _talkativeness);
    }

    public void Ask(string phrase, Action<string> callback)
    {
        WWWForm form = new WWWForm();

        var message1 = new ServerCommunication.Message("system", _characterDescription);
        var message2 = new ServerCommunication.Message("user", phrase);

        var messages = new List<ServerCommunication.Message>() { message1, message2 };

        string jsonMessages = ServerCommunication.ToJSON(messages);
        form.AddField("message", jsonMessages);
        form.AddField("max_tokens", _maxTokens);
        form.AddField("temperature", 1);
        _server.SendRequestToServer(form, callback);
    }

}
