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

	public List<Enemy> Resonate(int scaleDegree, int alteration)
	{
		List<Enemy> hits = new List<Enemy>();
		noteName = Scale.GetNoteName(scaleDegree, alteration);
        noteFreq = Scale.GetNoteFrequency(scaleDegree, alteration);
		foreach (Transform child in transform)
		{
			Enemy enemy = child.GetComponent<Enemy>();
			float enemyFrequency = enemy.GetNoteFrequency();
			if (Mathf.Abs(noteFreq - enemyFrequency < 100))
			{
				hits.Add(enemy);
			}
		}
		return hits;
	}
}
