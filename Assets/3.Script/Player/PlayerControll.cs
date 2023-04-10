using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControll : MonoBehaviour
{
    private Movement2D movement2D;
    
    public int speed = 5;

    private void Awake()
    {
        TryGetComponent(out movement2D);
    }
    private void Start()
    {
        if (movement2D.moveSpeed <= 0f)
        {
            movement2D.moveSpeed = 5f;
        }
    }

    // Update is called once per frame
    void Update()
    {

        if (gameObject.CompareTag("Player"))
        {
            //Vector3 direction = new Vector3(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"), 0);
            //transform.position = Vector3.MoveTowards(transform.position, transform.position + direction, speed * Time.deltaTime);

            Vector2 dirVector = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized * speed * Time.deltaTime;
            transform.position += (Vector3)dirVector;
        }
        else
        {
            float x = Input.GetAxisRaw("Horizontal");
            float y = Input.GetAxisRaw("Vertical");

            movement2D.MoveTo(new Vector3(x, y, 0));
        }

        if (Input.GetKeyDown(KeyCode.Space) )
        {
            
        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
           
        }
    }
}
