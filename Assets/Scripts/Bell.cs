using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bell : MonoBehaviour {

    public PlayerController player;
    public float frequency = 0;
    public bool targetable = false;
    public float spawn_dur = 1.0f;
    private bool spawning = true;
    private float spawn_start;
    private bool spawn_reverse;
    private Vector3 spawn_a;
    private Vector3 spawn_b;
    private bool kill_me = false;

    public float attack_dur = 0.7f;
    private float attack_start;
    public float attack_rate = 0.3f;
    private float projectile_speed = 2.0f;
    public float proj_speed_hard = 5.0f;
    public float proj_speed_easy = 1.5f;
    public int attack_angle_spread = 120;
    public float angle_rand = 5.0f;
    public GameObject projectile_used = null;
    public int projectile_damage = 1;
    GameObject bubble;
    GameObject bell1;
    public GameObject note_spray;

    private int atk_seq_n = -1;
    private bool attacking = false;

    private BellBoss bell_boss;
    private ThisTarget this_target;    
    


    // Use this for initialization
    void Awake () {
        bell_boss = GetComponentInParent<BellBoss>();
        this_target = GetComponent<ThisTarget>();
        this_target.frequency = frequency;
        this_target.targetable = targetable;
        bubble = transform.Find("bubble").gameObject;
        bell1 = transform.Find("bell_1").gameObject;
        ApplyDifficulty();
        Spawn(false);
    }

    public void SetFrequency(float freq)
    {
        frequency = freq;
        this_target.frequency = frequency;
    }

    public void SetTargetable(bool targ)
    {
        targetable = targ;
        this_target.targetable = targetable;
    }
	
	// Update is called once per frame
	void Update () {
        this_target.targetable = targetable;
        if (kill_me)
        {
            Destroy(this.gameObject);
        }
        if (attacking)
        {
            AttackSequence01(attack_start);
        }
        if (spawning)
        {
            SpawnSequence(spawn_start);
        }
    }

    public void Attack()
    {
        attack_start = Time.time;
        attacking = true;
        atk_seq_n = -1;
        bell_boss.RunChuckCode(ChuckSynths.Bell(frequency, attack_dur));
        Instantiate(note_spray, transform);
    }

    public void ResonatorUpdate(bool b)
    {
        if (b)
        {
            bell_boss.BellHitSignal(this.gameObject);
            bell_boss.RunChuckCode(ChuckSynths.BG_Plucked_String(frequency, attack_dur));
        }
        else if (!b)
        {
            bell_boss.PlayerMissedSignal(this.gameObject);
        }
    }

    private void AttackSequence01(float atk_strt)
    {
        if (Time.time >= atk_strt + attack_dur)
        {
            attacking = false;
            bell_boss.AttackConcludedSignal(this.gameObject);
            return;
        }
        int N = Mathf.FloorToInt(attack_dur / attack_rate);
        float angle_diff = attack_angle_spread / (N * 2);
        int n = Mathf.FloorToInt((Time.time - atk_strt) / attack_rate);
        float spawn_angle = n * angle_diff + Random.Range(-angle_rand, angle_rand) - 90.0f;
        if (n == atk_seq_n)
        {
            return;
        }
        else
        {
            atk_seq_n = n;
            FireProjectile(spawn_angle, false);
            if (n != 0)
            {
                FireProjectile(spawn_angle, true);
            }
        }

    }

    void FireProjectile(float spawn_angle , bool mirror)
    {
        int m = 1;
        if (mirror) { m = -1; }
        Vector2 my_pos = new Vector2(transform.position.x, transform.position.y);
        Vector2 direction = new Vector2(m * Mathf.Cos(spawn_angle * Mathf.PI / 180.0f), Mathf.Sin(spawn_angle * Mathf.PI / 180.0f));
        Quaternion rotation = Quaternion.Euler(0, 0, Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg + 90.0f);
        GameObject the_projectile = (GameObject)Instantiate(projectile_used, my_pos, rotation);
        the_projectile.GetComponent<Rigidbody2D>().velocity = direction * projectile_speed;
        the_projectile.GetComponent<Projectile>().damage = projectile_damage;
    }

    public void HasShield(bool value)
    {
        SpriteRenderer sr = bubble.GetComponent<SpriteRenderer>();
        sr.enabled = value;
    }

    public void FastTwist(bool value)
    {
        TwistBehavior t_b = bell1.GetComponent<TwistBehavior>();
        t_b.fast = value;
    }

    private void SpawnSequence(float spwn_strt)
    {
        float s = (Time.time - spwn_strt) / spawn_dur;
        if (s > 1.0f)
        {
            spawning = false;
            if (!spawn_reverse)
            {
                transform.position = spawn_b;
            }
            else
            {
                kill_me = true;
                return;
            }
            UpdateFloatBehavior();
            return;
        }
        if (!spawn_reverse)
        {
            s = 1.0f - Mathf.Pow(1.0f - s, 3.0f); // some smoothing
            transform.position = Vector3.Lerp(spawn_a, spawn_b, s);
        }
        else
        {
            s = Mathf.Pow(s, 3.0f); // some smoothing
            transform.position = Vector3.Lerp(spawn_b, spawn_a, s);
        }
    }

    private void UpdateFloatBehavior()
    {
        FloatBehavior f_b = GetComponent<FloatBehavior>();
        if (f_b != null)
        {
            f_b.enabled = !spawning;
        }
    }

    public void Spawn(bool reverse = false)
    {
        spawning = true;
        spawn_reverse = reverse;
        spawn_start = Time.time;
        spawn_b = transform.position; // this is the target on-screen position, where this object was originally spawned (or is if reversed)
        Vector2 screenBounds = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));
        transform.position += new Vector3 (0, screenBounds[1], 0);
        spawn_a = transform.position; // this is the off-screen position where the animation will start (or end, if reversed)
        UpdateFloatBehavior();
    }

    public void ApplyDifficulty()
    {
        float max_diff_speed = proj_speed_easy - proj_speed_hard;
        float diff_speed = max_diff_speed * Settings.GameDifficulty;
        projectile_speed = proj_speed_easy - diff_speed;
    }
}
