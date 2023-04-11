using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Object : MonoBehaviour
{
    [SerializeField] private int maxHp;
    private int currentHp;

    [SerializeField] GameObject[] dropTable;

    private void Awake()
    {
        currentHp = maxHp;
        //TryGetComponent(out dropTable);
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        DropItem();
        Destroy(gameObject);
    }
    
    public void GetDamage(int damage)
    {
        currentHp -= damage;
        if(currentHp <= 0)
        {
            DestroyThisObject();
        }
    }
    private void DestroyThisObject()
    {
        DropItem();
    }
    private void DropItem()
    {
        GameObject item;
        for(int i=0; i<dropTable.Length; i++)
        {
            item = Instantiate(dropTable[i]);
            item.transform.parent = null;
            item.transform.position = gameObject.transform.position;
        }
    }
}
