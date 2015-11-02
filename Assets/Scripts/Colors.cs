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

    // SETUP VARIABLES

    // Color objects
    private static readonly Color None = new Color(0, 0, 0, 0);
    private static readonly Color White = new Color(1, 1, 1);
    private static readonly Color Blue = new Color(0, .4f, 1);
    private static readonly Color Yellow = new Color(1, 1, 0);
    private static readonly Color Red = new Color(1, 0, 0);

    // Materials
    private static Material mat = Resources.Load("LightMaterialGradient") as Material;
    private static readonly Material NoneMaterial = MakeMaterial(mat, None);
    private static readonly Material WhiteMaterial = MakeMaterial(mat, White);
    private static readonly Material BlueMaterial = MakeMaterial(mat, Blue);
    private static readonly Material YellowMaterial = MakeMaterial(mat, Yellow);
    private static readonly Material RedMaterial = MakeMaterial(mat, Red);

    // Dictionary for mapping LightColor to relevant information (ColorInfo)
    private static readonly Dictionary<LightColor, ColorInfo> colorMap = new Dictionary<LightColor, ColorInfo>()
    {
        {LightColor.None, new ColorInfo {color=None, colorString="None", colorMaterial=NoneMaterial} },
        {LightColor.White, new ColorInfo {color=White, colorString="White", colorMaterial=WhiteMaterial} },
        {LightColor.Blue, new ColorInfo {color=Blue, colorString="Blue", colorMaterial=BlueMaterial} },
        {LightColor.Yellow, new ColorInfo {color=Yellow, colorString="Yellow", colorMaterial=YellowMaterial} },
        {LightColor.Red, new ColorInfo {color=Red, colorString="Red", colorMaterial=RedMaterial} }
    };


    // PUBLIC FUNCTIONS

    public static Color GetColor(LightColor color)
    {
        return colorMap[color].color;
    }

    public static string GetColorString(LightColor color)
    {
        return colorMap[color].colorString;
    }

    public static Material GetColorMaterial(LightColor color)
    {
        return colorMap[color].colorMaterial;
    }


    // PRIVATE HELPER FUNCTIONS / STRUCTURES

    private struct ColorInfo
    {
        public Color color;
        public string colorString;
        public Material colorMaterial;
    }

    private static Material MakeMaterial(Material mat, Color color)
    {
        Material copy = Material.Instantiate(mat);
        copy.color = color;
        return copy;
    }
}
