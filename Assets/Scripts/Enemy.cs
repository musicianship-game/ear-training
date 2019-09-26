using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    public bool dying = false;
    public float wait_in_sec = 5.0f;
    public float projectile_speed = 2.0f;
    public GameObject projectile_used = null;
    public PlayerController player;

    private int projectile_damage = 1;
    private float next_time = 0.0f;
    private ChuckSubInstance chuck;

    // Note stuff
    private int scale_degree;
    private int alteration;
    private string note_name;
    private float note_freq;

    // Use this for initialization
    void Start()
    {
        chuck = GetComponent<ChuckSubInstance>();
        scale_degree = GetScaleDegree();
        alteration = GetAlteration();
        note_name = Scale.GetNoteName(scale_degree, alteration);
        note_freq = Scale.GetNoteFrequency(scale_degree, alteration);
        Debug.Log(scale_degree + " " + alteration + " " + note_name + " " + note_freq);
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
        chuck.RunCode(ChuckSynths.Violin(note_freq));
    }

}