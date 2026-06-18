using UnityEngine;

public class SenstivityManager
{
    public static float mouseSensitivity {set; get; } = 1f;
    public static void SetMouseSenstivity(float mouseSenstivity)
    {
        mouseSensitivity = mouseSenstivity;
    }
}
