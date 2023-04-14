using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ClickManager : MonoBehaviour
{
    private void Update()
    {
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
                if (slot.transform.position == position)
                {
                    mySlot = slot;
                    break;
                }
            }

            QuickSlot myQSlot = null;
            foreach (QuickSlot qSlot in qSlots)
            {
                if (qSlot.transform.position == position)
                {
                    myQSlot = qSlot;
                    break;
                }
            }

        }
    }
}
