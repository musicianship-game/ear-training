using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHand : MonoBehaviour {

    public bool move_right;
    private float fade_dur = 1.0f;
    private float swoosh_dur = 2.0f;
    private float swoosh_size;
    private Vector3 start_position;
    private float start_time;
    private SpriteRenderer sprend;
    private ParticleSystem partsys;
    private bool bells_started = false;

	// Use this for initialization
	void Awake () {
        start_position = transform.position;
        start_time = -1.0f - (2.0f * fade_dur) - swoosh_dur; // to make it start 'dead'
        sprend = GetComponent<SpriteRenderer>();
        sprend.color *= new Color(1, 1, 1, 0);
    }

    private void Start()
    {
        partsys = GetComponentInChildren<ParticleSystem>();
        Vector2 screenBounds = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));
        swoosh_size = screenBounds[0] * 0.9f;
    }

    // Update is called once per frame
    void Update () {
		if (Time.time < start_time + fade_dur)
        {
            float j = (Time.time - start_time) / fade_dur;
            sprend.color = new Color (sprend.color.r, sprend.color.g, sprend.color.b, j);
        }
        else if (Time.time < start_time + fade_dur + swoosh_dur)
        {
            if (partsys != null && !partsys.isPlaying) { partsys.Play(); }
            if (bells_started == false)
            {
                GetComponentInParent<BossFace>().StartTheBells();
                bells_started = true;
            }
            sprend.color = new Color(sprend.color.r, sprend.color.g, sprend.color.b, 1);
            float k = (Time.time - start_time - fade_dur) / swoosh_dur;
            if (move_right)
            {
                transform.position = start_position + new Vector3(k * swoosh_size, 0, 0);
            }
            else
            {
                transform.position = start_position + new Vector3(-k * swoosh_size, 0, 0);
            }
        }
        else if (Time.time < start_time + (2 * fade_dur) + swoosh_dur)
        {
            if (partsys != null && partsys.isPlaying) { partsys.Stop(); }
            float l = (Time.time - start_time - fade_dur - swoosh_dur) / fade_dur;
            l = 1 - l;
            sprend.color = new Color(sprend.color.r, sprend.color.g, sprend.color.b, l);
        }
        else
        {
            sprend.color *= new Color(1, 1, 1, 0);
            bells_started = false;
        }
    }

    public void HandWave()
    {
        GetComponent<SpriteRenderer>().color *= new Color(1, 1, 1, 0);
        transform.position = start_position;
        start_time = Time.time;
    }
}