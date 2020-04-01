using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Instrument : MonoBehaviour {

    private EnemySpawnerController grand_parent;
    private Enemy parent;
    public string instrument_name;

    // Use this for initialization
    void Start () {
        parent = transform.parent.gameObject.GetComponent<Enemy>();
        grand_parent = parent.transform.parent.gameObject.GetComponent<EnemySpawnerController>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Play (float freq, float dur)
    {
        if (instrument_name == "trumpet")
        {
            grand_parent.RunChuckCode(ChuckSynths.Trumpet(freq, dur));
        }
        else if (instrument_name == "violin")
        {
            grand_parent.RunChuckCode(ChuckSynths.Violin(freq, dur));
        }
        else if (instrument_name == "bell")
        {
            grand_parent.RunChuckCode(ChuckSynths.Bell(freq, dur));
        }
    }
}
