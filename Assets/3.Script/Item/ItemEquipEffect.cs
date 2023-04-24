using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ItemEffect/Equipable")]
public class ItemEquipEffect : ItemEffect
{
    PlayerControll player;
    public PlayerControll.EquipPart equipPart;
    public int atk;
    public int def;
    public override bool ExecuteRole()
    {
        GameObject.FindGameObjectWithTag("Player").TryGetComponent(out player);
        

        return true;
    }
}
