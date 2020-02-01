using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

public static class GameManager
{
    public static StringBuilder sb = new StringBuilder();
    public static string delimiter = "|";
    public static string path = Application.dataPath + "\\saves\\";

    public static Dictionary<PixelColors, Color> Color_Dic;

    public static void _Initiate()
    {
        path = path + "save " + DateTime.Now.ToString("yyyy-mm-dd HH-mm-ss") + ".csv";
    }

    public static void WriteData(List<string> output, int length)
    {
        //Debug.Log(output.ToString());
        for (int index = 0; index < length; index++)
            sb.AppendLine(output[index] + "|");
            //sb.AppendLine(output[index], delimiter));

        if (!File.Exists(path))
            File.WriteAllText(path, sb.ToString());
        else
            File.AppendAllText(path, sb.ToString());
    }

    public static void CreateColorDic()
    {
        Color_Dic = new Dictionary<PixelColors, Color>();
        Color_Dic.Add(PixelColors.blue, Color.blue);
        Color_Dic.Add(PixelColors.cyan, Color.cyan);
        Color_Dic.Add(PixelColors.green, Color.green);
        Color_Dic.Add(PixelColors.pink, Color.magenta);
        Color_Dic.Add(PixelColors.red, Color.red);
        Color_Dic.Add(PixelColors.gold, new Color(0.95f, 0.75f, 0));
    }

}
