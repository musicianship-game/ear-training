# -*- coding: utf-8 -*-
"""Generate all the difficulty profiles (key profiles) for each scale.

This code has been written to test the difficulty engine of the game.
The notations and names of the scales follow the same structure as the
generator of scales.
"""

import os


distributions = {
    "krumhansl_kessler": [
        0.15195022732711172, 0.0533620483369227, 0.08327351040918879,
        0.05575496530270399, 0.10480976310122037, 0.09787030390045463,
        0.06030150753768843, 0.1241923905240488, 0.05719071548217276,
        0.08758076094759511, 0.05479779851639147, 0.06891600861450106,

        0.14221523253201526, 0.06021118849696697, 0.07908335205571781,
        0.12087171422152324, 0.05841383958660975, 0.07930802066951245,
        0.05706582790384183, 0.1067175915524601, 0.08941810829027184,
        0.06043585711076162, 0.07503931700741405, 0.07121995057290496
    ],
    "sapp": [
        0.2222222222222222, 0.0, 0.1111111111111111, 0.0,
        0.1111111111111111, 0.1111111111111111, 0.0, 0.2222222222222222,
        0.0, 0.1111111111111111, 0.0, 0.1111111111111111,

        0.2222222222222222, 0.0, 0.1111111111111111, 0.1111111111111111,
        0.0, 0.1111111111111111, 0.0, 0.2222222222222222,
        0.1111111111111111, 0.0, 0.05555555555555555, 0.05555555555555555
    ],
    "harmonic_minor": [
        3, 1, 2, 1, 2, 2, 1, 3, 1, 2, 1, 2,

        3, 1, 2, 2, 1, 2, 1, 3, 2, 1, 1, 2,
    ],
    "aarden_essen": [
        0.17766092893562843, 0.001456239417504233, 0.1492649402940239,
        0.0016018593592562562, 0.19804892078043168, 0.11358695456521818,
        0.002912478835008466, 0.2206199117520353, 0.001456239417504233,
        0.08154936738025305, 0.002329979068008373, 0.049512180195127924,

        0.18264800547944018, 0.007376190221285707, 0.14049900421497014,
        0.16859900505797015, 0.0070249402107482066, 0.14436200433086013,
        0.0070249402107482066, 0.18616100558483017, 0.04566210136986304,
        0.019318600579558018, 0.07376190221285707, 0.017562300526869017
    ],
    "bellman_budge": [
        0.168, 0.0086, 0.1295, 0.0141, 0.1349, 0.1193,
        0.0125, 0.2028, 0.018000000000000002, 0.0804, 0.0062, 0.1057,

        0.1816, 0.0069, 0.12990000000000002,
        0.1334, 0.010700000000000001, 0.1115,
        0.0138, 0.2107, 0.07490000000000001,
        0.015300000000000001, 0.0092, 0.10210000000000001
    ],
    "temperley": [
        0.17616580310880825, 0.014130946773433817, 0.11493170042392838,
        0.019312293923692884, 0.15779557230334432, 0.10833725859632594,
        0.02260951483749411, 0.16839378238341965, 0.02449364107395195,
        0.08619877531794629, 0.013424399434762127, 0.09420631182289213,

        0.1702127659574468, 0.020081281377002155, 0.1133158020559407,
        0.14774085584508725, 0.011714080803251255, 0.10996892182644036,
        0.02510160172125269, 0.1785799665311977, 0.09658140090843893,
        0.016017212526894576, 0.03179536218025341, 0.07889074826679417
    ],
    'albrecht_shanahan1': [
        0.238, 0.006, 0.111, 0.006, 0.137, 0.094,
        0.016, 0.214, 0.009, 0.080, 0.008, 0.081,

         0.220, 0.006, 0.104, 0.123, 0.019, 0.103,
         0.012, 0.214, 0.062, 0.022, 0.061, 0.052
    ],
    'albrecht_shanahan2': [
        0.21169, 0.00892766, 0.120448, 0.0100265, 0.131444, 0.0911768, 0.0215947, 0.204703, 0.012894, 0.0900445, 0.012617, 0.0844338,

        0.201933, 0.009335, 0.107284, 0.124169, 0.0199224, 0.108324,
        0.014314, 0.202699, 0.0653907, 0.0252515, 0.071959, 0.049419
    ]
}


