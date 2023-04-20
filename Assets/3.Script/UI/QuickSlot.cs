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
    public new void UseItemInSlot()
    {
        if (item is null)
        {
            return;
        }

        bool isUse = item.Use();
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
