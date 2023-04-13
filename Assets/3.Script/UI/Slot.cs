using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Slot : MonoBehaviour, IPointerUpHandler
{
    public int slotNum;
    public Item item;
    public Image itemIcon;
    [SerializeField]private Text amountText;

    private void Awake()
    {
        //TryGetComponent(out amountText);
    }

    public void UpdateSlotUI()
    {
        itemIcon.sprite = item.itemImage;
        amountText.text = item.amount.ToString();
        itemIcon.gameObject.SetActive(true);
    }
    public void RemoveSlot()
    {
        item = null;
        itemIcon.gameObject.SetActive(false);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if(item is null)
        {
            return;
        }
        bool isUse = item.Use();
        if(isUse)
        {
            if (item.amount <= 0)
            {
                Inventory.instance.RemoveItem(slotNum);
            }
        }
    }
    
}
