using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bell : MonoBehaviour {

    public PlayerController player;
    public float frequency = 0;
    public bool targetable = false;

    public float attack_dur = 1.51f;
    public float attack_start;
    public float attack_rate = 0.2f;
    public float projectile_speed = 2.0f;
    public GameObject projectile_used = null;
    public int projectile_damage = 1;

    private int atk_seq_n = -1;
    private bool attacking = false;

    private BellBoss bell_boss;
    private ThisTarget this_target;
    


    // Use this for initialization
    void Start () {
        bell_boss = GetComponentInParent<BellBoss>();
        this_target = GetComponent<ThisTarget>();
        this_target.frequency = frequency;
        this_target.targetable = targetable;
    }
	
	// Update is called once per frame
	void Update () {
        this_target.targetable = targetable;
        if (attacking)
        {
            if (Time.time >= attack_start + attack_dur)
            {
                attacking = false;
                bell_boss.AttackConcludedSignal(this.gameObject);
            }
            else
            {
                AttackSequence01(attack_start);
            }
        }
    }

    public void Attack()
    {
        attack_start = Time.time;
        attacking = true;
        atk_seq_n = -1;
        // play the sound of attacking here
    }

    public void ResonatorUpdate(bool b)
    {
        if (b)
        {
            bell_boss.BellHitSignal(this.gameObject);
        }
        else if (!b)
        {
            bell_boss.PlayerMissedSignal(this.gameObject);
        }
    }

    private void AttackSequence01(float atk_strt)
    {
        int attack_angle_spread = 120;
        int N = Mathf.FloorToInt(attack_dur / attack_rate);
        float angle_diff = attack_angle_spread / (N * 2);
        int n = Mathf.FloorToInt((Time.time - atk_strt) / attack_rate);
        float spawn_angle = n * angle_diff - 90.0f;
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
}
