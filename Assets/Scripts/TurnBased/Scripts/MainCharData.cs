using UnityEngine;
using System;

[System.Serializable]
public class MainCharData
{
    [SerializeField] private int maxHP = 200;
    [SerializeField] private int baseAttack = 15;

    public int MaxHP { get { return maxHP; } }
    public int BaseAttack { get { return baseAttack; } }
}
