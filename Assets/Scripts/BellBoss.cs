using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BellBoss : MonoBehaviour {
    // public PlayerController playerRef;

    public Bell enemyPrefab;
    enum Mode { Offense, Defense, Transitioning, Dying};
    Mode mode;
    private bool shouldAttack;
    private bool shouldMakeVulnerable;
    Stack<string> phases;
    List<Note> enemies;
    private int enemyPointer = -1;
    private string currentPhase = "";
    float timer = 0f;
    const float timeout = 10f;

    public class Note
    {
        public int scaleDegree;
        public int alteration;
        public float frequency;
        public Bell bell;
        public Note(int sd, int alt)
        {
            scaleDegree = sd;
            alteration = alt;
            frequency = Scale.GetNoteFrequency(sd, alt);
        }
    }

    private void InstantiateEnemies()
    {
        GameObject background = GameObject.Find("Background");
        RectTransform rt = (RectTransform)background.transform;              
        float width = rt.rect.width / 20f; // No idea why that "20f" works. 
        // It doesn't match the pixelsperunit or localscale or other things I tried
        float x0 = -width / 2f;
        float offset = width / enemies.Count;
        float padding = offset / 2f;
        for (int i = 0; i < enemies.Count; i++)
        {
            //Debug.Log("Background width " + width);
            enemies[i].bell = Instantiate(
                enemyPrefab,
                new Vector3(x0 + padding + (i * offset), 0, 0),
                Quaternion.identity,
                transform
            );
            enemies[i].bell.SetFrequency(enemies[i].frequency);
        }
    }    

    private void ClearBells()
    {
        for (int i = 0; i < enemies.Count; i++)
        {
            Destroy(enemies[i].bell.gameObject);
        }
    }

	void Awake ()
	{
        enemies = new List<Note>();
        phases = new Stack<string>();
        phases.Push("end");
        phases.Push("3");
        phases.Push("2");
        phases.Push("1");
        // phases.Push("intro");
        currentPhase = phases.Pop();
        SetPhase(currentPhase);        
	}

    private void SetOffenseMode()
    {
        foreach (Note note in enemies)
        {
            note.bell.SetTargetable(false);
            note.bell.HasShield(true);
        }
        mode = Mode.Offense;
        shouldAttack = true;
        enemyPointer = 0;
    }

    private void SetDefenseMode()
    {
        mode = Mode.Defense;
        shouldMakeVulnerable = true;
        enemyPointer = 0;
        timer = 0f;
    }

    private void Update()
    {
        if (mode == Mode.Offense)
        {
            UpdateOffense();
        }
        else if (mode == Mode.Defense)
        {
            UpdateDefense();
        }
        else if (mode == Mode.Dying)
        {
            Debug.Log("Aahh!!");
            SceneManager.LoadScene(8);
        }
    }

    private void UpdateOffense()
    {
        if (enemyPointer >= enemies.Count)
        {
            Debug.Log("BellBoss: Done attacking. I'm just gonna chill a bit...");
            SetDefenseMode();
        }
        else if (shouldAttack)
        {
            Debug.Log("Attacking with bell " + enemyPointer);
            enemies[enemyPointer].bell.Attack();
            shouldAttack = false;
        }
    }

    private void UpdateDefense()
    {
        if (enemyPointer >= enemies.Count)
        {            
            currentPhase = phases.Pop();
            Debug.Log("Transitioning to phase " + currentPhase);
            SetPhase(currentPhase);
        }
        else if (timer >= timeout)
        {
            Debug.Log("BellBoss: Timeout! I'ma attack you again! Bahaha!");
            SetOffenseMode();
        }
        else if (shouldMakeVulnerable)
        {
            Debug.Log("Bell " + enemyPointer + " is vulnerable!");
            enemies[enemyPointer].bell.SetTargetable(true);
            enemies[enemyPointer].bell.HasShield(false);
            shouldMakeVulnerable = false;            
        }
        else {
            timer += Time.deltaTime;
        }
    }

    public void RunChuckCode(string code)
	{
		GetComponent<ChuckSubInstance>().RunCode(code);
	}

    private void SetPhase(string phaseName)
    {
        Debug.Log("BellBoss: Phase" + phaseName);
        switch(phaseName)
        {
            case "intro":
                //something
                break;
            case "1":
                enemies.Clear();
                enemies.Add(new Note(2, 0));
                enemies.Add(new Note(1, 0));
                enemies.Add(new Note(0, 0));
                InstantiateEnemies();
                SetOffenseMode();
                break;
            case "2":
                ClearBells();
                enemies.Clear();
                enemies.Add(new Note(3, 0));
                enemies.Add(new Note(2, 0));
                enemies.Add(new Note(1, 0));
                enemies.Add(new Note(0, 0));
                InstantiateEnemies();
                SetOffenseMode();
                break;
            case "3":
                ClearBells();
                enemies.Clear();
                enemies.Add(new Note(4, 0));
                enemies.Add(new Note(5, 0));
                enemies.Add(new Note(3, 0));
                enemies.Add(new Note(2, 0));
                enemies.Add(new Note(1, 0));
                enemies.Add(new Note(0, 0));
                InstantiateEnemies();
                SetOffenseMode();
                break;
            case "end":
                Debug.Log("BellBoss: Oh no!!! I am DYING!");
                mode = Mode.Dying;
                break;
        }
    }

    // Signals
    public void AttackConcludedSignal(GameObject x)
    {
        // Called when a BellEnemy has concluded its attack routine
        enemyPointer++;
        shouldAttack = true;
    }

    public void BellHitSignal(GameObject x)
    {
        // Called when a BellEnemy was hit by the enemy
        enemies[enemyPointer].bell.SetTargetable(false);
        enemies[enemyPointer].bell.HasShield(true);
        enemyPointer++;
        shouldMakeVulnerable = true;
    }

    public void PlayerMissedSignal(GameObject x)
    {
        // Called when a Player attempted to attack a BellEnemy but failed (wrong note)
        SetOffenseMode();
    }
}
