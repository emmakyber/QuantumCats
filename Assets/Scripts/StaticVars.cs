using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class StaticVars
{
    public static bool direct { get; set; } = false; // Default value is set to false
    public static bool restarted = false;
    public static int heartNums = 4;
    public static int level = 0;
    public static float superPositionTimer = 24f;
    public static bool superPositionActive = false;
    public static bool superPositionEmpty = false;

    public static void Reset()
    {
        direct = false;
        heartNums = 4;
        restarted = false;
        level = 0;
    }
}
