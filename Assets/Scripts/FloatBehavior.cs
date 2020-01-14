using UnityEngine;
using System;
using System.Collections;

public class FloatBehavior : MonoBehaviour
{
    float originalX;
    float originalY;

    private float X_sin_offset;
    public float X_sin_freq = 0;
    public float X_floatStrength = 1;
    private float Y_sin_offset;
    public float Y_sin_freq = 0;
    public float Y_floatStrength = 1;

    void Start()
    {
        X_sin_offset = UnityEngine.Random.Range(0.0f, 1.0f);
        Y_sin_offset = UnityEngine.Random.Range(0.0f, 1.0f);
        this.originalX = this.transform.position.x;
        this.originalY = this.transform.position.y;
    }

    void Update()
    {
        transform.position = new Vector3(
            originalX + ((float)Math.Sin(Time.time * X_sin_freq + X_sin_offset) * X_floatStrength),
            originalY + ((float)Math.Sin(Time.time * Y_sin_freq + Y_sin_offset) * Y_floatStrength),
            transform.position.z
        );
    }
}