using UnityEngine;
using System;
using System.Collections;

public class TwistBehavior : MonoBehaviour
{
    public float freq = 0.5f;
    static float slow_freq = 0.5f;
    static float fast_freq = 1.1f;
    public bool fast = false;
    public float strength = 11.1f;
    private float phase;
    private float last_time;
    private float tau = 2.0f * Mathf.PI;

    void Start()
    {
        phase = UnityEngine.Random.Range(0.0f, tau);
        last_time = Time.time;
    }

    void Update()
    {
        if (fast) { freq = fast_freq; }
        else { freq = slow_freq; }
        phase = (phase + (Time.time - last_time) * freq * tau) % tau;
        transform.eulerAngles = (Vector3.forward * (Mathf.Sin(phase) * strength));
        last_time = Time.time;
    }
}