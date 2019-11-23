using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gate : MonoBehaviour
{

    public float death_time = 2.0f;
    private float start_time;
    private bool dying = false;
    private Component[] particleSys;
    private Component[] childSprites;
    private List<SpriteRenderer> allSprites = new List<SpriteRenderer>();
    public EnemySpawnerController spawner;

    // Use this for initialization
    void Start()
    {
        particleSys = GetComponentsInChildren<ParticleSystem>();
        childSprites = GetComponentsInChildren<SpriteRenderer>();
        allSprites.Add(GetComponent<SpriteRenderer>());
        foreach (SpriteRenderer sprite in childSprites)
        {
            allSprites.Add(sprite);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (spawner.all_enemies_dead)
        {
            Die();
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