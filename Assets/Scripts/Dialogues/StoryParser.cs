using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Ink.Parsed;
using UnityEngine;

// This class is responsible for parsing inkStory (mainly tags) and understanding what is going on.
public class StoryParser
{
    public Dictionary<string, string> _currentTags = new Dictionary<string, string>(){
            {"speaker", ""},
            {"background", ""},
            {"reset_characters", ""},
            {"may_receive_items", ""}
        };
    public Dictionary<string, string> _previousTags;
    private readonly List<string> _validTags = new List<string>
    {
        "speaker",
        "emotion",
        "barrier",
        "background",
        "system",
        "max_tokens",
        "reset_characters",
        "may_receive_items",
        "may_affect",
    };

    private Dictionary<string, string> _currentAffects = new Dictionary<string, string>(); // {eventDescription, varName}
    private List<string> _currentItems = new List<string>();
    private List<string> _currentBlockingNames = new List<string>();
    public bool IsCharactersReset()
    {
        return _currentTags["reset_characters"] != _previousTags["reset_characters"];
    }

    public void Save()
    {
        PlayerPrefs.SetString("CurrentBackground", GetCurrentBackGround());
        PlayerPrefs.SetString("CurrentText", GetCurrentText());
        PlayerPrefs.SetString("CurrentSpeaker", GetCurrentSpeaker());
        PlayerPrefs.SetString("CurrentCharacters", _currentTags["reset_characters"]);
    }

    public void Load()
    {
        _currentTags["background"] = PlayerPrefs.GetString("CurrentBackground");
        _currentText = PlayerPrefs.GetString("CurrentText");
        _currentTags["speaker"] = PlayerPrefs.GetString("CurrentSpeaker");
        _currentTags["reset_characters"] = PlayerPrefs.GetString("CurrentCharacters");
        _previousTags = new Dictionary<string, string>(_currentTags);
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
        _currentTags.Remove("may_receive_items");
        _currentTags.Remove("may_affect");
        _currentTags.Remove("barrier");
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

        if (_currentTags.ContainsKey("may_receive_items"))
        {
            string[] items = _currentTags["may_receive_items"].Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < items.Count(); ++i)
            {
                _currentItems.Add(items[i].Trim(' '));
                Debug.Log("Can receive item " + items[i]);
            }
        }

        if (_currentTags.ContainsKey("barrier"))
        {
            string[] names = _currentTags["barrier"].Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < names.Count(); ++i)
            {
                _currentBlockingNames.Add(names[i].Trim(' '));
                Debug.Log("parsed blocking var " + names[i]);
            }
        }

    }

    private string _currentText;
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
        if (_currentTags.ContainsKey("emotion"))
        {
            return _currentTags["emotion"];
        }
        else
        {
            return "0";
        }
    }
}
