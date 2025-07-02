using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRespawn : MonoBehaviour
{
    [Header("Спавн врагов")]
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private int maxEnemies = 5;
    [SerializeField] private float spawnIntervalMinutes = 3f;
    [SerializeField] private Transform[] spawnPoints;

    private List<GameObject> spawnedEnemies = new List<GameObject>();

    private void Start()
    {
        InitialSpawn();

        StartCoroutine(SpawnEnemiesRoutine());
    }

    private void InitialSpawn()
    {
        for (int i = 0; i < maxEnemies; i++)
        {
            Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
            GameObject enemy = Instantiate(enemyPrefab, spawnPoint.position, Quaternion.identity);
            spawnedEnemies.Add(enemy);
        }
    }

    private IEnumerator SpawnEnemiesRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnIntervalMinutes * 60f);

            CleanUpDeadEnemies();

            int enemiesToSpawn = maxEnemies - spawnedEnemies.Count;

            for (int i = 0; i < enemiesToSpawn; i++)
            {
                Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
                GameObject enemy = Instantiate(enemyPrefab, spawnPoint.position, Quaternion.identity);
                spawnedEnemies.Add(enemy);
            }
        }
    }

    private void CleanUpDeadEnemies()
    {
        spawnedEnemies.RemoveAll(e => e == null);
    }
}