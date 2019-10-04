using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {
    private Rigidbody2D rb2d;
    private Animator spriteanimator;
    private float speed;
    private float horizontal;
    private float vertical;
    private bool jumping;
    private bool falling;
    private int health_max = 12;
    private int health;
    public Slider health_slider;
    public GameObject mouth_LA = null;
    public EnemySpawnerController spawner = null;
    public float projectile_speed = 3.5f;
    public PlayerProjectile projectile_used = null;
    private GameObject mouth = null;
    private bool isAtGoalZone;
    private ChuckSubInstance chuck;
    private bool singing = false;
    private float sing_start;
    private float sing_time = 1.0f;

    private void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
        spriteanimator = transform.Find("PlayerSprite").gameObject.GetComponent<Animator>();
        health = health_max;
        speed = 5f;
        isAtGoalZone = false;
        chuck = GetComponent<ChuckSubInstance>();
    }

    void Update()
    {
        if (singing && Time.time > sing_start + sing_time)
        {
            singing = false;
            Destroy(mouth);
            mouth = null;
            Debug.Log("done singing");
        }
        Vector2 translation;
        if (isAtGoalZone)
        {
            Debug.Log("At Goal Zone");
        }
        if (jumping)
        {
            translation = new Vector2(0f, 1f) * speed * 2f;
        }
        else if (falling)
        {
            translation = new Vector2(0f, -1f) * speed * 2f;
        }
        else
        {
            horizontal = Input.GetAxis("Horizontal");
            vertical = Input.GetAxis("Vertical");
            translation = new Vector2(horizontal, vertical) * speed;
            if (translation.magnitude > 0f)
            {
                spriteanimator.SetBool("Walking", true);
            }
            else
            {
                spriteanimator.SetBool("Walking", false);
            }
        }
        rb2d.MovePosition(rb2d.position + translation * Time.fixedDeltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "JumpZone")
        {
            if (vertical > 0f)
            {
                Debug.Log("Jumping!");
                jumping = true;
                falling = false;
            }
            else if (vertical < 0f)
            {
                Debug.Log("Falling!");
                jumping = false;
                falling = true;
            }
        }
        else if (collision.gameObject.tag == "GoalZone")
        {
            isAtGoalZone = true;
        }
        if (collision.gameObject.tag == "enemy_projectile")
        {
            Projectile that = collision.gameObject.GetComponent<Projectile>();
            ImHit(that);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "JumpZone")
        {
            Debug.Log("Back To Earth!");
            if (jumping || falling)
            {
                jumping = false;
                falling = false;
            }
        }
        else if (collision.gameObject.tag == "GoalZone")
        {
            isAtGoalZone = false;
        }
    }

    private void HealthChange(int diff)
    {
        int new_health = health + diff;
        if (new_health < 0) {
            new_health = 0;
        } else if (new_health > health_max) {
            new_health = health_max;
        }
        health = new_health;
        health_slider.value = health;
    }

    public void ImHit(Projectile that)
    {
        HealthChange(-that.damage);
        that.Explode();
    }

    public void Sing(string note_name, float note_freq)
    {
        if (!singing)
        {
            singing = true;
            sing_start = Time.time;
            Debug.Log(note_name + ", " + note_freq);
            List<Enemy> enemies = spawner.Resonate(note_freq);
            foreach(Enemy enemy in enemies)
            {
                ShootTowards(enemy.gameObject);
            }
            mouth = (GameObject)Instantiate(mouth_LA, transform);
            chuck.RunCode(ChuckSynths.Voice(note_freq, sing_time));
        }
        else
        {
            Debug.Log("already singing...");
        }
    }

    public void ShootTowards(GameObject target)
    {
        PlayerProjectile the_projectile = (PlayerProjectile)Instantiate(projectile_used, transform);
        the_projectile.Target(target);
        the_projectile.speed = projectile_speed;
    }
}
