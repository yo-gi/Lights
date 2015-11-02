using UnityEngine;
using System.Collections.Generic;

public enum LightColor
{
    None,
    White,
    Blue,
    Yellow,
    Red
}

public class Colors {

    private static readonly Color None = new Color(0, 0, 0, 0);
    private static readonly Color White = new Color(1, 1, 1);
    private static readonly Color Blue = new Color(0, 0, 1);
    private static readonly Color Yellow = new Color(1, 1, 0);
    private static readonly Color Red = new Color(1, 0, 0);

    public static Color GetColor(LightColor color)
    {
        switch (color)
        {
            case LightColor.None:
                return None;
            case LightColor.White:
                return White;
            case LightColor.Blue:
                return Blue;
            case LightColor.Yellow:
                return Yellow;
            case LightColor.Red:
                return Red;
            default:
                return None;
        }
    }
}
