"""Generate all the scales in different notations.

This code has been written to test whether the user interface works.
The notations and names are approximated, as I am no expert of any other
way of spelling scales than spanish, and maybe english.

For the generation of these scales, an equal-tempered scale is assumed
and a reference frequency of A4 == 440.0Hz.
"""

if __name__ == '__main__':
    a4 = 440.0
    major_scale_semitones_to_a4 = [-9, -7, -5, -4, -2, 0, 2]
    minor_scale_semitones_to_a4 = [-9, -7, -6, -4, -2, -1, 2]

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
        # d# minor
        [1, 1, 1, 1, 1, 1, 0],
        # e minor
        [0, 0, 0, 1, 0, 0, 0],
        # f minor
        [0, -1, -1, 0, 0, -1, -1],
        # f# minor
        [1, 0, 0, 1, 1, 0, 0],
        # g minor
        [0, 0, -1, 0, 0, 0, -1],
        # g# minor
        [1, 1, 0, 1, 1, 1, 0],
        # a minor
        [0, 0, 0, 0, 0, 0, 0],
        # a# minor
        [1, 1, 1, 1, 1, 1, 1],
        # b minor
        [1, 0, 0, 1, 0, 0, 0],
    ]

    notations = {
        'North America': {
            'notes': ['C', 'D', 'E', 'F', 'G', 'A', 'B'],
            'modes': ['Major', 'Minor'],
            'scale_alterations': ['', '♯', '𝄪', '𝄫', '♭'],
            'note_alterations': ['♮', '♯', '𝄪', '𝄫', '♭'],
        },
        'German': {
            'notes': ['C', 'D', 'E', 'F', 'G', 'A', 'H'],
            'modes': ['Dur', 'Moll'],
            'scale_alterations': ['', 'es', 'is', 'eses', 'isis'],
            'note_alterations': ['♮', '♯', '𝄪', '𝄫', '♭'],
        },
        'Spanish': {
            'notes': ['Do', 'Re', 'Mi', 'Fa', 'Sol', 'La', 'Si'],
            'modes': ['Mayor', 'Menor'],
            'scale_alterations': ['', '♯', '𝄪', '𝄫', '♭'],
            'note_alterations': ['♮', '♯', '𝄪', '𝄫', '♭'],
        },
        'French': {
            'notes': ['Do', 'Re', 'Mi', 'Fa', 'Sol', 'La', 'Ti'],
            'modes': ['Majeur', 'Mineur'],
            'scale_alterations': ['', '♯', '𝄪', '𝄫', '♭'],
            'note_alterations': ['♮', '♯', '𝄪', '𝄫', '♭'],
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
        for idx, mode in enumerate(d['modes']):
            chromatic_increase = 0
            if idx == 0 :
                scale_semitones_to_a4 = major_scale_semitones_to_a4
            else:
                scale_semitones_to_a4 = minor_scale_semitones_to_a4
            for note, alteration in scales:
                scale_root = d['notes'][note]
                alteration_name = d['scale_alterations'][alteration]
                scale_name = '{}{} {}'.format(scale_root, alteration_name, mode)
                scale_semitones_to_a4 = [s + chromatic_increase for s in scale_semitones_to_a4]
                print(scale_name)
                print('5:7, ', end='')
                for idx, note in enumerate(d['notes']):
                    note_semitones_to_a4 = scale_semitones_to_a4[idx]
                    freq = a4 * 2.0 ** (note_semitones_to_a4 / 12.0)
                    print('{} ({:.2f}Hz), '.format(note, freq), end='')
                print()
                chromatic_increase += 1



