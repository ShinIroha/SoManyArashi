using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public struct CharacterSeries
{
    public string name;
    public string name_lowercase;
    public int maxLevel;
    public int[] cost;
    public int[] generationScore;
    public int[] jumpScore;
    public float[] generationRate;
    public float[] jumpRate;

    public CharacterSeries(string name, int maxLevel, int[] cost, int[] generationScore, int[] jumpScore, float[] generationRate, float[] jumpRate)
    {
        this.name = name;
        this.name_lowercase = name.ToLower();
        this.maxLevel = maxLevel;
        this.cost = cost;
        this.generationScore = generationScore;
        this.jumpScore = jumpScore;
        this.generationRate = generationRate;
        this.jumpRate = jumpRate;
    }
}

public static class CharacterSeriesDatabase
{
    public static readonly List<CharacterSeries> data;

    static CharacterSeriesDatabase()
    {
        data = new List<CharacterSeries>();
        CharacterSeries temp;
        //Dot series
        temp = new CharacterSeries(
           "Dot",
           10,
           new int[11] { 100, 1000, 5000, 12000, 22000, 45000, 70000, 100000, 160000, 250000, 0 },
           new int[11] { 0, 100, 110, 120, 130, 140, 150, 160, 170, 180, 200 },
           new int[11] { 0, 50, 51, 52, 53, 54, 55, 56, 57, 58, 60 },
           new float[11] { 0, 0.01f, 0.0105f, 0.011f, 0.0115f, 0.012f, 0.0125f, 0.013f, 0.0135f, 0.014f, 0.015f },
           new float[11] { 0, 0.02f, 0.0205f, 0.021f, 0.0215f, 0.022f, 0.0225f, 0.023f, 0.0235f, 0.024f, 0.025f }
        );
        data.Add(temp);
    }
}
