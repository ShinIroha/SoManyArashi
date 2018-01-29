using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


/// <summary>
/// constant configurations
/// </summary>
public static class Constants
{    
    public static readonly string DB_CONNECTION_STRING = "URI=file:" + Application.dataPath + "/data.db";
    public const float ASPECT_RATIO = 10f / 16f;
    public const string SAVE_PATH = "savedata.sav";
    public const float AUTO_SAVE_PERIOD = 60;
    public static readonly string[] ALPHABET_NAMES = new string[5] { "Aiba", "Jun", "Nino", "Ohno", "Sho" };
    public static readonly string[] ALPHABET_NAMES_LOWERCASE = new string[5] { "aiba", "jun", "nino", "ohno", "sho" };
    public const int CHARACTER_SERIES_COUNT = 1;
}
