using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharactersManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> _characters = new();
    private List<GameObject> _currentCharacters = new();

    [SerializeField] private GameObject _leftCharacterPanel;
    [SerializeField] private GameObject _rightCharacterPanel;


    public void ResetCharacters(string[] characterNames)
    {
        foreach (var character in _currentCharacters)
        {
            Destroy(character);

        }
        _currentCharacters = new();
        switch (characterNames.Length)
        {
            case 1:
                {
                    characterNames = new string[] { characterNames[0], "" };
                    break;
                }
            case 2:
                {
                    break;
                }
            default:
                Debug.LogError("There must be 2 characters on scene (1 may be none)");
                break;
        }
        if (characterNames[0] != "")
        {
            _currentCharacters.Add(Instantiate(GetCharacterByName(characterNames[0]), _leftCharacterPanel.transform));
        }
        if (characterNames[1] != "")
        {
            _currentCharacters.Add(Instantiate(GetCharacterByName(characterNames[1]), _rightCharacterPanel.transform));
        }
    }


    // public void AnimateCharacters(string speakerName, CharacterEmotion.EmotionState emotion)
    // {
    //     foreach (var character in _currentCharacters)
    //     {
    //         if (character.GetComponent<CharacterInfo>().CharacterNames.Contains(speakerName))
    //         {
    //             character.GetComponent<CharacterAnimations>().StartTalking();
    //             character.GetComponent<CharacterEmotion>().ChangeEmotion(emotion);
    //         }
    //         else
    //         {
    //             character.GetComponent<CharacterAnimations>().StopTalking();
    //         }
    //     }
    // }

    private GameObject GetCharacterByName(string name)
    {
        foreach (var character in _characters)
        {
            var chNames = character.GetComponent<CharacterInfo>().CharacterNames;
            foreach (var chName in chNames)
            {
                if (chName == name)
                {
                    return character;
                }
            }
        }
        Debug.LogError($"Can not find name {name} among characters");
        return null;
    }
}
