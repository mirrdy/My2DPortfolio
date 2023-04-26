using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    Equipable,
    Consumables,
    Etc
}
[System.Serializable]
public class Item
{
    public string itemName;
    public Sprite itemImage;
    public int amount = 1;
    public int atk;
    public int def;
    public ItemType itemType;
    public PlayerControll.EquipPart equipPart;
    public List<ItemEffect> effects;

    public Item()
    {
        
    }
    public Item(string itemName, Sprite itemImage, int amount = 1, int atk = 0, int def = 0, ItemType itemType = ItemType.Etc, 
        PlayerControll.EquipPart equipPart = PlayerControll.EquipPart.NonEquipment, List<ItemEffect> effects = null)
    {
        this.itemName = itemName;
        this.itemImage = itemImage;
        this.amount = amount;
        this.atk = atk;
        this.def = def;
        this.itemType = itemType;
        this.equipPart = equipPart;
        this.effects = effects;
    }

    public Item CloneItem()
    {
        Item tmpItem = new Item(this.itemName, this.itemImage, this.amount, this.atk, this.def, this.itemType, this.equipPart, this.effects);

        return tmpItem;
    }

    public bool MoveItem(int amount)
    {
        if (this.amount < amount)
        {
            return false;
        }

        this.amount -= amount;
        return true;
    }

    public bool Use(int invenIndex)
    {
        bool isUsed = false;
        foreach(ItemEffect effect in effects)
        {
            if (effect as ItemEquipEffect != null)
            {
                isUsed = effect.ExecuteRole(invenIndex);
            }
            else
            {
                isUsed = effect.ExecuteRole();
                if (isUsed)
                {
                    amount--;
                }
            }
        }

        return isUsed;
    }
}
