using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Craft : MonoBehaviour
{
    public CanvasRenderer craftPanel;
    public CraftSlot[] craftSlots = null;

    public List<Item> craftItemList;

    private void Awake()
    {
        craftSlots = craftPanel.gameObject.GetComponentsInChildren<CraftSlot>();
    }
    public void CraftItem()
    {
        CheckCraftList(craftSlots[0].item, craftSlots[1].item);
    }

    private void CheckCraftList(Item item1, Item item2)
    {
        if(item1 == null || item2 == null)
        {
            return;
        }
        // Brick 기반 조합
        if(item1.itemName == "brick")
        {
            // Brick + Brick
            if (item2.itemName == "brick")
            {

                // sword1
                if (item1.amount  + item2.amount == 3)
                {
                    
                    Inventory.instance.AddItem(craftItemList.Find((item) => item.itemName == craftItemList[0].itemName));
                    ClearCraftSlot();
                }
                // sword2
                else if (item1.amount + item2.amount == 5)
                {
                    Inventory.instance.AddItem(craftItemList.Find((item) => item.itemName == craftItemList[1].itemName));
                    ClearCraftSlot();
                }
                // shield1
                else if (item1.amount + item2.amount == 4)
                {

                    Inventory.instance.AddItem(craftItemList.Find((item) => item.itemName == craftItemList[2].itemName));
                    ClearCraftSlot();
                }
            }
        }
        // Branch 파생
        else if(item1.itemName == "Branch")
        {

        }
        UpdateCraftSlot();
    }
    private void ClearCraftSlot()
    {
        craftSlots[0].RemoveSlot();
        craftSlots[1].RemoveSlot();

    }
    private void UpdateCraftSlot()
    {
        craftSlots[0].UpdateSlotUI();
        craftSlots[1].UpdateSlotUI();
    }
}
