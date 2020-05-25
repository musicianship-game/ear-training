using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MelodicSequences
{
    public class Note
    {
        public readonly int scaleDegree;
        public readonly int alteration;
        public readonly float frequency;
        public Note(int sd, int alt)
        {
            scaleDegree = sd;
            alteration = alt;
            frequency = Scale.GetNoteFrequency(sd, alt);
        }
    }

    public class MelodicSequence
    {
        public readonly List<Note> sequence;
        public MelodicSequence(Note n1, Note n2, Note n3, Note n4, Note n5, Note n6)
        {
            sequence = new List<Note>
            {
                n1, n2, n3, n4, n5, n6
            };
        }
    }

    public static List<MelodicSequence> melodicSequences;

    static MelodicSequences()
    {
        MelodicSequence seq1 = new MelodicSequence(
                new Note(5, 0),
                new Note(4, 0),
                new Note(2, 0),
                new Note(3, 0),
                new Note(1, 0),
                new Note(0, 0)
        );

        MelodicSequence seq2 = new MelodicSequence(
                new Note(4, 0),
                new Note(5, 0),
                new Note(3, 0),
                new Note(2, 0),
                new Note(1, 0),
                new Note(0, 0)
        );

        MelodicSequence seq3 = new MelodicSequence(
                new Note(5, 0),
                new Note(4, 0),
                new Note(1, 0),
                new Note(2, 0),
                new Note(1, 0),
                new Note(0, 0)
        );

        MelodicSequence seq4 = new MelodicSequence(
                new Note(1, 0),
                new Note(4, 0),
                new Note(3, 0),
                new Note(2, 0),
                new Note(1, 0),
                new Note(0, 0)
        );

        MelodicSequence seq5 = new MelodicSequence(
                new Note(0, 0),
                new Note(3, 0),
                new Note(1, 0),
                new Note(2, 0),
                new Note(1, 0),
                new Note(0, 0)
        );

        melodicSequences = new List<MelodicSequence>
        {
            seq1, seq2, seq3, seq4, seq5
        };
    }

    public static MelodicSequence GetRandomSequence()
    {
        int index = Random.Range(0, melodicSequences.Count);
        return melodicSequences[index];
    }
}
