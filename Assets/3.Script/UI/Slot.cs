using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Slot : MonoBehaviour
{
    public int slotNum;
    public Item item;
    public Image itemIcon;

    public Text amountText;

    public void UpdateSlotUI()
    {
        if(item==null)
        {
            return;
        }
        if(item.amount <= 0)
        {
            itemIcon.sprite = null;
            itemIcon.gameObject.SetActive(false);
            amountText.text = "0";
            return;
        }
        itemIcon.sprite = item.itemImage;
        amountText.text = item.amount.ToString();
        itemIcon.gameObject.SetActive(true);
    }
    public void RemoveSlot()
    {
        item = null;
        itemIcon.sprite = null;
        amountText.text = "0";
        //itemIcon.gameObject.SetActive(false);
    }

    public void ExchangeItem(Slot slot)
    {
        Item tmpItem = this.item.CloneItem();
        this.item = slot.item.CloneItem();
        slot.item = tmpItem.CloneItem();
    }

    public void UseItemInSlot()
    {
        if (item is null)
        {
            return;
        }

        bool isUse = item.Use(Inventory.instance.items.FindIndex(item => item == this.item));
        if(item is null)
        {
            return;
        }
        if (isUse)
        {
            amountText.text = item.amount.ToString();
            if (item.amount <= 0)
            {
                Inventory.instance.RemoveItem(item);
            }
        }

    }
    
}
