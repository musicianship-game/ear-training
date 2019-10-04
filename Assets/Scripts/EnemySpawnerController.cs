using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnerController : MonoBehaviour {
	public GameObject enemyPrefab;
	public PlayerController playerRef;

	void Start ()
	{
		GameObject child = Instantiate(enemyPrefab, transform);
		child.GetComponent<Enemy>().player = playerRef;
	}

	public void RunChuckCode(string code)
	{
		GetComponent<ChuckSubInstance>().RunCode(code);
	}

	public List<Enemy> Resonate(float frequency)
	{
		List<Enemy> hits = new List<Enemy>();
		foreach (Transform child in transform)
		{
			Enemy enemy = child.GetComponent<Enemy>();
			float enemyFrequency = enemy.GetNoteFrequency();
			if (Mathf.Abs(frequency - enemyFrequency) < 100f)
			{
				hits.Add(enemy);
			}
		}
		return hits;
	}
}
