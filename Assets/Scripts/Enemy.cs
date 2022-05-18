﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    //public string instrument_synth_name;
    public bool dying = false;
    public bool targetable = true;
    private bool choose_new_pitch;
    private bool shielded = false;
    private Component[] particleSys;
    private Component[] childSprites;
    private List<SpriteRenderer> allSprites = new List<SpriteRenderer>();
    private float reload_in_sec = 5.0f;
    private float projectile_speed = 2.0f;
    public float reload_hard = 4.0f;
    public float reload_easy = 6.0f;
    public float proj_speed_hard = 5.0f;
    public float proj_speed_easy = 1.5f;
    public float shield_in_sec = 2.0f;
    public float dying_in_sec = 1.5f;
    public GameObject projectile_used = null;
    public int projectile_damage = 1;
    public PlayerController player;
    public ParticleSystem death_explosion;
    private FloatBehavior float_behavior;
    private ThisTarget this_target;
    public GameObject note_spray;

    public int max_hp = 3;
    public int hit_points = 3;
    public Slider hp_slider;

    private float reloading_time = 0.0f; //also initial offset of firing cycle
    private float shielded_time = 0.0f;
    private float dying_time = 0.0f;
    private EnemySpawnerController parent;

    // Note and instrument stuff
    private int scale_degree;
    private int alteration;
    private string note_name;
    private float note_freq;
    public float note_dur = 1.0f;
    public GameObject[] instrument_choices = new GameObject[2];
    private GameObject instrument_used;

    // Use this for initialization
    void Start()
    {
        parent = transform.parent.gameObject.GetComponent<EnemySpawnerController>();
        hp_slider.maxValue = max_hp;
        hp_slider.value = hit_points;
        ChooseNewPitch();
        choose_new_pitch = false;
        ApplyDifficulty();
        instrument_used = instrument_choices[Random.Range(0,instrument_choices.Length)];
        Instantiate(instrument_used, transform);
        particleSys = GetComponentsInChildren<ParticleSystem>();
        childSprites = GetComponentsInChildren<SpriteRenderer>();
        float_behavior = GetComponent<FloatBehavior>();
        allSprites.Add(GetComponent<SpriteRenderer>());
        foreach (SpriteRenderer sprite in childSprites)
        {
            allSprites.Add(sprite);
        }
        reloading_time = Random.Range(0.0f, reload_in_sec);
        this_target = GetComponent<ThisTarget>();
        this_target.frequency = note_freq;
        this_target.targetable = targetable;
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

    private void DropCurrentPitch()
    {
        note_name = "";
        note_freq = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (dying)
        {
            targetable = false;
            dying_time += Time.deltaTime;
            if (dying_time >= dying_in_sec)
            {
                Destroy(gameObject);
            }
            else
            {
                Color newColor = new Color(1, 1, 1, Mathf.SmoothStep(0.0f, 1.0f, dying_in_sec - dying_time));
                foreach (SpriteRenderer sprite in allSprites)
                {
                    sprite.color = newColor;
                }
            }
        }
        else if (shielded)
        {
            shielded_time += Time.deltaTime;
            if (shielded_time >= shield_in_sec)
            {
                shielded = false;
                UnshieldAnimation();
            }
        }
        else
        {
            reloading_time += Time.deltaTime;
            if (reloading_time >= reload_in_sec) {
                Vector2 target_position = player.transform.position;
                if (choose_new_pitch) ChooseNewPitch();
                choose_new_pitch = false;
                fire(target_position);
                reloading_time = 0.0f;
            }
        }
        this_target.frequency = note_freq;
        this_target.targetable = targetable;
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
        Play(note_dur);
    }

    public void GetHit()
    {
        if (shielded) return;
        Play(note_dur / 3.0f, true);
        if (hit_points > 1)
        {
            hit_points -= 1;
            hp_slider.value = hit_points;
            DropCurrentPitch();
            choose_new_pitch = true;
            shielded = true;
            shielded_time = 0.0f;
            ShieldAnimation();
        }
        else
        {
            hit_points = 0;
            hp_slider.value = hit_points;
            foreach (Image img in hp_slider.GetComponentsInChildren<Image>())
            {
                if (img.name == "Fill")
                {
                    Destroy(img); 
                }
            }
            Die();
        }
    }

    private void ShieldAnimation()
    {
        float_behavior.Y_sin_freq += 12f;
        foreach (SpriteRenderer sprite in allSprites)
        {
            if (sprite.gameObject.name == "bubble")
            {
                sprite.enabled = true;
            }
        }
    }

    private void UnshieldAnimation()
    {
        float_behavior.Y_sin_freq -= 12f;
        foreach (SpriteRenderer sprite in allSprites)
        {
            if (sprite.gameObject.name == "bubble")
            {
                sprite.enabled = false;
            }
        }
    }

    private void Die()
    {
        dying = true;
        dying_time = 0f;
        PlayerCloud.enemies_defeated++;
        float_behavior.X_sin_freq = 1f;
        float_behavior.Y_sin_freq = 1f;
        foreach (ParticleSystem part in particleSys)
        {
            part.Stop();
        }
        Instantiate(death_explosion, transform);
    }

    private void Play(float dur, bool hit = false)
    {
        if (!hit)
        {
            GetComponentInChildren<Instrument>().Play(note_freq, dur);
            Instantiate(note_spray, transform);
        }
        else if (hit)
        {
            parent.RunChuckCode(ChuckSynths.BG_Plucked_String(note_freq, dur));
        }
    }

    public void ApplyDifficulty()
    {
        float max_diff_reload = reload_easy - reload_hard;
        float diff_reload = max_diff_reload * Settings.GameDifficulty;
        reload_in_sec = reload_easy - diff_reload;
        float max_diff_speed = proj_speed_easy - proj_speed_hard;
        float diff_speed = max_diff_speed * Settings.GameDifficulty;
        projectile_speed = proj_speed_easy - diff_speed;
    }
}