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

    [SerializeField]private Text amountText;

    public void UpdateSlotUI()
    {
        itemIcon.sprite = item.itemImage;
        amountText.text = item.amount.ToString();
        itemIcon.gameObject.SetActive(true);
    }
    public void RemoveSlot()
    {
        item = null;
        amountText.text = "0";
        itemIcon.gameObject.SetActive(false);
    }

    private void UseItemInSlot()
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
                Inventory.instance.RemoveItem(slotNum);
            }
        }
    }
    
}
