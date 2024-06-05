using Ink.Parsed;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Enemy Data", menuName = "EnemySO")]
public class EnemyData : ScriptableObject
{
    [SerializeField] private string enemyName;
    [SerializeField] private Element enemyWeakness;
    [SerializeField] private int enemyBaseAttack;
    [SerializeField] private int enemyHealthMax;
    [SerializeField] private Sprite enemySprite;
    [SerializeField] private List<BattleItemSO> possibleLoot;

    public string EnemyName
    { get { return enemyName; } }
    public Element EnemyWeakness
    { get { return enemyWeakness; } }
    public int EnemyBaseAttack
    { get { return enemyBaseAttack; } }
    public int EnemyHealthMax
    { get { return enemyHealthMax; }}
    public Sprite EnemySprite
    { get { return enemySprite; } }    
    public List<BattleItemSO> PossibleLoot
    { get { return  possibleLoot; } }
}
