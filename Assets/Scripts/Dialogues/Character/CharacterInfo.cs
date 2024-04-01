using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterInfo : MonoBehaviour
{
    [SerializeField] private List<string> _characterNames = new();

    public List<string> CharacterNames { get { return _characterNames; } private set => _characterNames = value; }

}