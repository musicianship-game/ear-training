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
    public static float menu_time = 0f;

    public static void Restart()
    {
        life_max = 12;
        life = life_max;
        score = 0;
        shots_fired = 0;
        misspellings = 0;
        enemies_defeated = 0;
        menu_time = 0f;
    }

    public static float GetAccuracy()
    {
        return 1f - ((float)misspellings / (float)shots_fired);
    }
}
