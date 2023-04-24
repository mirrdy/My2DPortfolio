using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldItem : MonoBehaviour
{
    public Item item;
    public SpriteRenderer image;
    public float dropProb;

    public void SetItem(Item item)
    {
        this.item.itemName = item.itemName;
        this.item.itemImage = item.itemImage;
        this.item.itemType = item.itemType;
        this.item.effects = item.effects;

        image.sprite = this.item.itemImage;
    }
    public Item GetItem()
    {
        return this.item;
    }
    public void DestroyItem()
    {
        Destroy(gameObject);
    }
}
