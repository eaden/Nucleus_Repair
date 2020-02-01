using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public struct Statistic
{
    public PixelColors EPixColor;
    public int iCount;
    public float fTTL;
    public float ftimeStamp;  
}


[CreateAssetMenu(fileName = "NucleusStats", menuName = "Create NucleusStats", order = 0)]
public class NucleusStats : ScriptableObject
{
    public List<PixelColors> Colors = new List<PixelColors>();
    public float fXtraSpeed;
    public List<Statistic> _stats = new List<Statistic>();

    int icGold;
    int icCyan;
    int icGreen;
    int icPink;
    int icBlue;
    int icRed;
    //ToDo: neue Eigenschaften für Nucleus hier rein
}


/*
    pink,
    cyan,
    green,
    blue,
    red,
     */


