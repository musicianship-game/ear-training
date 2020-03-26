using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BellBoss : MonoBehaviour {

    public GameObject[] enemyPrefabs = new GameObject[2];
    private List<Vector3> spawnPoints = new List<Vector3>();
    public PlayerController playerRef;
	public float centsTolerance;
    public bool all_enemies_dead;
    public int number_of_enemies = 2;

	void Awake ()
	{
        foreach (Transform spawnPoint in transform)
        {
            spawnPoints.Add(spawnPoint.position);
            Destroy(spawnPoint.gameObject);
        }
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
