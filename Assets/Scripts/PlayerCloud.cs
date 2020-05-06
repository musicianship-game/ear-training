using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PlayerCloud {

    public static int life_max = 12;
    public static int life = life_max;
    public static int score = 0;
    public static int shots_fired = 0;
    public static int misspellings = 0;
    public static int enemies_defeated = 0;

    public static float get_accuracy()
    {
        return (float)misspellings / (float)shots_fired;
    }
}
