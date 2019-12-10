using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnerController : MonoBehaviour {
    public GameObject enemyPrefab0;
    public GameObject enemyPrefab1;
    public GameObject enemyPrefab2;
    public GameObject enemyPrefab3;
    private GameObject[] enemyPrefabs = new GameObject[4];
    private Vector3[] spawnPoints;
    public PlayerController playerRef;
	public float centsTolerance;
    public bool all_enemies_dead;
    public int number_of_enemies = 2;

	void Awake ()
	{
        // there is an important assumption for the intialization:
        // at first, all children are spawn point markers
        // then we delete these, and all new children are spawned enemies
        spawnPoints = new Vector3[transform.childCount];
        int spawn_i = 0;
        foreach (Transform spawnPoint in transform)
        {
            spawnPoints[spawn_i] = spawnPoint.position;
            Destroy(spawnPoint.gameObject);
            spawn_i++;
        }
        enemyPrefabs[0] = enemyPrefab0;
        enemyPrefabs[1] = enemyPrefab1;
        enemyPrefabs[2] = enemyPrefab2;
        enemyPrefabs[3] = enemyPrefab3;
        for (int i = 0; i < number_of_enemies; i++)
        {
            GameObject newEnemy = Instantiate(enemyPrefabs[Random.Range(0, 4)], spawnPoints[Random.Range(0, spawn_i)], transform.rotation, transform);
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
