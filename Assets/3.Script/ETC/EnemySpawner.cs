using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private int maxSpawnCount;
    [SerializeField] private float spawnRange;
    [SerializeField] private float spawnDelay;

    private GameObject[] enemies;

    private IEnumerator spawnEnemy_co;
    private void Awake()
    {
        enemies = new GameObject[maxSpawnCount];
        for(int i=0; i<maxSpawnCount; i++)
        {
            enemies[i] = Instantiate(enemyPrefab, transform.position, Quaternion.identity);
            enemies[i].transform.SetParent(transform);
            enemies[i].SetActive(false);
        }
        spawnEnemy_co = SpawnEnemy_co();
        StartCoroutine(spawnEnemy_co);
    }

    private IEnumerator SpawnEnemy_co()
    {
        WaitForSeconds spawnDelay = new WaitForSeconds(this.spawnDelay);
        while(true)
        {
            for(int i=0; i<maxSpawnCount; i++)
            {
                if (enemies[i].activeSelf == false)
                {
                    Vector2 spawnPos = new Vector2(
                        transform.position.x + Random.Range(-spawnRange, spawnRange),
                        transform.position.y + Random.Range(-spawnRange, spawnRange));

                    enemies[i].transform.position = spawnPos;
                    enemies[i].SetActive(true);
                }
            }

            yield return spawnDelay;
        }
    }
}
