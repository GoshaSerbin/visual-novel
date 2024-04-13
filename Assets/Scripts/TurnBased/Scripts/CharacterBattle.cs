using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CharacterBattle : MonoBehaviour
{
    CharacterBase _charBase;
    [SerializeField] private HealthSystem _healthSystem;

    //private enum StatusBuffs
    //{
    //    ATTACKBUFF,
    //    DEFENSEBUFF,
    //}

    //List<StatusBuffs> _characterBuffs;
    // заготовка на будущее прост
    bool _defenseBuffActive = false;
    private void Awake()
    {
        _charBase = GetComponent<CharacterBase>();
    }

    public void Attack(CharacterBattle target, Action onAttackComplete)
    {
        // animation
        int damageDone = _charBase._baseAttack + UnityEngine.Random.Range(0, _charBase._baseAttack + 1);
        Debug.Log(target._defenseBuffActive);
        if (target._defenseBuffActive ) {
            Debug.Log("Damage halved");
            damageDone /= 2;
            _defenseBuffActive = false;
        }
        target.TakeDamage(damageDone);
        Debug.Log(target + "CurrentHealth = " + target.GetCurrentHealth().ToString());
        onAttackComplete();
    }

    public void Guard(Action onAttackComplete)
    {
        _defenseBuffActive = true;
        onAttackComplete();
    }

    public void Setup()
    {
        _healthSystem = new HealthSystem(100);
    }


    public int GetMaxHealth()
    {
        return _healthSystem.GetMaxHealth();
    }

    public int GetCurrentHealth()
    {
        return _healthSystem.GetCurrentHealth();
    }
    public void TakeDamage(int damage)
    {
        _healthSystem.Damage(damage);
    }

    public bool IsDead() {
        return _healthSystem.IsDead();
    }
}
