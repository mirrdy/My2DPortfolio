using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class QuickSlot : Slot
{
    public int linkedSlotIndex;

    private void Awake()
    {
        linkedSlotIndex = -1;
    }
    public new void UpdateSlotUI()
    {
        if (linkedSlotIndex < 0)
        {
            itemIcon.sprite = null;
            amountText.text = "0";
            itemIcon.gameObject.SetActive(false);
        }
        else
        {
            itemIcon.sprite = item.itemImage;
            amountText.text = item.amount.ToString();
            itemIcon.gameObject.SetActive(true);
        }
    }
    public new void UseItemInSlot()
    {
        if (item is null)
        {
            return;
        }

        bool isUse = item.Use(Inventory.instance.items.FindIndex(item => item == this.item));
        if (item is null)
        {
            linkedSlotIndex = -1;
            return;
        }
        if (isUse)
        {
            amountText.text = item.amount.ToString();
            if (item.amount <= 0)
            {
                linkedSlotIndex = -1;
                
                Inventory.instance.RemoveItem(item);
            }
        }

    }

}
