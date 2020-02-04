using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class PlayerController : MonoBehaviour {
    private Rigidbody2D rb2d;
    private Animator spriteanimator;
    private GameObject PlayerSpriteHolder;
    private float speed;
    private float horizontal;
    private float vertical;
    Vector2 translation;
    private bool jumping;
    private bool falling;
    private Vector2 sung_pos;
    private Vector2 sung_pos_orig;
    public float sung_name_dist = 2.0f;
    public TextMeshProUGUI sung_note_name;
    public Slider health_slider;
    public TextMeshProUGUI attack_symbol;
    public TextMeshProUGUI score_counter;
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
        PlayerSpriteHolder = transform.Find("PlayerSpriteHolder").gameObject;
        spriteanimator = PlayerSpriteHolder.transform.Find("PlayerSprite").gameObject.GetComponent<Animator>();
        health_slider.maxValue = PlayerCloud.life_max;
        health_slider.value = PlayerCloud.life;
        speed = 3.5f;
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
            sung_note_name.transform.localPosition = sung_pos_orig;
        }
        else if (singing)
        {
            float k = ((Time.time - sing_start) / sing_time);
            Vector2 displace = new Vector2(0, k * sung_name_dist);
            sung_note_name.transform.position = sung_pos + displace;
            sung_note_name.alpha = Mathf.Sin(k*Mathf.PI);
        }

        if (isAtGoalZone)
        {
            Debug.Log("At Goal Zone");
            int currentScene = SceneManager.GetActiveScene().buildIndex;
            SceneManager.LoadScene(currentScene + 1);
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
            horizontal = Input.GetAxisRaw("Horizontal");
            vertical = Input.GetAxisRaw("Vertical");
            translation = new Vector2(horizontal, vertical) * speed;
            if (translation.magnitude > 0f && !singing)
            {
                spriteanimator.SetBool("Walking", true);
                PlayerSpriteHolder.transform.localScale = new Vector3(Mathf.Sign(horizontal), 1, 1);
            }
            else
            {
                spriteanimator.SetBool("Walking", false);
            }
        }
        score_counter.SetText(PlayerCloud.score.ToString());
    }

    void FixedUpdate()
    {
        if (!singing)
        {
            rb2d.MovePosition(rb2d.position + translation * Time.fixedDeltaTime);
        }
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
        int new_health = PlayerCloud.life + diff;
        if (new_health < 0) {
            new_health = 0;
        } else if (new_health > PlayerCloud.life_max) {
            new_health = PlayerCloud.life_max;
        }
        PlayerCloud.life = new_health;
        health_slider.value = PlayerCloud.life;
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
            sung_pos_orig = sung_note_name.transform.localPosition;
            sung_pos = sung_note_name.transform.position;
            Debug.Log(note_name + ", " + note_freq);
            attack_symbol.SetText(note_name);
            sung_note_name.SetText(note_name);
            List<Enemy> enemies = spawner.Resonate(note_freq);
            foreach(Enemy enemy in enemies)
            {
                if (!enemy.dying)
                {
                    ShootTowards(enemy.gameObject);
                }
            }
            mouth = (GameObject)Instantiate(mouth_LA, PlayerSpriteHolder.transform);
            chuck.RunCode(ChuckSynths.Voice(note_freq, sing_time));
        }
        else
        {
            Debug.Log("already singing...");
        }
    }

    public void ShootTowards(GameObject target)
    {
        PlayerProjectile the_projectile = Instantiate(projectile_used, transform.position, Quaternion.identity);
        Vector2 direction = new Vector2(0.0f,1.0f);
        direction.Normalize();
        the_projectile.transform.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg + 90.0f);
        the_projectile.GetComponent<Rigidbody2D>().velocity = direction * projectile_speed;
        the_projectile.Target(target);
        the_projectile.speed = projectile_speed;
    }
}
