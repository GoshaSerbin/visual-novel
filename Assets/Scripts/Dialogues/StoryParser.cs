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
    public bool IsCharactersReset()
    {
        return _currentTags["reset_characters"] != _previousTags["reset_characters"];
    }

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

    // only this getter can be called without UpdateTags
    public List<string> GetBlockingNames(List<string> tags)
    {
        var BlockingNames = new List<string>();
        foreach (string tag in tags)
        {
            if (tag.StartsWith("barrier"))
            {
                string[] parts = tag.Split(':', 2);
                string[] names = parts[1].Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                for (int i = 0; i < names.Count(); ++i)
                {
                    BlockingNames.Add(names[i].Trim(' '));
                    Debug.Log("parsed blocking var " + names[i]);
                }
            }
        }

        return BlockingNames;
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
    }
    public void UpdateCurrentTags(List<string> tags)
    {
        _previousTags = new Dictionary<string, string>(_currentTags);
        _currentAffects.Clear();// = new Dictionary<string, string>(); // not new because talkmanager use reference to it
        _currentItems.Clear();// = new List<string>();


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
