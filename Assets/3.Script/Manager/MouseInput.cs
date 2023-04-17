using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseInput : MonoBehaviour
{
    Vector3 MousePosition;
    public LayerMask layerMask;

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            MousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition) + new Vector3(0,0,10);
            Collider2D overCollider2d = Physics2D.OverlapCircle(MousePosition, 0.01f, layerMask);
            if(overCollider2d != null)
            {
                overCollider2d.transform.GetComponent<Brick>().TakeDamegeDot(MousePosition, 1);
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(MousePosition, 0.05f);
    }
}
