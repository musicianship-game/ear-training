using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    public float death_time = 1.5f;
    public string instrument_synth_name;
    private float start_time;
    public bool dying = false;
    private bool choose_new_pitch;
    private Component[] particleSys;
    private Component[] childSprites;
    private List<SpriteRenderer> allSprites = new List<SpriteRenderer>();
    public float wait_in_sec = 5.0f;
    public float projectile_speed = 2.0f;
    public GameObject projectile_used = null;
    public PlayerController player;
    public ParticleSystem death_explosion;
    private FloatBehavior float_behavior;

    public int hit_points = 3;

    private int projectile_damage = 1;
    private float next_time = 0.0f; //also initial offset of firing cycle
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
        ChooseNewPitch();
        choose_new_pitch = false;
        particleSys = GetComponentsInChildren<ParticleSystem>();
        childSprites = GetComponentsInChildren<SpriteRenderer>();
        float_behavior = GetComponent<FloatBehavior>();
        allSprites.Add(GetComponent<SpriteRenderer>());
        foreach (SpriteRenderer sprite in childSprites)
        {
            allSprites.Add(sprite);
        }
        next_time = Random.Range(0.0f,wait_in_sec);
    }

    public string GetNoteName()
    {
        return note_name;
    }

    public float GetNoteFrequency()
    {
        return note_freq;
    }

    private void ChooseNewPitch()
    {
        // Debug.Log("Difficulty: " + Settings.MusicalDifficulty);
        // Debug.Log(Scale.GetDistributionAsString());
        Vector2Int degreeAlteration = Scale.SampleNoteFromDistribution();
        scale_degree = degreeAlteration.x;
        alteration = degreeAlteration.y;
        note_name = Scale.GetNoteName(scale_degree, alteration);
        note_freq = Scale.GetNoteFrequency(scale_degree, alteration);
        Debug.Log(scale_degree + " " + alteration + " " + note_name + " " + note_freq);
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time > next_time && !dying)
        {
            Vector2 target_position = player.transform.position;
            if (choose_new_pitch) ChooseNewPitch();
            choose_new_pitch = false;
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
        if (instrument_synth_name == "violin")
        {
            parent.RunChuckCode(ChuckSynths.Violin(note_freq));
        }
        else if (instrument_synth_name == "trumpet")
        {
            parent.RunChuckCode(ChuckSynths.Trumpet(note_freq));
        }
        else
        {
            Debug.Log("Instrument name not recognized. Default synth chosen.");
            parent.RunChuckCode(ChuckSynths.Violin(note_freq));
        }
    }

    public void GetHit()
    {
        if (hit_points > 1)
        {
            hit_points -= 1;
            choose_new_pitch = true;
            GetAngrier();
        }
        else
        {
            hit_points = 0;
            Die();
        }
    }

    private void GetAngrier()
    {
        // float_behavior.X_sin_freq *= 2.5f;
        float_behavior.Y_sin_freq += 6f;
    }

    private void Die()
    {
        dying = true;
        start_time = Time.time;
        float_behavior.X_sin_freq = 1f;
        float_behavior.Y_sin_freq = 1f;
        foreach (ParticleSystem part in particleSys)
        {
            part.Stop();
        }
        Instantiate(death_explosion, transform);
    }

}