using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnerController : MonoBehaviour {

    public GameObject[] enemyPrefabs = new GameObject[2];
    private Vector3[] spawnPoints;
    public PlayerController playerRef;
	public float centsTolerance;
    public bool all_enemies_dead;
    public int number_of_enemies = 2;

	void Awake ()
	{
        // there is an important assumption for the intialization:
        // at first, all children of spawner are spawn point markers
        // then we delete these, and all new children will be spawned enemies
        spawnPoints = new Vector3[transform.childCount];
        int spawn_i = 0;
        foreach (Transform spawnPoint in transform)
        {
            spawnPoints[spawn_i] = spawnPoint.position;
            Destroy(spawnPoint.gameObject);
            spawn_i++;
        }
        List<Vector3> spawnPointsList = new List<Vector3>(spawnPoints);
        for (int i = 0; i < number_of_enemies; i++)
        {
            int j = Random.Range(0, spawnPointsList.Count);
            Vector3 uniqueSpawnPoint = spawnPointsList[j];
            spawnPointsList.RemoveAt(j);
            GameObject newEnemy = Instantiate(enemyPrefabs[Random.Range(0, enemyPrefabs.Length)], uniqueSpawnPoint, transform.rotation, transform);
            newEnemy.GetComponent<Enemy>().player = playerRef;
        }
        all_enemies_dead = false;
	}

    private void Update()
    {
        if (transform.childCount == 0)
        {
            all_enemies_dead = true;
        }
    }

    public void RunChuckCode(string code)
	{
		GetComponent<ChuckSubInstance>().RunCode(code);
	}

	public List<Enemy> Resonate(float playerFrequency)
	{
		List<Enemy> hits = new List<Enemy>();
		foreach (Transform child in transform)
		{
			Enemy enemy = child.GetComponent<Enemy>();
			float enemyFrequency = enemy.GetNoteFrequency();
			float centsDifference = 1200 * Mathf.Log(enemyFrequency / playerFrequency, 2);
			if (Mathf.Abs(centsDifference) < centsTolerance)
			{
				hits.Add(enemy);
			}
		}
		return hits;
	}
}
