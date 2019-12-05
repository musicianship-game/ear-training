using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class Scale {
    private const int DEFAULT_ALTERATIONS = 5;
    private const int DEFAULT_SCALEDEGREES = 7;

    public static List<string> NoteNames { get; set; }
    public static List<float> Frequencies { get; set; }
    public static List<float> Distribution {get; set; }
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
        Distribution = new List<float>()
        {
            // C, D, E, F, G, A, B
            16f, 8f, 8f, 8f, 16f, 8f, 8f,
            // C#, D#, E#, F#, G#, A#, B#
            4f, 1f, 0f, 4f, 1f, 1f, 0f,
            // Cx, Dx, Ex, Fx, Gx, Ax, Bx,
            0f, 0f, 0f, 0f, 0f, 0f, 0f,
            // Cbb, Dbb, Fbb, Gbb, Abb, Bbb
            0f, 0f, 0f, 0f, 0f, 0f, 0f,
            // Cb, Db, Eb, Fb, Gb, Ab, Bb,
            0f, 1f, 4f, 0f, 1f, 4f, 4f
        };
        NormalizeDistribution();
        ScaleDegrees = DEFAULT_SCALEDEGREES;
        Alterations = DEFAULT_ALTERATIONS;
    }

    public static void NormalizeDistribution()
    {
        float sum = Distribution.Sum();
        for (int i = 0; i < Distribution.Count ; i++)
        {
            float v = sum / Distribution[i];
            Distribution[i] = v;
        }
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

    public static float GetNoteProbability(int scaleDegree, int alteration = 0)
    {
        return Distribution[GetNoteIndex(scaleDegree, alteration)];
    }
}
