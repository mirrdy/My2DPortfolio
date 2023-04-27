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
        if(craftSlots[0].item != null && craftSlots[1].item != null)
        {

        }
        else if(craftSlots[0].item != null && craftSlots[1].item == null)
        {

        }
        else if(craftSlots[0].item == null && craftSlots[1].item != null)
        {

        }
        else
        {

        }
        
        CheckCraftList(craftSlots[0].item, craftSlots[1].item);
    }

    private void CheckCraftList(Item item1, Item item2)
    {
        if(item1 == null || item2 == null)
        {
            return;
        }

        bool isSuccess = false;

        // Brick 기반 조합
        if(item1.itemName == "brick")
        {
            // Brick + Brick
            if (item2.itemName == "brick")
            {

                // sword1
                if (item1.amount  + item2.amount == 3)
                {
                    
                    //Inventory.instance.AddItem(craftItemList.Find((item) => item.itemName == craftItemList[0].itemName));
                    Inventory.instance.AddItem(craftItemList[0]);

                    isSuccess = true;
                }
                // shield1
                else if (item1.amount + item2.amount == 4)
                {
                    //Inventory.instance.AddItem(craftItemList.Find((item) => item.itemName == craftItemList[2].itemName));
                    Inventory.instance.AddItem(craftItemList[2]);

                    isSuccess = true;
                }
                //potion1
                else if(item1.amount+item2.amount == 2)
                {
                    Inventory.instance.AddItem(craftItemList[3]);

                    isSuccess = true;
                }
            }
        }
        // 슬라임조각
        else if(item1.itemName == "slimeSlice")
        {
            if(item2.itemName == "slimeSlice")
            {
                if(item1.amount + item2.amount == 5)
                {
                    Inventory.instance.AddItem(craftItemList[1]);

                    isSuccess = true;
                }
            }
        }
        // Branch 파생
        else if(item1.itemName == "Branch")
        {

        }
        if (isSuccess)
        {
            ClearCraftSlot();
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
    private void UpdateInventoryUI()
    {

    }
}