if __name__ == '__main__':
    major_scale_semitones_to_c = [0, 2, 4, 5, 7, 9, 11]
    minor_scale_semitones_to_c = [0, 2, 3, 5, 7, 8, 10]

    key_profile_weights_major = [
        # C, D, E, F, G, A, B
        1, 1, 1, 1, 1, 1, 1,
        # C#, D#, E#, F#, G#, A#, B#
        0.75, 0.25, 0, 0.75, 0.25, 0.25, 0,
        # Cx, Dx, Ex, Fx, Gx, Ax, Bx,
        0, 0, 0, 0, 0, 0, 0,
        # Cbb, Dbb, Fbb, Gbb, Abb, Bbb
        0, 0, 0, 0, 0, 0, 0,
        # Cb, Db, Eb, Fb, Gb, Ab, Bb,
        0, 0.25, 0.75, 0, 0.25, 0.75, 0.75,
    ]

    key_profile_weights_minor = [
        # C, D, Eb, F, G, Ab, Bb
        1,   1, 1,  1, 1, 1,  1,
        # C#, D#, E, F#,  G#, A, B
        0.25, 0,  1, 0.5, 0,  1, 1,
        # Cx, Dx, E#, Fx, Gx, Ax, B$,
        0,    0,  0,  0,  0,  0,  0,
        # Cbb, Dbb, Ebbb, Fbb, Gbb, Abbb, Bbbb
        0,     0,   0,    0,   0,   0,    0,
        # Cb, Db,   Ebb, Fb, Gb,  Abb, Bbb,
        0,    0.75, 0,   0,  0.5, 0,   0,
    ]

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
        base_distribution = 'temperley'
        print(notation)
        for mode_id, mode in enumerate(d['modes']):
            chromatic_increase = 0
            base_semitones_to_c = major_scale_semitones_to_c if mode_id == 0 else minor_scale_semitones_to_c
            scale_alterations = major_scale_alterations if mode_id == 0 else minor_scale_alterations
            key_profile = distributions[base_distribution]
            key_profile = key_profile[:12] if mode_id == 0 else key_profile[12:]
            key_profile_weights = key_profile_weights_major if mode_id == 0 else key_profile_weights_minor
            for scale_id, scale in enumerate(scales):
                note, alteration = scale
                scale_note = d['notes'][note]
                alteration_name = d['scale_alterations'][alteration]
                scale_name = '{}{} {}'.format(scale_note, alteration_name, mode)
                scale_semitones_to_c = [s + chromatic_increase for s in base_semitones_to_c]
                note_alterations = scale_alterations[scale_id]
                note_indexes = [n % 7 for n in range(note, note + 7)]
                scale_dir = os.path.join('Scales', notation, scale_name)
                if not os.path.exists(scale_dir):
                    os.makedirs(scale_dir)
                csv_filepath = os.path.join(scale_dir, 'distribution.csv')
                with open(csv_filepath, encoding='utf-8', mode='w') as csv:
                    print(scale_name)
                    for alt in [0, 1, 2, -2, -1]:
                        for scale_degree, note_letter_id in enumerate(note_indexes):
                            note_name = d['notes'][note_letter_id]
                            note_alteration = note_alterations[note_letter_id] + alt
                            note_alteration_name = d['note_alterations'][note_alteration]
                            note_name = '{}{}'.format(note_name, note_alteration_name)
                            print('{}, '.format(note_name), end='')
                            if scale_degree < len(note_indexes) - 1:
                                csv.write('{}, '.format(note_name))
                            else:
                                csv.write('{}\n'.format(note_name))
                        for scale_degree, note_idx in enumerate(note_indexes):
                            kp_weight = key_profile_weights[7*alt + scale_degree]
                            kp_index = (12 + base_semitones_to_c[scale_degree] + alt) % 12
                            probability = kp_weight * key_profile[kp_index]
                            print(probability)
                            if scale_degree < len(note_indexes) - 1:
                                csv.write('{}, '.format(probability))
                            else:
                                csv.write('{}\n'.format(probability))
                    chromatic_increase += 1
            print()