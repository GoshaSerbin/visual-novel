using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewCharacter", menuName = "CharacterSO")]
public class CharacterSO : ScriptableObject
{
    public List<string> Names = new List<string>();
    public List<Sprite> EmotionSprites = new List<Sprite>();
}
