# -*- coding: utf-8 -*-
"""Generate all the scales in different notations.

This code has been written to test whether the user interface works.
The notations and names are approximated, as I am no expert of any other
way of spelling scales than spanish, and maybe english.

For the generation of these scales, an equal-tempered scale is assumed
and a reference frequency of A4 == 440.0Hz.
"""

import os

if __name__ == '__main__':
    a4 = 440.0
    major_scale_semitones_to_a4 = [-9, -7, -5, -4, -2, 0, 2]
    minor_scale_semitones_to_a4 = [-9, -7, -6, -4, -2, -1, 1]

    major_scale_alterations = [
        # C Major
        [0, 0, 0, 0, 0, 0, 0],
        # C# Major
        [1, 1, 1, 1, 1, 1, 1],
        # D Major
        [1, 0, 0, 1, 0, 0, 0],
        # Eb Major
        [0, 0, -1, 0, 0, -1, -1],
        # E Major
        [1, 1, 0, 1, 1, 0, 0],
        # F Major
        [0, 0, 0, 0, 0, 0, -1],
        # F# Major
        [1, 1, 1, 1, 1, 1, 0],
        # G Major
        [0, 0, 0, 1, 0, 0, 0],
        # Ab Major
        [0, -1, -1, 0, 0, -1, -1],
        # A Major
        [1, 0, 0, 1, 1, 0, 0],
        # Bb Major
        [0, 0, -1, 0, 0, 0, -1],
        # B Major
        [1, 1, 0, 1, 1, 1, 0],
    ]

    minor_scale_alterations = [
        # c minor
        [0, 0, -1, 0, 0, -1, -1],
        # c# minor
        [1, 1, 0, 1, 1, 0, 0],
        # d minor
        [0, 0, 0, 0, 0, 0, -1],
        # eb minor
        [-1, -1, -1, 0, -1, -1, -1],
        # e minor
        [0, 0, 0, 1, 0, 0, 0],
        # f minor
        [0, -1, -1, 0, 0, -1, -1],
        # f# minor
        [1, 0, 0, 1, 1, 0, 0],
        # g minor
        [0, 0, -1, 0, 0, 0, -1],
        # ab minor
        [-1, -1, -1, -1, -1, -1, -1],
        # a minor
        [0, 0, 0, 0, 0, 0, 0],
        # bb minor
        [0, -1, -1, 0, -1, -1, -1],
        # b minor
        [1, 0, 0, 1, 0, 0, 0],
    ]

    notations = {
        'North America': {
            'notes': ['C', 'D', 'E', 'F', 'G', 'A', 'B'],
            'modes': ['Major', 'Minor'],
            'scale_alterations': ['', '♯', '𝄪', '♯♯♯', '♭♭♭', '𝄫', '♭'],
            'note_alterations': ['', '♯', '𝄪', '♯♯♯', '♭♭♭', '𝄫', '♭'],
        },
        'German': {
            'notes': ['C', 'D', 'E', 'F', 'G', 'A', 'H'],
            'modes': ['Dur', 'Moll'],
            'scale_alterations': ['', 'es', 'is', 'eses', 'isis'],
            'note_alterations': ['', '♯', '𝄪', '♯♯♯', '♭♭♭', '𝄫', '♭'],
        },
        'Spanish': {
            'notes': ['Do', 'Re', 'Mi', 'Fa', 'Sol', 'La', 'Si'],
            'modes': ['Mayor', 'Menor'],
            'scale_alterations': ['', '♯', '𝄪', '♯♯♯', '♭♭♭', '𝄫', '♭'],
            'note_alterations': ['', '♯', '𝄪', '♯♯♯', '♭♭♭', '𝄫', '♭'],
        },
        'French': {
            'notes': ['Do', 'Re', 'Mi', 'Fa', 'Sol', 'La', 'Ti'],
            'modes': ['Majeur', 'Mineur'],
            'scale_alterations': ['', '♯', '𝄪', '♯♯♯', '♭♭♭', '𝄫', '♭'],
            'note_alterations': ['', '♯', '𝄪', '♯♯♯', '♭♭♭', '𝄫', '♭'],
        }
    }

    scales = [
        # (note, alteration) pairs
        (0, 0), (0, 1), (1, 0), (2, -1),
        (2, 0), (3, 0), (3, 1), (4, 0),
        (5, -1), (5, 0), (6, -1), (6, 0)
    ]

    for notation, d in notations.items():
        print(notation)
        for mode_id, mode in enumerate(d['modes']):
            chromatic_increase = 0
            base_semitones_to_a4 = major_scale_semitones_to_a4 if mode_id == 0 else minor_scale_semitones_to_a4
            scale_alterations = major_scale_alterations if mode_id == 0 else minor_scale_alterations
            for scale_id, scale in enumerate(scales):
                note, alteration = scale
                scale_note = d['notes'][note]
                alteration_name = d['scale_alterations'][alteration]
                scale_name = '{}{} {}'.format(scale_note, alteration_name, mode)
                scale_semitones_to_a4 = [s + chromatic_increase for s in base_semitones_to_a4]
                note_alterations = scale_alterations[scale_id]
                note_indexes = [n % 7 for n in range(note, note + 7)]
                scale_dir = os.path.join('Scales', notation, scale_name)
                if not os.path.exists(scale_dir):
                    os.makedirs(scale_dir)
                csv_filepath = os.path.join(scale_dir, 'fundamental_frequencies.csv')
                with open(csv_filepath, encoding='utf-8', mode='w') as csv:
                    print(scale_name)
                    for alt in [0, 1, 2, -2, -1]:
                        for idx, note_idx in enumerate(note_indexes):
                            note_name = d['notes'][note_idx]
                            note_alteration = note_alterations[note_idx] + alt
                            note_alteration_name = d['note_alterations'][note_alteration]
                            note_name = '{}{}'.format(note_name, note_alteration_name)
                            # print('{}, '.format(note_name), end='')
                            if idx < len(note_indexes) - 1:
                                csv.write('{}, '.format(note_name))
                            else:
                                csv.write('{}\n'.format(note_name))
                        for idx, note_idx in enumerate(note_indexes):
                            note_semitones_to_a4 = scale_semitones_to_a4[idx] + alt
                            freq = a4 * 2.0 ** (note_semitones_to_a4 / 12.0)
                            # print('{:.2f}Hz, '.format(freq), end='')
                            if idx < len(note_indexes) - 1:
                                csv.write('{:.2f}, '.format(freq))
                            else:
                                csv.write('{:.2f}\n'.format(freq))
                    chromatic_increase += 1
            print()