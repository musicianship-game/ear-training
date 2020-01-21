using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchParentColor : MonoBehaviour {

    public Color glow_color;

    // Use this for initialization
    void Start () {
        glow_color = GetComponent<SpriteRenderer>().color;
    }

	// Update is called once per frame
	void Update () {
        GetComponent<SpriteRenderer>().color = glow_color;
	}
}
