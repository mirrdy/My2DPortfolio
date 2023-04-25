using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ClickManager : MonoBehaviour
{
    public static ClickManager instance;
    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }

    private Slot selectedSlot;
    private Item selectedItem;
    private Image clickedItemIcon;

    private float interval = 0.75f;
    private float doubleClickTime = 0;

    private bool isSelecting = false;

    public delegate void OnChangeItem();
    public OnChangeItem onChangeItem;

    private void Update()
    {
        if (isSelecting)
        {
            UpdateMouseMoving();
        }
        
        if (Input.GetMouseButtonDown(0))
        {   
            Vector3 position = Input.mousePosition;
            //Vector3 position = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            Slot[] allSlots = (Slot[])FindObjectsOfType(typeof(Slot));
            Slot[] slots = Array.FindAll(allSlots, (slot) => (slot as QuickSlot) == null);
            QuickSlot[] qSlots = (QuickSlot[])FindObjectsOfType(typeof(QuickSlot));

            foreach (Slot slot in allSlots)
            {
                if (slot.gameObject.activeSelf == false)
                {
                    continue;
                }
                else if(slot.item == null && isSelecting == false)
                {
                    continue;
                }

                slot.TryGetComponent(out RectTransform slotRect);

                Vector3[] slotCorners = new Vector3[4];
                slotRect.GetWorldCorners(slotCorners);

                if (slotCorners[0].x <= position.x && slotCorners[2].x >= position.x &&
                    slotCorners[0].y <= position.y && slotCorners[2].y >= position.y)
                {
                    if(isSelecting == false)
                    {
                        selectedSlot = slot;
                        isSelecting = true;
                        CreateSelectedItemIcon(selectedSlot);
                    }
                    else
                    {
                        // 이전에 선택했던 슬롯인지
                        if(selectedSlot == slot)
                        {
                            // 더블클릭 ON
                            if ((Time.time - doubleClickTime) < interval)
                            {
                                DoWhenSlotDoubleClick(selectedSlot);
                                selectedSlot.UpdateSlotUI();
                                if(selectedSlot as QuickSlot != null)
                                {
                                    slots[(selectedSlot as QuickSlot).linkedSlotIndex].UpdateSlotUI();
                                }
                                doubleClickTime = -1.0f;
                            }
                            else
                            {
                                CreateSelectedItemIcon(selectedSlot);
                                doubleClickTime = Time.time;
                            }
                        }
                        // 다른 슬롯 선택
                        else
                        {
                            // 선택한곳이 퀵슬롯이면
                            QuickSlot qSlot = GetQuickSlotFromPosition(position);
                            if (qSlot != null)
                            {
                                // 이전에 선택했던 슬롯이 퀵슬롯이면 
                                if (selectedSlot as QuickSlot != null)
                                {
                                    qSlot.item = selectedItem.CloneItem();
                                    selectedSlot.RemoveSlot();
                                }
                                // 일반 슬롯이면 선택했던 아이템 퀵슬롯에 등록(참조복사)
                                else
                                {
                                    // 소비, 장비아이템만 퀵슬롯 등록 가능
                                    if(selectedItem.itemType != ItemType.Etc)
                                    {
                                        qSlot.item = selectedItem;
                                        qSlot.linkedSlotIndex = selectedSlot.slotNum;
                                    }
                                }
                                qSlot.UpdateSlotUI();
                            }
                            // 선택한 곳이 크래프트 슬롯이면
                            CraftSlot cSlot = GetCraftSlotFromPosition(position);
                            if(cSlot != null)
                            {
                                // 이전에 선택했던 슬롯이 일반슬롯일 때
                                if(selectedSlot as QuickSlot == null && selectedSlot as CraftSlot == null)
                                {
                                    // 크래프팅 재료는 기타아이템으로만 구현할거임 (임시)
                                    if(selectedItem.itemType == ItemType.Etc)
                                    {
                                        if (cSlot.item?.itemName == selectedItem.itemName)
                                        {
                                            if (selectedItem.MoveItem(1) == true)
                                            {
                                                cSlot.item.amount++;
                                            }
                                        }
                                        else
                                        {
                                            cSlot.item = selectedItem.CloneItem();
                                            cSlot.item.amount = 0;
                                            if (selectedItem.MoveItem(1) == true)
                                            {
                                                cSlot.item.amount++;
                                            }
                                        }
                                    }
                                }
                                cSlot.UpdateSlotUI();
                                if(selectedItem.amount <= 0)
                                {
                                    Inventory.instance.RemoveItem(selectedItem);
                                }
                                selectedSlot.UpdateSlotUI();
                            }


                            isSelecting = false;
                            DestroySelectedIcon();
                        }
                    }

                    Debug.Log($"Slot{selectedSlot.slotNum} is selected");
                    
                    break;
                }
            }
        }
        else if (Input.GetMouseButtonDown(1))
        {
            CraftSlot[] cSlots = (CraftSlot[])FindObjectsOfType(typeof(CraftSlot));
            Vector3 position = Input.mousePosition;

            foreach (Slot slot in cSlots)
            {
                if (slot.gameObject.activeSelf == false)
                {
                    continue;
                }
                else if (slot.item == null && isSelecting == false)
                {
                    continue;
                }

                slot.TryGetComponent(out RectTransform slotRect);

                Vector3[] slotCorners = new Vector3[4];
                slotRect.GetWorldCorners(slotCorners);

                if (slotCorners[0].x <= position.x && slotCorners[2].x >= position.x &&
                    slotCorners[0].y <= position.y && slotCorners[2].y >= position.y)
                {
                    if(slot.item == null)
                    {
                        return;
                    }

                    slot.item.amount--;
                    Inventory.instance.AddItem(slot.item);

                    if (slot.item.amount <=0)
                    {
                        slot.RemoveSlot();
                        return;
                    }
                    slot.UpdateSlotUI();
                }
            }
        }
    }

    private QuickSlot GetQuickSlotFromPosition(Vector3 position)
    {
        QuickSlot[] qSlots = (QuickSlot[])FindObjectsOfType(typeof(QuickSlot));

        foreach (QuickSlot qSlot in qSlots)
        {
            qSlot.TryGetComponent(out RectTransform slotRect);

            Vector3[] slotCorners = new Vector3[4];
            slotRect.GetWorldCorners(slotCorners);

            if (slotCorners[0].x <= position.x && slotCorners[2].x >= position.x &&
                slotCorners[0].y <= position.y && slotCorners[2].y >= position.y)
            {
                return qSlot;
            }
        }

        return null;
    }
    private CraftSlot GetCraftSlotFromPosition(Vector3 position)
    {
        CraftSlot[] cSlots = (CraftSlot[])FindObjectsOfType(typeof(CraftSlot));

        foreach(CraftSlot cSlot in cSlots)
        {
            cSlot.TryGetComponent(out RectTransform slotRect);

            Vector3[] slotCorners = new Vector3[4];
            slotRect.GetWorldCorners(slotCorners);

            if (slotCorners[0].x <= position.x && slotCorners[2].x >= position.x &&
                slotCorners[0].y <= position.y && slotCorners[2].y >= position.y)
            {
                return cSlot;
            }
        }

        return null;
    }


    private void CreateSelectedItemIcon(Slot slot)
    {
        DestroySelectedIcon();

        if (slot.item == null)
        {
            return;
        }
        //selectedItem = slot.item.CloneItem();
        selectedItem = slot.item;
        
        clickedItemIcon = Instantiate(slot.itemIcon, Input.mousePosition, Quaternion.identity);
        clickedItemIcon.gameObject.transform.SetParent(FindObjectOfType<Canvas>().transform);

        // 아이콘 생성 후 기존 UI 이벤트 방해하지 않도록 레이캐스터 비활성화
        GraphicRaycaster raycaster = clickedItemIcon.GetComponent<GraphicRaycaster>();
        if (raycaster == null)
        {
            raycaster = clickedItemIcon.gameObject.AddComponent<GraphicRaycaster>();
        }
        raycaster.enabled = false;

        Color tmpColor = clickedItemIcon.color;
        tmpColor.a = 0.5f;

        clickedItemIcon.gameObject.SetActive(true);

        // 이미지의 컬러의 a값을 직접 바꿀 수가 없어서 임시로 담은 컬러를 변경 후 덮어씀
        clickedItemIcon.color = tmpColor;
    }
    private void DoWhenSlotDoubleClick(Slot slot)
    {
        slot.UseItemInSlot();

        /*if (slot.item == null)
        {
            return;
        }

        bool isUse = slot.item.Use();
        if (isUse)
        {
            slot.amountText.text = slot.item.amount.ToString();
            if (slot.item.amount <= 0)
            {
                Inventory.instance.RemoveItem(slot.item);
            }
        }*/
    }

    private void DestroySelectedIcon()
    {
        if (clickedItemIcon != null)
        {
            Destroy(clickedItemIcon.gameObject);
        }
    }
    private void UpdateMouseMoving()
    {
        if (clickedItemIcon == null)
        {
            return;
        }
        Vector2 mousePos = Input.mousePosition;

        clickedItemIcon.transform.position = mousePos;
    }

}
