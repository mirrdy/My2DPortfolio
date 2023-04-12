using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static Inventory instance;
    private void Awake()
    {
        if(instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }

    public delegate void OnSlotCountChange(int value);
    public OnSlotCountChange onSlotCountChange;

    public delegate void OnChangeItem();
    public OnChangeItem onChangeItem;

    public List<Item> items = new List<Item>();

    private int slotCount;
    public int SlotCount
    {
        get => slotCount;
        set 
        { 
            slotCount = value;
            //onSlotCountChange.Invoke(slotCount);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        SlotCount = 25;
    }

    public bool AddItem(Item item)
    {
        if (items.Count < slotCount)
        {
            this.items.Add(item);
            if (onChangeItem != null)
            {
                onChangeItem.Invoke();
            }
            return true;
        }
        return false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("FieldItem"))
        {
            collision.TryGetComponent(out FieldItem fieldItem);
            if(AddItem(fieldItem.GetItem()))
            {
                fieldItem.DestroyItem();
            }
        }
    }
}
