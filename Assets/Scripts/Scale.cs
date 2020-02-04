using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class Scale {
    private const int DEFAULT_ALTERATIONS = 5;
    private const int DEFAULT_SCALEDEGREES = 7;

    public static List<string> NoteNames { get; set; }
    public static List<float> Frequencies { get; set; }
    public static List<float> Distribution { get; set; }
    public static List<float> NormalizedDistribution { get; set; }
    public static List<float> ScaledDistribution { get; set; }
    public static int ScaleDegrees { get; set; }
    public static int Alterations { get; set; }

    static Scale()
    {
        NoteNames = new List<string>()
        {
            "C", "D", "E", "F", "G", "A", "B",
            "C♯", "D♯", "E♯", "F♯", "G♯", "A♯", "B♯",
            "C𝄪", "D𝄪", "E𝄪", "F𝄪", "G𝄪", "A𝄪", "B𝄪",
            "C𝄫", "D𝄫", "E𝄫", "F𝄫", "G𝄫", "A𝄫", "B𝄫",
            "C♭", "D♭", "E♭", "F♭", "G♭", "A♭", "B♭"
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
        UpdateDistribution(Distribution);
        ScaleDegrees = DEFAULT_SCALEDEGREES;
        Alterations = DEFAULT_ALTERATIONS;
    }

    public static void UpdateDistribution(List<float> newDistribution)
    {
        NormalizedDistribution = NormalizeDistribution(newDistribution);
        ScaledDistribution = ComputeScaledDistribution(NormalizedDistribution);
    }

    private static List<float> ComputeScaledDistribution(List<float> original)
    {
        List<float> distribution = new List<float>(original);
        float difficulty = Settings.MusicalDifficulty;
        float handicap = 2f;
        float clipping = 0.2f;
        distribution = ClipDistribution(distribution, difficulty, clipping);
        distribution = ScaleDistribution(distribution, handicap, difficulty);
        return distribution;
    }

    private static float ComputeThreshold(float distMax, float distMin, float difficulty, float cutoff)
    {
        float threshold = 0f;
        if (difficulty < cutoff)
        {
            threshold = distMax - difficulty * ((distMax - distMin) / cutoff);
        }
        return threshold;
    }

    private static List<float> ClipDistribution(List<float> originalDist, float difficulty, float cutoff)
    {
        List<float> clippedDistribution = new List<float>(originalDist);
        float threshold = ComputeThreshold(clippedDistribution.Max(), clippedDistribution.Min(), difficulty, cutoff);
        for (int i = 0; i < clippedDistribution.Count; i++)
        {
            clippedDistribution[i] = clippedDistribution[i] < threshold ? 0f : clippedDistribution[i];
        }
        clippedDistribution = NormalizeDistribution(clippedDistribution);
        return clippedDistribution;
    }

    private static List<float> ScaleDistribution(List<float> dist, float handicap, float difficulty)
    {
        List<float> scaledDistribution = new List<float>(dist);
        float scaling = handicap - (handicap * difficulty);
        for (int i = 0; i < scaledDistribution.Count; i++)
        {
            scaledDistribution[i] = Mathf.Pow(scaledDistribution[i], scaling);
        }
        scaledDistribution = NormalizeDistribution(scaledDistribution);
        return scaledDistribution;
    }

    public static List<float> NormalizeDistribution(List<float> dist)
    {
        List<float> normalizedDistribution = new List<float>(dist);
        float sum = normalizedDistribution.Sum();
        for (int i = 0; i < normalizedDistribution.Count ; i++)
        {
            float v = normalizedDistribution[i] / sum;
            normalizedDistribution[i] = v;
        }
        return normalizedDistribution;
    }

    public static Vector2Int SampleNoteFromDistribution() {
        Vector2Int degreeAlteration = new Vector2Int(0, 0);
        float rand = Random.value;
        // Debug.Log("Rand: " + rand);
        float currentProb = 0f;
        for (int i = 0 ; i < ScaledDistribution.Count ; i++)
        {
            int scaleDegree = i % ScaleDegrees;
            int alteration = i / ScaleDegrees;
            currentProb += ScaledDistribution[i];
            if (rand <= currentProb)
            {
                degreeAlteration.x = scaleDegree;
                degreeAlteration.y = alteration;
                break;
            }
        }
        return degreeAlteration;
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

    public static string GetDistributionAsString()
    {
        string output = ScaledDistribution[0] + " ";
        for (int i = 1; i < ScaledDistribution.Count; i++)
        {
            if (i % ScaleDegrees != 0)
            {
                output += ScaledDistribution[i] + " ";
            }
            else
            {
                output += ScaledDistribution[i] + "\n";
            }
        }
        return output;
    }
}
