using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

[Serializable]
public class SaveData
{
    public long score { set; get; }
    public long totalScore { set; get; }
    public int characterCountTotal { set; get; }
    public int[] characterCount { set; get; }  //aiba,jun,nino,ohno,sho
    public int cheerCount { set; get; }
    public int[][] characterLevel { set; get; }

    public SaveData()
    {
        score = 0;
        totalScore = 0;
        characterCountTotal = 0;
        characterCount = new int[5] { 0, 0, 0, 0, 0 };
        cheerCount = 0;
        characterLevel = new int[Constants.CHARACTER_SERIES_COUNT][];
        for (int i = 0; i < Constants.CHARACTER_SERIES_COUNT; i++)
        {
            characterLevel[i] = new int[5] { 0, 0, 0, 0, 0 };
        }
    }
}
