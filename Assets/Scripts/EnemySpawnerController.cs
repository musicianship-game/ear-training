using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnerController : MonoBehaviour {

    public GameObject[] enemyPrefabs = new GameObject[2];
    private List<Vector3> spawnPoints = new List<Vector3>();
    public PlayerController playerRef;
    public bool all_enemies_dead;
    private int number_of_enemies = 3;
    public int max_enemies_count = 4;
    public int min_enemies_count = 2;

	void Awake ()
	{
        // there is an important assumption for the intialization:
        // at first, all children of spawner are spawn point markers
        // then we delete these, and all new children will be spawned enemies
        foreach (Transform spawnPoint in transform)
        {
            spawnPoints.Add(spawnPoint.position);
            Destroy(spawnPoint.gameObject);
        }
        ApplyDifficulty();
        for (int i = 0; i < number_of_enemies; i++)
        {
            int j = Random.Range(0, spawnPoints.Count);
            Vector3 uniqueSpawnPoint = spawnPoints[j];
            spawnPoints.RemoveAt(j);
            GameObject newEnemy = Instantiate(enemyPrefabs[Random.Range(0, enemyPrefabs.Length)], uniqueSpawnPoint, transform.rotation, transform);
            newEnemy.GetComponent<Enemy>().player = playerRef;
        }
        all_enemies_dead = false;
	}

    private void Update()
    {
        if (transform.childCount == 0 && !all_enemies_dead)
        {
            all_enemies_dead = true;
            playerRef.AllEnemiesDefeated();
        }
    }

    public void RunChuckCode(string code)
	{
		GetComponent<ChuckSubInstance>().RunCode(code);
	}

    private void ApplyDifficulty()
    {
        int range_num = max_enemies_count - min_enemies_count;
        float bin_size = 1.0f / (range_num + 1.0f);
        for (int i = range_num; i >= 0; i--)
        {
            if (Settings.GameDifficulty >= i * bin_size)
            {
                number_of_enemies = min_enemies_count + i;
                break;
            }
        }
    }
}
