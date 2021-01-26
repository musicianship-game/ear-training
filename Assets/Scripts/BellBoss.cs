using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BellBoss : MonoBehaviour {
    // public PlayerController playerRef;

    public Bell enemyPrefab;
    enum Mode {Intro, Offense, Defense, Transition, Dying };
    Mode mode;
    BossFace bossFace;
    bool endTransition;
    bool shouldTransition;
    private bool shouldAttack;
    private bool shouldMakeVulnerable;
    Stack<string> phases;
    List<BellEnemy> enemies;
    private int enemyPointer = -1;
    private string currentPhase = "";
    float timer = 0f;
    float timeout = 10f;

    public class BellEnemy
    {
        public readonly MelodicSequences.Note note;
        public Bell bell;
        public BellEnemy(MelodicSequences.Note n)
        {
            note = n;
        }
    }

    private void InstantiateEnemies()
    {
        Vector2 screenBounds = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));
        float width = screenBounds.x * 2f;
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
            enemies[i].bell.SetFrequency(enemies[i].note.frequency);
        }
    }    

    private void ClearBells()
    {
        for (int i = 0; i < enemies.Count; i++)
        {
            enemies[i].bell.Spawn(true);
            //Destroy(enemies[i].bell.gameObject);
        }
    }

	void Awake ()
	{        
        enemies = new List<BellEnemy>();
        phases = new Stack<string>();
        endTransition = false;
        phases.Push("end");
        phases.Push("3");
        phases.Push("transition");
        phases.Push("2");
        phases.Push("transition");
        phases.Push("1");
        phases.Push("intro");
        currentPhase = phases.Pop();        
    }

    private void Start()
    {
        bossFace = transform.Find("boss_face").gameObject.GetComponent<BossFace>();
        SetPhase(currentPhase);
    }
    
    private void SetIntroMode()
    {
        mode = Mode.Intro;
        endTransition = false;
        shouldTransition = true;        
    }

    private void SetOffenseMode()
    {
        foreach (BellEnemy enemy in enemies)
        {
            enemy.bell.SetTargetable(false);
            enemy.bell.HasShield(true);
            enemy.bell.FastTwist(false);
        }
        mode = Mode.Offense;
        shouldAttack = true;
        enemyPointer = 0;
        timer = 0f;
        timeout = 3f * enemies.Count;
    }

    private void SetDefenseMode()
    {
        mode = Mode.Defense;
        shouldMakeVulnerable = true;
        enemyPointer = 0;
        timer = 0f;
    }

    private void SetTransitionMode()
    {
        mode = Mode.Transition;
        endTransition = false;
        shouldTransition = true;        
    }

    private void Update()
    {
        if (mode == Mode.Intro)
        {
            UpdateIntro();
        }
        else if (mode == Mode.Offense)
        {
            UpdateOffense();
        }
        else if (mode == Mode.Defense)
        {
            UpdateDefense();
        }
        else if (mode == Mode.Transition)
        {
            UpdateTransition();
        }
        else if (mode == Mode.Dying)
        {
            Debug.Log("Aahh!!");
            SceneManager.LoadScene(8);
        }     
    }

    private void UpdateIntro()
    {
        if (shouldTransition)
        {
            shouldTransition = false;
            bossFace.Boo();
        }
        if (endTransition)
        {
            currentPhase = phases.Pop();
            Debug.Log("Transitioning to phase " + currentPhase);
            SetPhase(currentPhase);
        }
    }

    private void UpdateOffense()
    {
        // Leave room for bells to finish their spawn animation
        timer += Time.deltaTime;
        if (timer > enemyPrefab.spawn_dur)
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

    private void UpdateTransition()
    {
        if (shouldTransition)
        {
            shouldTransition = false;
            bossFace.Boo();
        }
        if (endTransition)
        {
            currentPhase = phases.Pop();
            Debug.Log("Transitioning to phase " + currentPhase);
            SetPhase(currentPhase);
        }
    }

    public void RunChuckCode(string code)
	{
		GetComponent<ChuckSubInstance>().RunCode(code);
	}

    private void SetPhase(string phaseName)
    {
        Debug.Log("BellBoss: Phase" + phaseName);
        MelodicSequences.MelodicSequence seq = MelodicSequences.GetRandomSequence();
        int seqSize = seq.sequence.Count;
        switch (phaseName)
        {
            case "intro":                
                SetIntroMode();
                break;
            case "1":                
                enemies.Clear();               
                enemies.Add(new BellEnemy(seq.sequence[seqSize - 3]));
                enemies.Add(new BellEnemy(seq.sequence[seqSize - 2]));
                enemies.Add(new BellEnemy(seq.sequence[seqSize - 1]));
                InstantiateEnemies();
                SetOffenseMode();
                break;
            case "2":
                ClearBells();
                enemies.Clear();
                enemies.Add(new BellEnemy(seq.sequence[seqSize - 4]));
                enemies.Add(new BellEnemy(seq.sequence[seqSize - 3]));
                enemies.Add(new BellEnemy(seq.sequence[seqSize - 2]));
                enemies.Add(new BellEnemy(seq.sequence[seqSize - 1]));
                InstantiateEnemies();
                SetOffenseMode();
                break;
            case "3":
                ClearBells();
                enemies.Clear();
                enemies.Add(new BellEnemy(seq.sequence[seqSize - 6]));
                enemies.Add(new BellEnemy(seq.sequence[seqSize - 5]));
                enemies.Add(new BellEnemy(seq.sequence[seqSize - 4]));
                enemies.Add(new BellEnemy(seq.sequence[seqSize - 3]));
                enemies.Add(new BellEnemy(seq.sequence[seqSize - 2]));
                enemies.Add(new BellEnemy(seq.sequence[seqSize - 1]));
                InstantiateEnemies();
                SetOffenseMode();
                break;
            case "transition":
                ClearBells();
                enemies.Clear();
                SetTransitionMode();
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
        enemies[enemyPointer].bell.FastTwist(true);
        enemyPointer++;
        shouldMakeVulnerable = true;
    }

    public void PlayerMissedSignal(GameObject x)
    {
        // Called when a Player attempted to attack a BellEnemy but failed (wrong note)
        SetOffenseMode();
    }

    public void BooEndSignal()
    {
        endTransition = true;
    }
}
