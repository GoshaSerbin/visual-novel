using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Ink.Parsed;
using UnityEngine;

// This class is responsible for parsing inkStory (mainly tags) and understanding what is going on.
public class StoryParser
{
    public string _currentText;
    private Dictionary<string, string> _currentTags = new Dictionary<string, string>(){
            {"speaker", ""},
            {"emotion", "0"},
            {"temperature", "1.2"},
            {"background", ""},
            {"reset_characters", ""},
            {"may_receive_items", ""}
        };
    private Dictionary<string, string> _previousTags;
    private readonly List<string> _validTags = new List<string>
    {
        "ai",
        "speaker",
        "emotion",
        "barrier",
        "background",
        "system",
        "max_tokens",
        "reset_characters",
        "may_receive_items",
        "may_affect",
        "temperature"
    };

    private Dictionary<string, string> _currentAffects = new Dictionary<string, string>(); // {eventDescription, varName}
    private List<string> _currentItems = new List<string>();
    private List<string> _currentBlockingNames = new List<string>();
    public bool IsCharactersReset()
    {
        return _currentTags["reset_characters"] != _previousTags["reset_characters"];
    }

    // public void Save()
    // {
    //     List<string> data = new();
    //     foreach (KeyValuePair<string, string> entry in _currentTags)
    //     {

    //         PlayerPrefs.SetString("STORYPARSER__" + entry.Key);
    //     }
    //     CurrentTags = _currentTags;
    //     PreviousTags = _previousTags;
    //     string jsonString = JsonSerializer.Serialize(dictionary);
    //     Console.WriteLine(jsonString);
    //     // string CurrentTags = _currentTags.ToString();
    //     string PreviouesTags = _previousTags.ToString();
    //     string[] result = { CurrentTags, PreviouesTags, _currentText };
    //     CurrentTags.ToDictionary();
    //     return data;
    // }

    // public void Load(string[] data)
    // {
    //     _currentTags = data[0].ToDictionary();
    //     string PreviouesTags = _previousTags.ToString();
    //     string[] result = { CurrentTags, PreviouesTags, _currentText };

    //     return result;
    // }

    public string[] GetCurrentCharacterNames()
    {
        string[] names = _currentTags["reset_characters"].Split(",");
        for (int i = 0; i < names.Count(); ++i)
        {
            names[i] = names[i].Trim();
        }
        return names;
    }

    public bool IsBackgroundChanged()
    {
        return _currentTags["background"] != _previousTags["background"];
    }

    public List<string> GetBlockingNames()
    {
        return _currentBlockingNames;
    }

    public string GetCurrentBackGround()
    {
        return _currentTags["background"];
    }

    public string GetCurrentSpeaker()
    {
        return _currentTags["speaker"];
    }

    public List<string> GetMayReceiveItems()
    {
        return _currentItems;
    }

    public Dictionary<string, string> GetCurrentAffects()
    {
        return _currentAffects;
    }

    // This tags are cleared on every update
    private void ResetUncacheableTags()
    {
        _currentTags["AI"] = "";
        _currentTags["max_tokens"] = "";
        _currentTags["may_receive_items"] = "";
        _currentTags["may_affect"] = "";
        _currentTags["barrier"] = "";
    }
    public void UpdateCurrentTags(List<string> tags)
    {
        _previousTags = new Dictionary<string, string>(_currentTags);
        _currentAffects.Clear();// = new Dictionary<string, string>(); // not new because talkmanager use reference to it
        _currentItems.Clear();// = new List<string>();
        _currentBlockingNames.Clear();


        ResetUncacheableTags();

        foreach (string tag in tags)
        {
            Debug.Log("parsing tag " + tag);
            string[] parts = tag.Split(':', 2);
            if (parts.Count() != 2)
            {
                Debug.LogError($"Parsed strange tag {tag}.");
                continue;
            }

            string key = parts[0].Trim();
            string value = parts[1].Trim();
            if (!_validTags.Contains(key))
            {
                Debug.LogError($"Parsed unknown tag {key} with value {value}.");
            }
            if (key == "may_affect")
            {
                string[] values = value.Split("=>");
                _currentAffects[values[1].Trim()] = values[0].Trim();
            }
            else
            {
                _currentTags[key] = value;
            }

        }

        Debug.Log("may_receive_items:" + _currentTags["may_receive_items"]);
        string[] items = _currentTags["may_receive_items"].Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
        for (int i = 0; i < items.Count(); ++i)
        {
            _currentItems.Add(items[i].Trim(' '));
            Debug.Log("Can receive item " + items[i]);
        }

        string[] names = _currentTags["barrier"].Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
        for (int i = 0; i < names.Count(); ++i)
        {
            _currentBlockingNames.Add(names[i].Trim(' '));
            Debug.Log("parsed blocking var " + names[i]);
        }
    }

    public void UpdateCurrentText(string text)
    {
        _currentText = text;
    }

    public string GetCurrentText()
    {
        return _currentText;
    }
    public string GetCurrentEmotion()
    {
        return _currentTags["emotion"];
    }
}
