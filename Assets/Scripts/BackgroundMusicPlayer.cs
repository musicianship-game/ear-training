using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMusicPlayer : MonoBehaviour {
	public int bpm;
	private float eventDelta;
	private float t;
	private ChuckSubInstance chuck;

	// Use this for initialization
	void Start ()
	{
		// If bpm not set, set it to 120
		chuck = GetComponent<ChuckSubInstance>();
		bpm = bpm == 0 ? 120 : bpm;
		SetBPM(bpm);
		t = eventDelta;
	}

	void SetBPM(int newBpm)
	{
		bpm = newBpm;
		int quartersPerSecond = bpm / 60;
		int sixteenthsPerSecond = quartersPerSecond * 4;
		eventDelta = 1f / sixteenthsPerSecond;
	}

	// Update is called once per frame
	void FixedUpdate ()
	{
		t -= Time.fixedDeltaTime;
		if (t <= 0f)
		{
			int [,] notes = BackgroundMusic.GetNextEvent();
			for (int inst = 0; inst < 4; inst++)
			{
				int degree = notes[inst, 0] - 1;
				int alteration = notes[inst, 1];
				float frequency = 0f;
				int power = 0;
				if (degree >= 0)
				{
					switch(inst)
					{
						case 0:
							frequency = Scale.GetNoteFrequency(degree, alteration) / 4f;
							chuck.RunCode(ChuckSynths.BG_Plucked_String(frequency));
							break;
						case 1:
							power = degree;
							chuck.RunCode(ChuckSynths.BG_Bass(power));
							break;
						case 2:
							power = degree;
							chuck.RunCode(ChuckSynths.BG_Snare(power));
							break;
						case 3:
							power = degree;
							chuck.RunCode(ChuckSynths.BG_Hi_Hat(power));
							break;
					}
				}
			}
			// Debug.Log(notes);
			t = eventDelta;
		}
	}
}
