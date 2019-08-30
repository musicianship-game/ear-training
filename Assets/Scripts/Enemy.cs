﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    public bool dying = false;
    public float wait_in_sec = 5.0f;
    public float projectile_speed = 2.0f;
    public GameObject projectile_used = null;
    public PlayerController player;


    private float next_time = 0.0f;

    // Use this for initialization
    void Start()
    {

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
        Quaternion rotation = Quaternion.Euler(0, 0, Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg);
        GameObject projectile = (GameObject)Instantiate(projectile_used, my_pos, rotation);
        projectile.GetComponent<Rigidbody2D>().velocity = direction * projectile_speed;
    }
}