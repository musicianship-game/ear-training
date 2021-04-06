using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Settings {
	public static float GameDifficulty { get; set; }
    public static float MusicalDifficulty { get; set; }

    static Settings() {
        GameDifficulty = 0.5f;
        MusicalDifficulty = 0.5f;
    }
}
