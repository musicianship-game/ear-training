using UnityEngine;
using System;
using System.Collections;

public class FloatBehavior : MonoBehaviour
{
    public float originalX;
    public float originalY;

    private float X_sin_offset;
    public float X_sin_freq = 0;
    public float X_floatStrength = 1;
    private float Y_sin_offset;
    public float Y_sin_freq = 0;
    public float Y_floatStrength = 1;
    private float ramp_up;
    public float ramp_up_time = 10.0f;
    private float ramp_start;

    void Start()
    {
        X_sin_offset = UnityEngine.Random.Range(0.0f, 6.28f);
        Y_sin_offset = UnityEngine.Random.Range(0.0f, 6.28f);
        RampReset();
    }

    private void OnEnable()
    {
        originalX = this.transform.position.x;
        originalY = this.transform.position.y;
        RampReset();
    }

    private void RampReset()
    {
        ramp_up = 0.0f;
        ramp_start = Time.time;
    }

    void Update()
    {
        Vector3 old = transform.position;
        Vector3 target = new Vector3(
            originalX + ((float)Math.Sin(Time.time * X_sin_freq + X_sin_offset) * X_floatStrength),
            originalY + ((float)Math.Sin(Time.time * Y_sin_freq + Y_sin_offset) * Y_floatStrength),
            transform.position.z
        );
        if (ramp_up < 1.0f)
        {
            ramp_up = (Time.time - ramp_start) / ramp_up_time;
        }
        if (ramp_up > 1.0f)
        {
            ramp_up = 1.0f;
        }
        transform.position += (target - old) * ramp_up;
    }
}