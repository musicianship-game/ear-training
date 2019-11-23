using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    public float death_time = 2.0f;
    private float start_time;
    public bool dying = false;
    private Component[] particleSys;
    private Component[] childSprites;
    private List<SpriteRenderer> allSprites = new List<SpriteRenderer>();
    public float wait_in_sec = 5.0f;
    public float projectile_speed = 2.0f;
    public GameObject projectile_used = null;
    public PlayerController player;

    public int hit_points = 3;

    private int projectile_damage = 1;
    private float next_time = 0.0f;
    private EnemySpawnerController parent;

    // Note stuff
    private int scale_degree;
    private int alteration;
    private string note_name;
    private float note_freq;

    // Use this for initialization
    void Start()
    {
        parent = transform.parent.gameObject.GetComponent<EnemySpawnerController>();
        scale_degree = GetScaleDegree();
        alteration = GetAlteration();
        note_name = Scale.GetNoteName(scale_degree, alteration);
        note_freq = Scale.GetNoteFrequency(scale_degree, alteration);
        Debug.Log(scale_degree + " " + alteration + " " + note_name + " " + note_freq);
        particleSys = GetComponentsInChildren<ParticleSystem>();
        childSprites = GetComponentsInChildren<SpriteRenderer>();
        allSprites.Add(GetComponent<SpriteRenderer>());
        foreach (SpriteRenderer sprite in childSprites)
        {
            allSprites.Add(sprite);
        }
    }

    public string GetNoteName()
    {
        return note_name;
    }

    public float GetNoteFrequency()
    {
        return note_freq;
    }

    int GetScaleDegree()
    {
        return Random.Range(0, Scale.ScaleDegrees - 1);
    }

    int GetAlteration()
    {
        return Random.Range(0, Scale.Alterations - 1);
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time > next_time && !dying)
        {
            Vector2 target_position = player.transform.position;
            fire(target_position);
            next_time = Time.time + wait_in_sec;
        }
        if (dying)
        {
            float x = (death_time - (Time.time - start_time)) / death_time;
            if (x < 0)
            {
                Destroy(gameObject);
            }
            else
            {
                Color newColor = new Color(1, 1, 1, Mathf.SmoothStep(0.0f, 1.0f, x));
                foreach (SpriteRenderer sprite in allSprites)
                {
                    sprite.color = newColor;
                }
            }
        }
    }

    void fire(Vector2 target_pos)
    {
        Vector2 my_pos = new Vector2(transform.position.x, transform.position.y);
        Vector2 direction = target_pos - my_pos;
        direction.Normalize();
        Quaternion rotation = Quaternion.Euler(0, 0, Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg + 90.0f);
        GameObject the_projectile = (GameObject)Instantiate(projectile_used, my_pos, rotation);
        the_projectile.GetComponent<Rigidbody2D>().velocity = direction * projectile_speed;
        the_projectile.GetComponent<Projectile>().damage = projectile_damage;
        parent.RunChuckCode(ChuckSynths.Violin(note_freq));
    }

    public void GetHit()
    {
        if (hit_points > 1)
        {
            hit_points -= 1;
        }
        else
        {
            hit_points = 0;
            Die();
        }
    }

    private void Die()
    {
        dying = true;
        start_time = Time.time;
        foreach (ParticleSystem part in particleSys)
        {
            part.Stop();
        }
    }

}