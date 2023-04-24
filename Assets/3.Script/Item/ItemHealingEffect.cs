using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ItemEffect/Consumable/Health")]
public class ItemHealingEffect : ItemEffect
{
    PlayerControll player;
    public int healingPoint;
    public override bool ExecuteRole()
    {
        GameObject.FindGameObjectWithTag("Player").TryGetComponent(out player);
        if(player.currentHp+healingPoint < player.maxHp)
        {
            player.currentHp = player.maxHp;
        }
        else
        {
            player.currentHp += healingPoint;
        }

        return true;
    }
}
