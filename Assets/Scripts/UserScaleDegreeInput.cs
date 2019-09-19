using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserScaleDegreeInput : MonoBehaviour {
	private int alterationUp;
	private int alterationDown;
	private int alterationMultiplier;

	public int alteration;

	private void Start()
	{
		alterationUp = 0;
		alterationDown = 0;
		alterationMultiplier = 1;
	}

	private void Update()
    {
		// Get the inputs
        if (Input.GetButtonDown("AlterationUp"))
		{
			Debug.Log("Oh oh oh!");
			alterationUp = 1;
		}
		else if (Input.GetButtonUp("AlterationUp")) alterationUp = 0;

		if (Input.GetButtonDown("AlterationDown")) alterationDown = 1;
		else if (Input.GetButtonUp("AlterationDown")) alterationDown = 0;

		if (Input.GetButtonDown("AlterationMultiplier")) alterationMultiplier = 2;
		else if (Input.GetButtonUp("AlterationMultiplier")) alterationMultiplier = 1;

		// Update the current alteration value
		alteration = (alterationUp - alterationDown) * alterationMultiplier;
    }
}
