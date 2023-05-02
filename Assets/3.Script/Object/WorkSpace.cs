using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkSpace : MonoBehaviour
{
    public CraftUI craftUI;

    private void Awake()
    {
        craftUI.CloseUI();
    }

    public void Open()
    {
        // UI OPEN
        craftUI.OpenUI();

        Debug.Log("WorkSpace Open");
    }
    public void tmpFunc()
    {
        //inventory.AddItem
        
    }
}
