using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;

class SaveData
{
    public long score { set; get; }
    public int characterCountTotal { set; get; }
    public int[] characterCount { set; get; }  //aiba,jun,nino,ohno,sho
    public int cheerCount { set; get; }

    XmlDocument sav = new XmlDocument();
    XmlNode xScore;
    XmlNode xCharacterCountTotal;
    XmlNodeList xCharacterCount;
    XmlNode xCheerCount;

    public SaveData()
    {
        try
        {
            sav.Load(Constants.SAVE_PATH);
            xScore= sav.GetElementsByTagName("score")[0];
            xCharacterCountTotal = sav.GetElementsByTagName("characterCountTotal")[0];
            xCharacterCount = sav.GetElementsByTagName("characterCount");
            xCheerCount = sav.GetElementsByTagName("cheerCount")[0];

            score = long.Parse(xScore.InnerText);
            characterCountTotal = int.Parse(xCharacterCountTotal.InnerText);
            characterCount = new int[5];
            for (int i = 0; i < 5; i++)
            {
                characterCount[i] = int.Parse(xCharacterCount[i].InnerText);
            }
            cheerCount = int.Parse(xCheerCount.InnerText);
        }
        catch (FileNotFoundException)
        {
            score = 0;
            characterCountTotal = 0;
            characterCount = new int[5] { 0, 0, 0, 0, 0 };
            cheerCount = 0;
            XmlDeclaration dec = sav.CreateXmlDeclaration("1.0", "UTF-8", null);
            sav.AppendChild(dec);
            XmlElement root = sav.CreateElement("savedata");
            sav.AppendChild(root);
            xScore = sav.CreateElement("score");
            root.AppendChild(xScore);
            xCharacterCountTotal = sav.CreateElement("characterCountTotal");
            root.AppendChild(xCharacterCountTotal);
            for(int i = 0; i < 5; i++)
            {
                root.AppendChild(sav.CreateElement("characterCount"));
            }
            xCharacterCount=sav.GetElementsByTagName("characterCount");
            xCheerCount = sav.CreateElement("cheerCount");
            root.AppendChild(xCheerCount);

            Save();
        }
    }

    public void Save()
    {
        xScore.InnerText = score.ToString();
        xCharacterCountTotal.InnerText = characterCountTotal.ToString();
        for(int i = 0; i < 5; i++)
        {
            xCharacterCount[i].InnerText = characterCount[i].ToString();
        }
        xCheerCount.InnerText = cheerCount.ToString();

        sav.Save(Constants.SAVE_PATH);
    }
}
