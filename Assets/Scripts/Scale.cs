using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Scale {
	public static List<string> NoteNames { get; set; }
    public static List<float> Frequencies { get; set; }
    public static int ScaleDegrees { get; set; }
    public static int Alterations { get; set; }

    public static string GetNoteName(int scaleDegree, int alteration = 0)
    {
        int index = alteration * ScaleDegrees + scaleDegree - 1;
        return NoteNames[index];
    }

    public static float GetNoteFrequency(int scaleDegree, int alteration = 0)
    {
        int index = alteration * ScaleDegrees + scaleDegree - 1;
        return Frequencies[index];
    }
}
