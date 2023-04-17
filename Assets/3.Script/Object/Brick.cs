using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Brick : MonoBehaviour
{
    public Tilemap tileMap;
    public float brickMaxHp;
    private float[] brickCurrentHp;
    private List<Vector3> brickPosition;
    private int brickCount;
    // Start is called before the first frame update
    void Start()
    {
        tileMap = GetComponent<Tilemap>();

        brickPosition = new List<Vector3>();

        // 타일맵의 모든 타일들의 포지션 가져오기
        foreach (var position in tileMap.cellBounds.allPositionsWithin)
        {
            brickPosition.Add(position);
        }


        brickCount = brickPosition.Count;

        brickCurrentHp = new float[brickCount];
        for (int i = 0; i < brickCount; i++)
        {
            brickCurrentHp[i] = brickMaxHp;
        }
    }

    public void TakeDamegeDot(Vector3 Pos, float damage)
    {
        Vector3Int cellPosition = tileMap.WorldToCell(Pos);

        int tileIndex = brickPosition.FindIndex(vector => vector.Equals(cellPosition));
        if(tileIndex < 0)
        {
            return;
        }
        brickCurrentHp[tileIndex] -= damage;
        if(brickCurrentHp[tileIndex] <= 0)
        {
            tileMap.SetTile(cellPosition, null);

            DropBrick(cellPosition);
        }
    }

    private void DropBrick(Vector3Int brickPosition)
    {
        //GameObject item;

        //item = Instantiate(tileMap.GetSprite(brickPosition));
        //item.transform.parent = null;
        //item.transform.position = gameObject.transform.position;

        //obj.GetComponent<FieldItem>().SetItem(itemDB[Random.Range(0, 3)]);
    }
}
