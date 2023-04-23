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
        // Brick 기반 조합
        if(item1.itemName == "brick")
        {
            // Brick + Brick
            if (item2.itemName == "brick")
            {
                // 2개 + 3개
                if (item1.amount == 2 && item2.amount == 3)
                {
                    Inventory.instance.AddItem(craftItemList.Find((item) => item.itemName == "ShortSword"));
                    ClearCraftSlot();
                }
                // 3개 + 2개
                else if(item1.amount == 3 && item2.amount == 2)
                {
                    Inventory.instance.AddItem(craftItemList.Find((item) => item.itemName == "SlimeSword"));
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

        craftSlots[0].item = null;
    }
    private void UpdateCraftSlot()
    {
        craftSlots[0].UpdateSlotUI();
        craftSlots[1].UpdateSlotUI();
    }
}
