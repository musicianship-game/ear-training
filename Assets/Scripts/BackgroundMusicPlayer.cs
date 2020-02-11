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
			// TODO: Play the synths
			int degree1 = notes[0, 0] - 1;
			int alteration1 = notes[0, 1];
			if (degree1 >= 0)
			{
				float frequency1 = Scale.GetNoteFrequency(degree1, alteration1) / 4f;
				chuck.RunCode(ChuckSynths.BG_Plucked_String(frequency1));
			}
			// Debug.Log(notes);
			t = eventDelta;
		}
	}
}
