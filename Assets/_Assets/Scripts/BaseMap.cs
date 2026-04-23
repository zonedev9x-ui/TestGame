using System.Collections;
using UnityEngine;

public class BaseMap : MonoBehaviour
{
    public Transform SpawnPointEnemy;
    public Transform EndPoint;

    [Header("Enemy Prefab")]
    public BaseEnemy enemyPrefab;

    [Header("Wave Settings")]
    public int waveCount;
    public int enemiesPerWave;
    public float timeBetweenWaves;
    public float enemySpacing;

    private void OnEnable()
    {
        StartCoroutine(SpawnWavesCoroutine());
    }

    private IEnumerator SpawnWavesCoroutine()
    {
        for (int w = 0; w < waveCount; w++)
        {
            float totalWidth = (enemiesPerWave - 1) * enemySpacing;
            float startOffsetX = -totalWidth / 2f;

            for (int i = 0; i < enemiesPerWave; i++)
            {
                float offsetX = startOffsetX + (i * enemySpacing);
                Vector3 spawnPos = SpawnPointEnemy.position + SpawnPointEnemy.right * offsetX;
                SpawnEnemy(spawnPos);
            }

            if (w < waveCount - 1)
            {
                yield return new WaitForSeconds(timeBetweenWaves);
            }
        }
    }

    public BaseEnemy SpawnEnemy(Vector3 spawnPos)
    {
        if (enemyPrefab == null)
        {
            return null;
        }

        if (SpawnPointEnemy == null)
        {
            return null;
        }

        BaseEnemy enemy = Instantiate(enemyPrefab, spawnPos, Quaternion.identity);
        enemy.Active(spawnPos);
        return enemy;
    }
}