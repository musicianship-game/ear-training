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
    public static List<string> Messages { get; set; }

    static PlayerCloud()
    {
        Messages = new List<string>()
        {
            "The spirits calm down after\nI sing 3 notes to them",
            "I wonder what is behind that gate",
            "I guess these spirits only\nwant someone to sing to them",
            "I hear some loud bells coming up from the basement",
            "The sound of the bells is pretty strong now"
        };
    }

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
