using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterData : MonoBehaviour
{
    [HideInInspector] public CharacterSO Character;

    private void Awake()
    {
        Character = ScriptableObject.CreateInstance<CharacterSO>();
    }
}
