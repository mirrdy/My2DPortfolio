using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveGameOver : MonoBehaviour
{
    bool upFlg;
    private void Awake()
    {
        upFlg = true;
        StartCoroutine("moveGameOver_co", 0.05f);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    private IEnumerator moveGameOver_co()
    {
        WaitForSeconds waitSec = new WaitForSeconds(0.3f);
        while (true)
        {
            if(upFlg == true)
            {
                transform.position += new Vector3(0, 0.04f);
            }
            else
            {
                transform.position -= new Vector3(0, 0.04f);
            }

            if(transform.position.y > 1.45f)
            {
                upFlg = false;
            }
            else if(transform.position.y < -1.45f)
            {
                upFlg = true;
            }
            
            yield return waitSec;
        }
    }
}
