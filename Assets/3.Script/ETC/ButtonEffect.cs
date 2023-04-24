using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonEffect : MonoBehaviour
{
    public Sprite baseImage = null;
    public Sprite cursorOnImage = null;

    private void Awake()
    {
        baseImage = GetComponent<Image>().sprite;
    }
    private void Update()
    {
        if(EventSystem.current.IsPointerOverGameObject())
        {
            GetComponent<Image>().sprite = cursorOnImage;
        }
        else
        {
            gameObject.GetComponent<Image>().sprite = baseImage;
        }
    }

    // 이벤트가 작동을 안함
    private void OnMouseEnter()
    {
        GetComponent<Image>().sprite = cursorOnImage;
    }
    private void OnMouseExit()
    {
        gameObject.GetComponent<Image>().sprite = baseImage;
    }
}
