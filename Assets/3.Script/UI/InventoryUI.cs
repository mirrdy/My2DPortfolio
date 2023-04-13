using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    Inventory inventory;

    public GameObject inventoryPanel;
    bool isActiveInventory = false;

    public Slot[] slots;
    public Transform slotHolder;

    private void Start()
    {
        inventory = Inventory.instance;
        slots = slotHolder.GetComponentsInChildren<Slot>();
        inventory.onSlotCountChange += SlotChange;
        inventory.onChangeItem += RedrawSlotUI;
        inventoryPanel.SetActive(isActiveInventory);
    }
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.I))
        {
            isActiveInventory = !isActiveInventory;
            inventoryPanel.SetActive(isActiveInventory);
        }
    }
    private void SlotChange(int value)
    {
        for(int i =0; i<slots.Length; i++)
        {
            slots[i].slotNum = i;
            if (i<inventory.SlotCount)
            {
                slots[i].GetComponent<Button>().interactable = true;
            }
            else
            {
                slots[i].GetComponent<Button>().interactable = false;
            }
        }
    }
    public void AddSlot()
    {
        inventory.SlotCount+=5;
    }
    private void RedrawSlotUI()
    {
        for(int i =0; i<slots.Length; i++)
        {
            slots[i].RemoveSlot();
        }
        for(int i=0; i<inventory.items.Count; i++)
        {
            slots[i].slotNum = i;
            slots[i].item = inventory.items[i];
            slots[i].UpdateSlotUI();
        }
    }
}
