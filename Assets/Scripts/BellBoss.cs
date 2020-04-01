using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BellBoss : MonoBehaviour {
    // public PlayerController playerRef;

    public Bell enemyPrefab;

    const bool OFFENSE = true;
    const bool DEFENSE = false;
    private bool mode;
    private bool shouldAttack;
    Stack<string> phases;
    List<Note> enemies;
    private int enemyPointer = -1;
    private string currentPhase = "";
    float timer = 0f;
    const float timeout = 2f;

    public class Note
    {
        public int scaleDegree;
        public int alteration;
        public Bell bell;
        public Note(int sd, int alt)
        {
            scaleDegree = sd;
            alteration = alt;
        }
    }

	void Awake ()
	{
        phases = new Stack<string>();
        phases.Push("end");
        phases.Push("3");
        phases.Push("2");
        phases.Push("1");
        // phases.Push("intro");
        currentPhase = phases.Pop();
        SetPhase(currentPhase);
        mode = OFFENSE;
        shouldAttack = true;
	}

    private void Update()
    {
        if (mode == OFFENSE)
        {
            UpdateOffense();
        }
        else if (mode == DEFENSE)
        {
            UpdateDefense();
        }
    }

    private void UpdateOffense()
    {
        if (enemyPointer >= enemies.Count)
        {
            mode = DEFENSE;
            enemyPointer = 0;
        }
        else if (shouldAttack)
        {
            enemies[enemyPointer].bell.Attack();
            enemyPointer++;
            shouldAttack = false;
        }
    }

    private void UpdateDefense()
    {
        if (enemyPointer >= enemies.Count)
        {
            currentPhase = phases.Pop();
            if (currentPhase == "end")
            {
                // DIE
            }
            else
            {
                SetPhase(currentPhase);
                mode = OFFENSE;
                shouldAttack = true;
            }
        }
        else if (timer >= timeout)
        {
            foreach (Note note in enemies)
            {
                // note.bell.gameObject.targetable = false;
            }
            mode = OFFENSE;
            shouldAttack = true;
        }
        else
        {

        }
    }

    public void RunChuckCode(string code)
	{
		GetComponent<ChuckSubInstance>().RunCode(code);
	}

    private void SetPhase(string phaseName)
    {
        switch(phaseName)
        {
            case "intro":
                //something
                break;
            case "1":
                enemies.Clear();
                enemies.Add(new Note(5, 0));
                enemies.Add(new Note(2, 0));
                enemies.Add(new Note(1, 0));
                InstantiateEnemies();
                enemyPointer = 0;
                break;
            case "2":
                // something
                break;
            case "3":
                // something
                break;
        }
    }

    private void InstantiateEnemies()
    {
        for (int i = 0; i < enemies.Count; i++)
        {
            enemies[i].bell = Instantiate(
                enemyPrefab,
                new Vector3(i * 3.0f, 0, 0),
                Quaternion.identity,
                transform
            );
        }
    }

    // Signals
    public void AttackConcludedSignal(GameObject x)
    {
        // Called when a BellEnemy has concluded its attack routine
        shouldAttack = true;
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
