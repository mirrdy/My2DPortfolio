using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftUI : MonoBehaviour
{
    public GameObject craftPanel;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OpenUI()
    {
        craftPanel.SetActive(true);
    }
    public void CloseUI()
    {
        craftPanel.SetActive(false);
    }
    
}
