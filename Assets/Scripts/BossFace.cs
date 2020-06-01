using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossFace : MonoBehaviour {

    private BossHand[] hands;
    private SpriteRenderer rend;
    private Color orig_color;
    private bool appearing = false;
    private float start_time;
    private float disco_dur = 3.0f;
    private float fade_dur = 1.0f;
    private int disco_lvl = 0;
    private float disco_frq = 4.11f;
    private BellBoss bell_boss;

	// Use this for initialization
	void Start () {
        hands = GetComponentsInChildren<BossHand>();
        rend = GetComponent<SpriteRenderer>();
        orig_color = rend.color * 1.0f;
        rend.color *= new Color(1, 1, 1, 0);
        bell_boss = GetComponentInParent<BellBoss>();
	}
	
	// Update is called once per frame
	void Update () {
		if (appearing)
        {
            Appear();
        }
    }

    public void Boo()
    {
        appearing = true;
        start_time = Time.time;
        disco_lvl += 1;
        disco_frq *= disco_lvl;
        foreach (BossHand hand in hands)
        {
            hand.HandWave();
        }
    }

    private void Appear() 
    {
        if (Time.time < start_time + fade_dur)
        {
            float j = (Time.time - start_time) / fade_dur;
            rend.color = new Color(orig_color[0], orig_color[1], orig_color[2], j);
        }
        else if (Time.time < start_time + fade_dur + disco_dur)
        {
            if (disco_lvl == 0)
            {
                rend.color = new Color(orig_color[0], orig_color[1], orig_color[2], 1);
            }
            else
            {
                float k = (Time.time - start_time - fade_dur) / disco_dur;
                if (k > 0.5f) { k = 1 - k; } // triangle wave peak
                k *= 2.0f; // scale triangle wave to go from 0 to 1 to 0
                float rk = orig_color[0];
                float gk = ((1 - k) * orig_color[1] + k * Mathf.Sin(Time.time * disco_frq) + 1.0f) / 2.0f;
                float bk = ((1 - k) * orig_color[2] + k * Mathf.Sin(Time.time * disco_frq) + 1.0f) / 2.0f;
                rend.color = new Color(rk, gk, bk, 1);
            }
        }
        else if (Time.time < start_time + (2 * fade_dur) + disco_dur)
        {
            float l = (Time.time - start_time - fade_dur - disco_dur) / fade_dur;
            l = 1 - l;
            rend.color = new Color(orig_color[0], orig_color[1], orig_color[2], l);
        }
        else {
            rend.color *= new Color(1, 1, 1, 0);
            appearing = false;
            if (bell_boss != null) { bell_boss.BooEndSignal(); }
        }
    }
}