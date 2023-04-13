using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ItemEffect/Consumable/Health")]
public class ItemHealingEffect : ItemEffect
{
    public override bool ExecuteRole()
    {
        Debug.Log($"PlayerHp Add: healingPoint");
        return true;
    }
}
