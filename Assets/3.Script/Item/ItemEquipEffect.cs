using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ItemEffect/Equipable")]
public class ItemEquipEffect : ItemEffect
{
    public override bool ExecuteRole()
    {
        return false;
    }

    public override bool ExecuteRole(int invenIndex)
    {
        GameObject.FindGameObjectWithTag("Player").TryGetComponent(out PlayerControll player);

        player.Equip(invenIndex);

        return true;
    }
}
