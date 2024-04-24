using UnityEngine;
public enum Weakness
{
    FIRE,
    ICE,
    LASER,
    WATER,
    ELECTRIC,
};

[CreateAssetMenu(fileName = "New Enemy Data", menuName = "EnemySO")]
public class EnemyData : ScriptableObject
{
    [SerializeField] private string enemyName;
    [SerializeField] private Weakness enemyWeakness;
    [SerializeField] private int enemyBaseAttack;
    [SerializeField] private int enemyHealthMax;
    [SerializeField] private Sprite enemySprite;

    public string EnemyName
    { get { return enemyName; } }
    public Weakness EnemyWeakness
    { get { return enemyWeakness; } }
    public int EnemyBaseAttack
    { get { return enemyBaseAttack; } }
    public int EnemyHealthMax
    { get { return enemyHealthMax; }}
    public Sprite EnemySprite
    { get { return enemySprite; } }
}
