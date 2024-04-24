using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MainCharBattle : CharacterBattle
{
    [SerializeField] private MainCharData _mainCharData = null;

    public override bool AttackBuffActive { get; set; }
    public override bool DefenseBuffActive { get; set; }

    public override HealthSystem HealthComponent { get; set; }

    public MainCharBattle(MainCharData charData)
    {
        _mainCharData = charData;
        HealthComponent = new HealthSystem(charData.MaxHP);
        AttackBuffActive = false;
        DefenseBuffActive = false;
    }
    public override void Attack(CharacterBattle target, Action onAttackComplete)
    {

    }
}
