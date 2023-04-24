using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipableItem : Item
{
    public EquipableItem(string itemName, Sprite itemImage, int amount = 1, ItemType itemType = ItemType.Etc, List<ItemEffect> effects = null)
    {
        this.itemName = itemName;
        this.itemImage = itemImage;
        this.amount = amount;
        this.itemType = itemType;
        this.effects = effects;
    }
    
}
