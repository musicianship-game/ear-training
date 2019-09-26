using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserScaleDegreeInput : MonoBehaviour {
	private int alterationUp;
	private int alterationDown;
	private int alterationMultiplier;

	private int alteration;
	private int scaleDegree;

	private PlayerController playerController;

	private void Start()
	{
		alterationUp = 0;
		alterationDown = 0;
		alterationMultiplier = 1;
		playerController = GetComponent<PlayerController>();
	}

	private void Update()
    {
		// Get the inputs
        if (Input.GetButtonDown("AlterationUp")) alterationUp = 1;
		else if (Input.GetButtonUp("AlterationUp")) alterationUp = 0;

		if (Input.GetButtonDown("AlterationDown")) alterationDown = 1;
		else if (Input.GetButtonUp("AlterationDown")) alterationDown = 0;

		if (Input.GetButtonDown("AlterationMultiplier")) alterationMultiplier = 2;
		else if (Input.GetButtonUp("AlterationMultiplier")) alterationMultiplier = 1;

		// Update the current alteration value
		alteration = (alterationUp - alterationDown) * alterationMultiplier;

		// Handle any spelling of notes from the user
		if (Input.GetButtonDown("ScaleDegree1")) SpellScaleDegre(1);
		if (Input.GetButtonDown("ScaleDegree2")) SpellScaleDegre(2);
		if (Input.GetButtonDown("ScaleDegree3")) SpellScaleDegre(3);
		if (Input.GetButtonDown("ScaleDegree4")) SpellScaleDegre(4);
		if (Input.GetButtonDown("ScaleDegree5")) SpellScaleDegre(5);
		if (Input.GetButtonDown("ScaleDegree6")) SpellScaleDegre(6);
		if (Input.GetButtonDown("ScaleDegree7")) SpellScaleDegre(7);
		if (Input.GetButtonDown("ScaleDegree8")) SpellScaleDegre(8);
		if (Input.GetButtonDown("ScaleDegree9")) SpellScaleDegre(9);
		if (Input.GetButtonDown("ScaleDegree10")) SpellScaleDegre(10);
    }

	private void SpellScaleDegre(int n)
	{
		if (n >= 1 && n <= Scale.ScaleDegrees)
		{
			scaleDegree = n - 1;
			string noteName = Scale.GetNoteName(scaleDegree, alteration);
			float freq = Scale.GetNoteFrequency(scaleDegree, alteration);
			playerController.Sing(noteName, freq);
		}
	}
}
