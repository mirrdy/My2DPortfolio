using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    Inventory inventory;
    ClickManager clickManager;

    public GameObject inventoryPanel;
    bool isActiveInventory = false;

    public Slot[] slots;
    public Transform slotHolder;

    public QuickSlot[] qSlots;
    public Transform qSlotHolder;

    private void Start()
    {
        inventory = Inventory.instance;
        clickManager = ClickManager.instance;

        slots = slotHolder.GetComponentsInChildren<Slot>();
        qSlots = qSlotHolder.GetComponentsInChildren<QuickSlot>();

        inventory.onSlotCountChange += SlotChange;
        inventory.onChangeItem += RedrawSlotUI;
        inventoryPanel.SetActive(isActiveInventory);

        clickManager.onChangeItem += RedrawSlotUI;
    }
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.I))
        {
            isActiveInventory = !isActiveInventory;
            inventoryPanel.SetActive(isActiveInventory);
        }

        CheckInputQuickSlot();

    }
    private void CheckInputQuickSlot()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            qSlots[0].UseItemInSlot();
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            qSlots[1].UseItemInSlot();
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            qSlots[2].UseItemInSlot();
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            qSlots[3].UseItemInSlot();
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            qSlots[4].UseItemInSlot();
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

        for (int i = 0; i < qSlots.Length; i++)
        {
            //qSlots[i].RemoveSlot();
            //qSlots[i].item = inventory.items[i];
            qSlots[i].slotNum = i;
            
            if (qSlots[i].linkedSlotIndex >= 0)
            {
                if (slots[qSlots[i].linkedSlotIndex].item == null)
                {
                    qSlots[i].linkedSlotIndex = -1;
                    qSlots[i].item = null;
                }
                else
                {
                    qSlots[i].item = slots[qSlots[i].linkedSlotIndex].item;
                }
            }
            qSlots[i].UpdateSlotUI();
        }
    }
}
