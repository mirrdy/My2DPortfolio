using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    Equipment,
    Consumables,
    Etc
}
[System.Serializable]
public class Item
{
    public string itemName;
    public Sprite itemImage;
    public int amount = 1;
    public ItemType itemType;
    public List<ItemEffect> effects;

    public Item(string itemName, Sprite itemImage, int amount = 1, ItemType itemType = ItemType.Etc, List<ItemEffect> effects = null)
    {
        this.itemName = itemName;
        this.itemImage = itemImage;
        this.amount = amount;
        this.itemType = itemType;
        this.effects = effects;
    }

    public bool Use()
    {
        bool isUsed = false;
        foreach(ItemEffect effect in effects)
        {
            isUsed = effect.ExecuteRole();
        }
        
        if(isUsed)
        {
            amount--;
        }

        return isUsed;
    }
}
