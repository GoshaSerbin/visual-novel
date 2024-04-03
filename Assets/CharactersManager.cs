using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharactersManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> _characters = new();
    private List<GameObject> _currentCharacters = new();

    private string _previousResetCharactersValue = "";

    [SerializeField] private GameObject _leftCharacterPanel;
    [SerializeField] private GameObject _rightCharacterPanel;


    public void ResetCharacters(string resetCharactersValue)
    {
        if (_previousResetCharactersValue == resetCharactersValue)
        {
            return;
        }
        _previousResetCharactersValue = resetCharactersValue;
        string[] characterNames = resetCharactersValue.Split(",");
        Debug.Log("Destroying characters");
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
            var character = Instantiate(GetCharacterByName(characterNames[0].Trim()), _leftCharacterPanel.transform);
            // character.GetComponent<CharacterAnimations>().Appear(true);
            _currentCharacters.Add(character);
        }
        if (characterNames[1] != "")
        {
            var character = Instantiate(GetCharacterByName(characterNames[1].Trim()), _rightCharacterPanel.transform);
            // character.GetComponent<CharacterAnimations>().Appear(false);
            // TO DO: Add animations to appear and disapear. Возможно придется переделать панели для персонажей.
            _currentCharacters.Add(character);
        }
    }

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
