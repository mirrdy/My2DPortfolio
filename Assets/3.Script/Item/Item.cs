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
    public ItemType itemType;
    public string itemName;
    public Sprite itemImage;
    public int amount = 1;
    public List<ItemEffect> effects;

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
