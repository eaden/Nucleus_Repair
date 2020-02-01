using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

public enum PixelColors
{
    pink,
    cyan,
    green,
    blue,
    red,
    gold
}


public static class GameManager
{
    public static StringBuilder sb = new StringBuilder();
    public static string delimiter = "|";
    public static string path = Application.dataPath + "\\saves\\";

    public static Dictionary<PixelColors, Color> Color_Dic = new Dictionary<PixelColors, Color>();

    public static void _Initiate()
    {
        path = path + "save " + DateTime.Now.ToString("yyyy-mm-dd HH-mm-ss") + ".csv";
        List<string> output = new List<string>();
        output.Add("Color$Anzahl$TTL$Zeit;");

        Debug.Log("initiated");
   
    }

    public static void WriteData(List<string> output, int length)
    {
        //Debug.Log(output.ToString());
        for (int index = 0; index < length; index++)
            GameManager.sb.AppendLine(output[index] + "|");
            //sb.AppendLine(output[index], delimiter));

        if (!File.Exists(GameManager.path))
            File.WriteAllText(GameManager.path, GameManager.sb.ToString());
        else
            File.AppendAllText(GameManager.path, GameManager.sb.ToString());
    }


    public static void CreateColorDic()
    {

    GameManager.Color_Dic.Add(PixelColors.blue, Color.blue);
    GameManager.Color_Dic.Add(PixelColors.cyan, Color.cyan);
    GameManager.Color_Dic.Add(PixelColors.green, Color.green);
    GameManager.Color_Dic.Add(PixelColors.pink, Color.magenta);
    GameManager.Color_Dic.Add(PixelColors.red, Color.red);
    GameManager.Color_Dic.Add(PixelColors.gold, new Color(0.95f, 0.75f, 0));
        Debug.Log("Color_DIC created");
    }

}
