using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


/// <summary>
/// constant configurations
/// </summary>
public class Constants
{
    private Constants() { }
    
    public static readonly string DB_CONNECTION_STRING = "URI=file:" + Application.dataPath + "/data.db";
    public const float ASPECT_RATIO = 10f / 16f;
    public const string SAVE_PATH = "savedata.sav";
    public const float AUTO_SAVE_PERIOD = 60;
}
