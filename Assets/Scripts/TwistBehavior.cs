using UnityEngine;
using System;
using System.Collections;

public class TwistBehavior : MonoBehaviour
{
    public float freq = 1.0f;
    public float strength = 30.0f;

    void Start()
    {

    }

    void Update()
    {
        transform.eulerAngles = (Vector3.forward * (Mathf.Sin(Time.time * freq) * strength));
    }
}