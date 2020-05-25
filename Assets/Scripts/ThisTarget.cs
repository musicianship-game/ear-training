using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThisTarget : MonoBehaviour {

    public float centsTolerance = 20;
    public bool targetable = false;
    public float frequency = 0.0f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		// it's up to the enemy script or whatever else to update my variables
	}

    public bool Resonate(float player_note_freq) {
        float centsDifference = 1200 * Mathf.Log(frequency / player_note_freq, 2);
        bool success = (Mathf.Abs(centsDifference) < centsTolerance);
        //Bell bell = GetComponent<Bell>();
        //if (bell != null && targetable)
        //{
        //    bell.ResonatorUpdate(success);
        //}
        return (targetable && success);
    }
}
