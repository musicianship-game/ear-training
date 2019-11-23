using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnerController : MonoBehaviour {
	public GameObject enemyPrefab;
	public PlayerController playerRef;
	public float centsTolerance;
    public bool all_enemies_dead;

	void Awake ()
	{
		GameObject child = Instantiate(enemyPrefab, transform);
		child.GetComponent<Enemy>().player = playerRef;
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
