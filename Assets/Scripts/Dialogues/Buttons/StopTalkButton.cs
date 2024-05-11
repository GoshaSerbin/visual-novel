using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class StopTalkButton : MonoBehaviour
{
    void Start()
    {
        var stopTalkButton = GetComponent<Button>();
        var talkManager = FindObjectOfType<TalkManager>();
        stopTalkButton.onClick.AddListener(() =>
        {
            FindObjectOfType<AudioManager>()?.Play("ButtonClick");
            talkManager.StopAITalk();
        });
    }
}
