using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gate : MonoBehaviour
{

    public float death_time = 2.0f;
    public float glow_pulse_time = 5.0f;
    private float start_time = 0.0f;
    private bool dying = false;
    private SpriteRenderer this_renderer;
    private Component[] particleSys;
    private Component[] childSprites;
    private List<SpriteRenderer> allSprites = new List<SpriteRenderer>();
    public EnemySpawnerController spawner;

    // Use this for initialization
    void Start()
    {
        particleSys = GetComponentsInChildren<ParticleSystem>();
        childSprites = GetComponentsInChildren<SpriteRenderer>();
        this_renderer = GetComponent<SpriteRenderer>();
        allSprites.Add(this_renderer);
        foreach (SpriteRenderer sprite in childSprites)
        {
            allSprites.Add(sprite);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!dying && spawner.all_enemies_dead)
        {
            Die();
        }
        if (dying)
        {
            float x = (death_time + start_time - Time.time) / death_time;
            if (x < 0.0f)
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
        float y = (Time.time % glow_pulse_time) / glow_pulse_time;
        y = Mathf.Abs(y - 0.5f)*2.0f;
        y = Mathf.SmoothStep(0.0f, 1.0f, y);
        Color glowColor = new Color(1, y, 1, this_renderer.color.a);
        this_renderer.color = glowColor;
        GetComponentInChildren<MatchParentColor>().front_renderer.color = glowColor;
    }

    private void Die()
    {
        dying = true;
        start_time = Time.time + 0.0f;
        foreach (ParticleSystem part in particleSys)
        {
            part.Stop();
        }
    }

}