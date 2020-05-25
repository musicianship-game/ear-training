using UnityEngine;
using System;
using System.Collections;

public class TwistBehavior : MonoBehaviour
{
    public float freq = 1.0f;
    public float strength = 30.0f;
    private float offset;

    void Start()
    {
        offset = UnityEngine.Random.Range(0.0f, 6.28f);
    }

    void Update()
    {
        transform.eulerAngles = (Vector3.forward * (Mathf.Sin(Time.time * freq + offset) * strength));
    }
}