using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ClickManager : MonoBehaviour
{
    private Image clickedItemIcon;

    private float interval = 0.75f;
    private float doubleClickTime = 0;

    private bool isSelecting = false;

    private void Update()
    {
        if (isSelecting)
        {
            UpdateMouseMoving();
        }
        else
        {

        }

        if (Input.GetMouseButtonDown(0))
        {
            //Vector3 clickPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            //Vector3 position = Input.mousePosition;
            Vector3 position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Slot[] slots = (Slot[])FindObjectsOfType(typeof(Slot));
            QuickSlot[] qSlots = (QuickSlot[])FindObjectsOfType(typeof(QuickSlot));

            Slot mySlot = null;
            foreach (Slot slot in slots)
            {
                if (slot.gameObject.activeSelf == false)
                {
                    break;
                }

                Vector2 size = slot.gameObject.GetComponent<RectTransform>().sizeDelta;
                Vector3 slotPosition = slot.transform.position;

                if(slotPosition.x <= position.x && slotPosition.x + size.x >= position.x &&
                    slotPosition.y <= position.y && slotPosition.y + size.y >= position.y)
                {
                    mySlot = slot;
                    DoWhenSlotClick(mySlot);
                    break;
                }


                if (slot.transform.position == position)
                {
                    
                }
            }

            /*QuickSlot myQSlot = null;
            foreach (QuickSlot qSlot in qSlots)
            {
                if (qSlot.transform.position == position)
                {
                    myQSlot = qSlot;
                    break;
                }
            }*/
        }
    }

    private void DoWhenSlotClick(Slot slot)
    {
        DestroySelectedIcon();

        if (slot.itemIcon == null)
        {
            return;
        }
        clickedItemIcon = Instantiate(slot.itemIcon, slot.transform.position, Quaternion.identity);
        clickedItemIcon.transform.localScale = transform.localScale;
        GraphicRaycaster raycaster = clickedItemIcon.GetComponent<GraphicRaycaster>();
        if (raycaster == null)
        {
            raycaster = clickedItemIcon.gameObject.AddComponent<GraphicRaycaster>();
        }
        raycaster.enabled = false;

        Color tmpColor = clickedItemIcon.color;
        tmpColor.a = 0.5f;

        // 이미지의 컬러의 a값을 직접 바꿀 수가 없어서 임시로 담은 컬러를 변경 후 덮어씀
        clickedItemIcon.color = tmpColor;
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
        Vector2 mousePos = Input.mousePosition;

        clickedItemIcon.transform.position = mousePos;
    }

}
