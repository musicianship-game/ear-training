using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BellBoss : MonoBehaviour {

    public GameObject[] enemyPrefabs = new GameObject[2];
    private List<Vector3> spawnPoints = new List<Vector3>();
    public PlayerController playerRef;
    public bool all_enemies_dead;
    public int number_of_enemies = 2;

    const bool ATTACK = true;
    const bool DEFENSE = false;
    Stack<string> phases;
    private string currentPhase = "";

    public class Minion
    {
        public int scaleDegree;
        public int alteration;
        public GameObject obj;
    }

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
        for (int i = 0; i < number_of_enemies; i++)
        {
            int j = Random.Range(0, spawnPoints.Count);
            Vector3 uniqueSpawnPoint = spawnPoints[j];
            spawnPoints.RemoveAt(j);
            GameObject newEnemy = Instantiate(enemyPrefabs[Random.Range(0, enemyPrefabs.Length)], uniqueSpawnPoint, transform.rotation, transform);
            newEnemy.GetComponent<Enemy>().player = playerRef;
        }
        all_enemies_dead = false;

        phases = new Stack<string>();
        phases.Push("3");
        phases.Push("2");
        phases.Push("1");
        phases.Push("intro");
        currentPhase = phases.Pop();
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

    private void InstantiateEnemies()
    {

    }

    private void SetStage(string stageName)
    {
        switch(stageName)
        {
            case "intro":
                //something
                break;
            case "1":
                // something
                break;
            case "2":
                // something
                break;
            case "3":
                // something
                break;
        }
    }

    // Signals
    public void AttackConcludedSignal(GameObject x)
    {
        // Called when a BellEnemy has concluded its attack routine
    }

    public void BellHitSignal(GameObject x)
    {
        // Called when a BellEnemy was hit by the enemy
    }

    public void PlayerMissedSignal(GameObject x)
    {
        // Called when a Player attempted to attack a BellEnemy but failed (wrong note)
    }
}
