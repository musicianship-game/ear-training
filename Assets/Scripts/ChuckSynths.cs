using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ChuckSynths {

    public static string Voice(float frequency, float duration = 1.0f)
    {
        string snippet = @"
        VoicForm voc => ADSR adsr => JCRev r => dac;
        adsr.set( 100::ms, 10::ms, .5, 100::ms );
        1.0 => voc.pitchSweepRate;
        111.0 => voc.freq;
        " + frequency + @" => voc.freq;
        0.95 => voc.gain;
        .9 => r.gain;
        .01 => r.mix;
        0.3 => voc.loudness;
        0.05 => voc.vibratoGain;
        ""lll"" => voc.phoneme;
        0.7 => voc.speak;
        adsr.keyOn();
        " + 0.15f * duration + @"::second => now;
        ""aaa"" => voc.phoneme;
        " + 0.75f * duration + @"::second => now;
        adsr.keyOff();
        " + 0.1f * duration + @"::second => now;";
        return snippet;
    }

    public static string Violin(float frequency, float duration = 1.0f)
    {
        string snippet = @"
        Bowed bow => dac;
        Math.random2f(0.3, 0.4) => bow.bowPressure;
        Math.random2f(0.1, 0.3) => bow.bowPosition;
        Math.random2f(5, 6) => bow.vibratoFreq;
        Math.random2f(0.01, 0.03) => bow.vibratoGain;
        Math.random2f(0.7, 0.8) => bow.volume;
        " + frequency + @" => bow.freq;
        .8 => bow.noteOn;
        " + 0.9f * duration + @"::second => now;
        0.0 => bow.noteOff;
        " + 0.1f * duration + @"::second => now;";
        return snippet;
    }

    public static string Trumpet(float frequency, float duration = 1.0f)
    {
        string snippet = @"
        Brass brass => LPF lo => JCRev r => Gain g => dac;
        0.02 => r.mix;
        0.5 => lo.Q;
        4.0 => g.gain;
        " + frequency + @" * 6.0 => lo.freq;
        " + (frequency * (182.647f + (10.3153f * Mathf.Log(frequency))) / 256.0f) + @" => brass.freq;
        0.034 * Math.exp(0.0023 * " + (frequency * (182.647f + (10.3153f * Mathf.Log(frequency))) / 256.0f) + @") => brass.noteOn;
        " + 0.9f * duration + @"::second => now;
        0.1 => brass.noteOff;
        " + 0.1f * duration + @"::second => now;
        3::second => now;";
        return snippet;
    }
}
