using UnityEngine;
public class CharacterBase : MonoBehaviour
{
    public string _unitName;
    public int _maxHP;
    // типа характеристики
    public int _baseAttack;
    public float _critRate;

    private void Awake()
    {
        _maxHP = 100;
        _baseAttack = 10;
        _critRate = 0.2f;
    }
}
