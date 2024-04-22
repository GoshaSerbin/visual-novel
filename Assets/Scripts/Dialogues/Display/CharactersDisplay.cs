using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class CharactersDisplay : MonoBehaviour
{
    [SerializeField] private float _animationTime = 0.9f;
    private string _charactersLocationPath = "Characters"; // path in Resources folder
    private List<CharacterSO> _allCharacters;

    [SerializeField] private GameObject _leftCharacterPanel;
    [SerializeField] private GameObject _rightCharacterPanel;

    private Vector3 _hiddenLeftPanelPos;
    private Vector3 _hiddenRightPanelPos;

    private struct CharacterUIData
    {
        public CharacterSO Character;
        public TalkAnimations Animations;
        public UnityEngine.UI.Image Image;

        public CharacterUIData(GameObject panel)
        {
            Image = panel.GetComponentInChildren<UnityEngine.UI.Image>();
            Animations = panel.GetComponentInChildren<TalkAnimations>();
            Character = ScriptableObject.CreateInstance<CharacterSO>();
        }
    }

    private CharacterUIData _leftCharacterData;
    private CharacterUIData _rightCharacterData;

    private void OnEnable()
    {
        Narrator.OnCharactersReset += ResetCharacters;
        Narrator.OnCharacterSaid += Animate;
    }

    private void OnDisable()
    {
        Narrator.OnCharactersReset -= ResetCharacters;
        Narrator.OnCharacterSaid -= Animate;
    }

    private void Awake()
    {
        _leftCharacterData = new CharacterUIData(_leftCharacterPanel);
        _rightCharacterData = new CharacterUIData(_rightCharacterPanel);
    }

    private void Start()
    {
        LoadAllCharacters();
        Vector3 leftScreenPos = new Vector3(0, 0.5f, 10);
        Vector3 rightScreenPos = new Vector3(1, 0.5f, 10);
        _hiddenLeftPanelPos = Camera.main.ViewportToWorldPoint(leftScreenPos);
        _hiddenRightPanelPos = Camera.main.ViewportToWorldPoint(rightScreenPos);
        _leftCharacterPanel.transform.position = _hiddenLeftPanelPos;
        _rightCharacterPanel.transform.position = _hiddenRightPanelPos;
    }

    private void LoadAllCharacters()
    {
        _allCharacters = new List<CharacterSO>();

        Object[] objects = Resources.LoadAll(_charactersLocationPath, typeof(CharacterSO));
        foreach (Object obj in objects)
        {
            _allCharacters.Add((CharacterSO)obj);
        }
        Debug.Log($"Found {_allCharacters.Count()} characters.");
    }

    private void PadNames(ref string[] names, string pad)
    {
        switch (names.Count())
        {
            case 0:
                {
                    names = new string[] { pad, pad };
                    break;
                }
            case 1:
                {
                    names = new string[] { names[0], pad };
                    break;
                }
            case 2:
                {
                    break;
                }
            default:
                Debug.LogError("2+ characters not implemented.");
                break;
        }
    }

    private void ResetCharacters(string[] names)
    {
        Debug.Log($"{names.Count()} characters will be displayed.");
        PadNames(ref names, "");
        ResetCharacter(ref _leftCharacterData, _leftCharacterPanel, _hiddenLeftPanelPos.x, names[0]);
        ResetCharacter(ref _rightCharacterData, _rightCharacterPanel, _hiddenRightPanelPos.x, names[1]);
    }

    private void ResetCharacter(ref CharacterUIData character, GameObject panel, float hiddenPanelPos, string name)
    {
        if (name == "" || !character.Character.Names.Contains(name))
        {
            // Hide
            panel.transform.LeanMoveX(hiddenPanelPos, _animationTime).setEaseInCubic();
        }
        if (name != "")
        {
            // Show
            character.Character = GetCharacterByName(name);
            character.Image.sprite = character.Character.EmotionSprites[0];
            panel.transform.LeanMoveX(0, _animationTime).setEaseOutCubic().delay = _animationTime;
        }
    }

    private void Animate(Narrator.Replica replica)
    {
        Animate(_leftCharacterData, _leftCharacterPanel, replica);
        Animate(_rightCharacterData, _rightCharacterPanel, replica);
    }

    private void Animate(CharacterUIData character, GameObject panel, Narrator.Replica replica)
    {
        if (replica.Name != "" && character.Character.Names.Contains(replica.Name))
        {
            character.Animations.StartTalking();
            character.Animations.ChangeEmotion(character.Character.EmotionSprites[(int)replica.Emotion]);
        }
        else
        {
            panel.GetComponentInChildren<TalkAnimations>().StopTalking();
        }
    }

    private CharacterSO GetCharacterByName(string searchingName)
    {
        foreach (var character in _allCharacters)
        {
            foreach (var name in character.Names)
            {
                if (name == searchingName)
                {
                    return character;
                }
            }
        }

        if (searchingName != "") // empty name is a valid behavior
        {
            Debug.LogError($"Can not find name {searchingName} among characters");
        }
        return ScriptableObject.CreateInstance<CharacterSO>(); // will create empty sprite
    }
}
