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
            "C", "D", "E", "F", "G", "A", "B",
            "C#", "D#", "E#", "F#", "G#", "A#", "B#",
            "Cx", "Dx", "Ex", "Fx", "Gx", "Ax", "Bx",
            "Cbb", "Dbb", "Ebb", "Fbb", "Gbb", "Abb", "Bbb",
            "Cb", "Db", "Eb", "Fb", "Gb", "Ab", "Bb"
        };
        Frequencies = new List<float>()
        {
            261.63f, 293.66f, 329.63f, 349.23f, 392.00f, 440.00f, 493.88f,
            277.18f, 311.13f, 349.23f, 369.99f, 415.30f, 466.16f, 523.25f,
            293.66f, 329.63f, 369.99f, 392.00f, 440.00f, 493.88f, 554.37f,
            233.08f, 261.63f, 293.66f, 311.13f, 349.23f, 392.00f, 440.00f,
            246.94f, 277.18f, 311.13f, 329.63f, 369.99f, 415.30f, 466.16f
        };
        ScaleDegrees = 7;
        Alterations = 5;
    }

    private static int GetNoteIndex(int scaleDegree, int alteration)
    {
        alteration = alteration >= 0 ? alteration : alteration + Alterations;
        return alteration * ScaleDegrees + scaleDegree;
    }

    public static string GetNoteName(int scaleDegree, int alteration = 0)
    {
        return NoteNames[GetNoteIndex(scaleDegree, alteration)];
    }

    public static float GetNoteFrequency(int scaleDegree, int alteration = 0)
    {
        return Frequencies[GetNoteIndex(scaleDegree, alteration)];
    }
}
