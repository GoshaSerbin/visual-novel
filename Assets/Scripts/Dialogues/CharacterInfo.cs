using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterInfo : MonoBehaviour
{
    [SerializeField] private string characterName = "";
    public string CharacterName { get { return characterName; } private set { characterName = value; } }

}