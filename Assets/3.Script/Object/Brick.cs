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

    public GameObject fieldItemPrefab; // 블럭 파괴시 드랍할 아이템
    // Start is called before the first frame update
    void Start()
    {
        TryGetComponent(out tileMap);

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
        Vector3Int brickPosition = tileMap.WorldToCell(Pos);

        int tileIndex = this.brickPosition.FindIndex(vector => vector.Equals(brickPosition));
        if(tileIndex < 0)
        {
            return;
        }
        brickCurrentHp[tileIndex] -= damage;
        if(brickCurrentHp[tileIndex] <= 0 && tileMap.GetTile<Tile>(brickPosition) != null)
        {
            DropBrick(brickPosition);
            tileMap.SetTile(brickPosition, null);
        }
    }

    private void DropBrick(Vector3Int brickPosition)
    {
        GameObject obj = Instantiate(fieldItemPrefab, tileMap.CellToWorld(brickPosition), Quaternion.identity);
        Item brickFieldItem = new Item("brick", tileMap.GetTile<Tile>(brickPosition).sprite, 1);
        obj.transform.localScale = new Vector3(1f, 1f);
        obj.GetComponent<FieldItem>().SetItem(brickFieldItem);
    }
}
