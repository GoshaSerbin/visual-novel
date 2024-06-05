using Inventory.Model;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class BattleItemSO : ItemSO
{
    [field: SerializeField]
    public Element ItemElement { get; set; }

    [field: SerializeField]
    public int ItemBaseAttack { get; set; }

    [field: SerializeField]
    public float AppearanceProbability { get; set; }
}
