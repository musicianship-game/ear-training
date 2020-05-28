using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchAlpha : MonoBehaviour {

    private SpriteRenderer parend;
    private ParticleSystem partsys;
    private Color orig_color;

	// Use this for initialization
	void Start () {
        parend = GetComponentInParent<SpriteRenderer>();
        partsys = GetComponent<ParticleSystem>();
        orig_color = partsys.startColor;
	}
	
	// Update is called once per frame
	void Update () {
        partsys.startColor = orig_color * new Color (1, 1, 1, parend.color.a);
	}
}