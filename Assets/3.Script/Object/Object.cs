using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Object : MonoBehaviour
{
    public int maxHp;
    public int currentHp;

    [SerializeField] GameObject[] dropTable;

    private void Awake()
    {
        currentHp = maxHp;
        //TryGetComponent(out dropTable);
    }
    
    public void TakeDamage(int damage)
    {
        if(currentHp <= 0)
        {
            return;
        }
        currentHp -= damage;
        if(currentHp <= 0)
        {
            DestroyThisObject();
        }
    }
    public abstract void DestroyThisObject();
    private void DropItem()
    {
        GameObject item;
        FieldItem fieldItem;
        for(int i=0; i<dropTable.Length; i++)
        {
            dropTable[i].TryGetComponent(out fieldItem);
            int roll = Random.Range(0, 100);
            
            if(roll < fieldItem.dropProb)
            {
                Debug.Log($"Roll:{roll}, dropProb:{fieldItem.dropProb} - 생성");
                item = Instantiate(dropTable[i]);
                item.transform.parent = null;  // 월드기준좌표로 드랍하기위해 부모설정 삭제

                float randomX = Random.Range(-0.5f, 0.5f);
                float randomY = Random.Range(-0.5f, 0.5f);
                item.transform.position = gameObject.transform.position + new Vector3(randomX, randomY);

            }
            else
            {
                Debug.Log($"Roll:{roll}, dropProb:{fieldItem.dropProb} - 미생성");
            }
        }
    }
    private void Destroy()
    {
        Destroy(gameObject);
    }
    private void ActiveOff()
    {
        gameObject.SetActive(false);
    }
}
