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
    private Image clickedItemIcon;
    [SerializeField]private Text amountText;
    [SerializeField] private Transform canvasTransform;

    private float interval = 0.75f;
    private float doubleClickTime = 0;

    private bool isSelecting = false;

    private void Awake()
    {
        canvasTransform = GetComponentInParent<Canvas>().transform;
        //TryGetComponent(out amountText);
    }

    private void Update()
    {
        if (isSelecting)
        {
            UpdateMouseMoving();
        }
        else
        {

        }
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
        amountText.text = "0";
        itemIcon.gameObject.SetActive(false);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        Debug.Log("OnPointerUp - Start");
        if (isSelecting)
        {
            isSelecting = false;
            DestroySelectedIcon();
            Debug.Log("OnPointerUp - isSelecting");
        }

        if ((Time.time - doubleClickTime) < interval)
        {
            isSelecting = false;
            DestroySelectedIcon();

            doubleClickTime = 0;
            DoWhenDoubleClick();
            Debug.Log("OnPointerUp - double");
        }
        else
        {
            isSelecting = true;
            doubleClickTime = Time.time;
            DoWhenClick(eventData.position);
            Debug.Log("OnPointerUp - double else");
        }
    }
    
    private void DestroySelectedIcon()
    {
        if (clickedItemIcon != null)
        {
            Destroy(clickedItemIcon.gameObject);
        }
    }

    private void DoWhenClick(Vector3 clickPosition)
    {
        DestroySelectedIcon();
        clickedItemIcon = Instantiate(itemIcon, clickPosition, Quaternion.identity);
        Color tmpColor = clickedItemIcon.color;
        tmpColor.a = 0.5f;
        clickedItemIcon.transform.SetParent(canvasTransform);

        // 이미지의 컬러의 a값을 직접 바꿀 수가 없어서 임시로 담은 컬러를 변경 후 덮어씀
        clickedItemIcon.color = tmpColor;
    }
    private void DoWhenDoubleClick()
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
    private void UpdateMouseMoving()
    {
        Vector2 mousePos = Input.mousePosition;

        clickedItemIcon.transform.position = mousePos;
    }
    
}
