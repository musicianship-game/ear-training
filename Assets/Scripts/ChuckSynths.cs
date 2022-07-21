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
        0.037 * Math.exp(0.0023 * " + (frequency * (182.647f + (10.3153f * Mathf.Log(frequency))) / 256.0f) + @") => brass.noteOn;
        " + 0.9f * duration + @"::second => now;
        0.1 => brass.noteOff;
        " + 0.1f * duration + @"::second => now;
        3::second => now;";
        return snippet;
    }

    public static string BG_Plucked_String(float frequency, float duration = 0.25f)
    {
        string snippet = @"
        StifKarp m => NRev r => dac;
        .0625 => r.gain;
        .02 => r.mix;
        0.1234 => m.pickupPosition;
        0.2 => m.sustain;
        0.0 => m.stretch;
        " + frequency + @" => float x;
        -0.0614092 + 0.99378*x + 0.000270136*x*x => m.freq;
        0.5 => m.pluck;
        " + 0.9f * duration + @"::second => now;
        0.0 => m.noteOff;
        " + 0.1f * duration + @"::second => now;
        3::second => now;";
        return snippet;
    }

    public static string BG_Bass(int power)
    {
        // power is assumed to be an int from 1 to 9, inclusive
        string snippet = @"
        Impulse i => LPF l => NRev r => ADSR e => dac;
        e.set(2::ms, 2::ms, 0.99, 200::ms);
        " + Mathf.Pow( 0.9f , power*1.0f ) + @" => float power;
        6.0 => r.gain;
        .4 => r.mix;
        500.0 => l.freq;
        1.0 - power => l.gain;
        e.keyOn();
        1.0 => i.next;
        10::ms => now;
        0.8 => i.next;
        e.keyOff();
        1000::ms => now;";
        return snippet;
    }

    public static string BG_Snare(int power)
    {
        // power is assumed to be an int from 1 to 9, inclusive
        string snippet = @"
        Noise n => ADSR e => LPF l => NRev r => ADSR E => dac;
        e.set(3::ms, 10::ms, 0.5, 100::ms);
        E.set(1::ms, 1::ms, 0.99, 200::ms);
        .25 => r.gain;
        .1 => r.mix;
        5000.0 => l.freq;
        " + 0.01f*power + @" => n.gain;
        E.keyOn();
        e.keyOn();
        100::ms => now;
        e.keyOff();
        100::ms => now;
        E.keyOff();
        1000::ms => now;";
        return snippet;
    }

    public static string BG_Hi_Hat(int power)
    {
        // power is assumed to be an int from 1 to 9, inclusive
        string snippet = @"
        Noise n => ADSR e => HPF h => NRev r => ADSR E => dac;
        e.set(2::ms, 10::ms, 0.5, 50::ms);
        E.set(1::ms, 1::ms, 0.99, 200::ms);
        .5 => r.gain;
        .05 => r.mix;
        " + 0.01f * power + @" => n.gain;
        10.0 => h.Q;
        4000.0 => h.freq;
        E.keyOn();
        e.keyOn();
        40::ms => now;
        e.keyOff();
        100::ms => now;
        E.keyOff();
        1000::ms => now;";
        return snippet;
    }

    public static string Bell(float frequency, float duration = 4.0f)
    {
        string snippet = 
        frequency + @" => float freq1;
        ModalBar mb0 => Gain g0 => dac;
        7 => mb0.preset;
        freq1 => mb0.freq;
        .5012 => mb0.strikePosition;
        0.5 => mb0.stickHardness;
        0.5 => g0.gain;
        0.99 => mb0.strike;
        ModalBar mb1 => Gain g1 => dac;
        1 => mb1.preset;
        freq1 => mb1.freq;
        .498 => mb1.strikePosition;
        0.8 => mb1.stickHardness;
        0.8 => g1.gain;
        0.99 => mb1.strike;
        ModalBar mb2 => Gain g2 => dac;
        4 => mb2.preset;
        freq1 => mb2.freq;
        .498 => mb2.strikePosition;
        0.5 => mb2.stickHardness;
        0.5 => g2.gain;
        0.99 => mb2.strike;
        " + duration + @"::second => now;
        ";
        return snippet;
    }

    public static string Flute(float frequency, float duration = 1.0f)
    {
        string snippet = @"
        Flute flute => JCRev r => dac;
        .75 => r.gain;
        .02 => r.mix;
        0.05 => flute.noiseGain;
        Math.random2f( 0, 9 ) => flute.vibratoFreq;
        Math.random2f( 0.01, 0.08 ) => flute.vibratoGain;
        0.7 => flute.pressure;
        0.9 => flute.gain;
        " + frequency + @" => flute.freq;
        0.2 => flute.noteOn;
        " + 0.9f * duration + @"::second => now;
        0.1 => flute.noteOff;
        " + 0.2f * duration + @"::second => now;
        3::second => now;";
        return snippet;
    }
}
