using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Scale {
	public static List<string> NoteNames { get; set; }
    public static List<float> Frequencies { get; set; }
    public static int ScaleDegrees { get; set; }
    public static int Alterations { get; set; }

    static Scale()
    {
        NoteNames = new List<string>()
        {
            "C", "D", "E", "F", "G", "A", "B"
        };
        Frequencies = new List<float>()
        {
            261.63f, 293.66f, 329.63f, 349.23f, 392.00f, 440.00f, 493.88f
        };
        ScaleDegrees = NoteNames.Count;
        Alterations = 1;
    }

    public static string GetNoteName(int scaleDegree, int alteration = 0)
    {
        int index = alteration * ScaleDegrees + scaleDegree;
        return NoteNames[index];
    }

    public static float GetNoteFrequency(int scaleDegree, int alteration = 0)
    {
        int index = alteration * ScaleDegrees + scaleDegree;
        return Frequencies[index];
    }
}
