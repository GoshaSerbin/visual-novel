using UnityEngine;
using System;

[System.Serializable]
public class MainCharData
{
    [SerializeField] private int maxHP = 200;
    [SerializeField] private int baseAttack = 15;

    [SerializeField] private int stamina = 1;
    [SerializeField] private int intellect = 1;
    [SerializeField] private int strength = 1;
    [SerializeField] private int agility = 1;

    public int MaxHP { get { return maxHP; } }
    public int Stamina { get { return stamina; } }
    public float Strength { get { return strength; } }
    public int Agility { get {  return agility; } }
    public int Intellect { get { return intellect; } }
    public int BaseAttack { get { return baseAttack; } }
}
