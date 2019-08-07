using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Xml.Serialization;
using System.Xml;
using System.Text;

public class DataBaseManager : MonoBehaviour {
    public MapsDataBase MapsDB;
    public TowerDataBase.Towers TowerDB;

    public EnemiesData.Enemies EnemyDB;
    public static DataBaseManager ins;

    public CallDataMap DataXMLMap = new CallDataMap();

    public Material[] MaterialAmmo;

    public int Coin;


    void Awake()
    {
        ins = this;
        //SaveDataMaps();

        //SaveDataMapsXmL();
        //SaveDataTowers();
        //SaveDataEnemies();
        LoadDataEnemies();
        LoadDataTowers();
        LoadDataMapsXML();


      
       
        TranslateAllDataMap();
        //SaveDataMapsXmL();
    }

    #region EditDataUsingJson
    public void LoadDataMaps (){
        //FileStream stream = new FileStream(Application.dataPath + "/Resources/XML/User_data.Json", FileMode.Open);
        string path = Application.dataPath + "/Resources/json/Maps_Data.json";
        if (File.Exists(path))
        {
            string jsonString = File.ReadAllText(path);
            MapsDB = JsonUtility.FromJson<MapsDataBase>(jsonString);
                    
        }
        else {
            Debug.Log("Load File Failed");
        }

    }

    public void SaveDataMaps()
    {
        string path = Application.dataPath + "/Resources/json/Maps_Data.json";
        string json = JsonUtility.ToJson(MapsDB);
        saveDatajson(path, json);
    }

    public void SaveDataTowers()
    {
        string path = Application.streamingAssetsPath + "/Tower_Data.json";
        string json = JsonUtility.ToJson(TowerDB);

        saveDatajson(path, json);

    }


    public void LoadDataTowers()
    {
        string path = Application.streamingAssetsPath + "/Tower_Data.json";
        if (File.Exists(path))
        {
            string jsonString = File.ReadAllText(path);
            TowerDB = JsonUtility.FromJson<TowerDataBase.Towers>(jsonString);
        }
        else
        {
            Debug.Log("Load File Failed");
        }

    }



    public void SaveDataEnemies()
    {
        string path = Application.streamingAssetsPath + "/Enemies_Data.json";
        string json = JsonUtility.ToJson(EnemyDB);
        saveDatajson(path, json);
    }


    public void LoadDataEnemies()
    {
        string path = Application.streamingAssetsPath + "/Enemies_Data.json";
        if (File.Exists(path))
        {
            string jsonString = File.ReadAllText(path);
            EnemyDB = JsonUtility.FromJson<EnemiesData.Enemies>(jsonString);
        }
        else
        {
            Debug.Log("Load File Failed");
        }

    }

    void saveDatajson(string path, string file)
    {
        
        StreamWriter sw = File.CreateText(path);
        sw.Close();

        File.WriteAllText(path, file);

    }

 


    #endregion



    #region EditDataUsingXml
    public void SaveDataMapsXmL()
    {
        //string path = Application.dataPath + "/Resources/xml/Maps_Data.xml";
        string path = Application.streamingAssetsPath + "/Maps_Data.xml";
        XmlSerializer serializer = new XmlSerializer(typeof(MapsDataBase));

        FileStream stream = new FileStream(path, FileMode.Create);
        var encoding = Encoding.GetEncoding("UTF-8");
        StreamWriter filename = new StreamWriter(stream, encoding);
        serializer.Serialize(filename, MapsDB);
        stream.Close();
    

    }

    public void LoadDataMapsXML()
    {
        //string path = Application.dataPath + "/Resources/xml/Maps_Data.xml";
        string path = Application.streamingAssetsPath + "/Maps_Data.xml";
        
        if (File.Exists(path)){
            XmlSerializer serializer = new XmlSerializer(typeof(MapsDataBase));
            FileStream stream = new FileStream(path, FileMode.Open);
            MapsDB = serializer.Deserialize(stream) as MapsDataBase;
            stream.Close();
        }
        else
        {

            Debug.LogWarning("Load Maps DataFailed");
        }
    }


    #endregion


    #region TranslateData
    void TranslateMapsData(int indexLevel, int indexStage)
    {
        int ValueofChar = 0;
        for (int i = 0; i < MapsDB.MapDetail[indexLevel].LevelDesign[indexStage].stageMapDetail.Length; i++)
        {
            if (MapsDB.MapDetail[indexLevel].LevelDesign[indexStage].stageMapDetail[i] == '(')
            {
                DataXMLMap.dataLevel[indexLevel].dataStage[indexStage].Widthstage = 0;
                DataXMLMap.dataLevel[indexLevel].dataStage[indexStage].detailMapsInt.Add(new List<int>());
                DataXMLMap.dataLevel[indexLevel].dataStage[indexStage].Lenghtstage++;
            }
            else if (MapsDB.MapDetail[indexLevel].LevelDesign[indexStage].stageMapDetail[i] == ',' || MapsDB.MapDetail[indexLevel].LevelDesign[indexStage].stageMapDetail[i] == ')')
            {
                DataXMLMap.dataLevel[indexLevel].dataStage[indexStage].detailMapsInt[DataXMLMap.dataLevel[indexLevel].dataStage[indexStage].Lenghtstage - 1].Add(ValueofChar);
                ValueofChar = 0;
                DataXMLMap.dataLevel[indexLevel].dataStage[indexStage].Widthstage++;
            }
            else
            {
                ValueofChar = ValueofChar * 10 + (int)char.GetNumericValue(MapsDB.MapDetail[indexLevel].LevelDesign[indexStage].stageMapDetail[i]);
            }
        }
        
        
    }

    public void TranslateAllDataMap()
    {
        DataXMLMap.dataLevel = new List<DataEchLevel>();

        for (int i = 0; i < MapsDB.MapDetail.Count; i++)
        {
            DataXMLMap.dataLevel.Add(new DataEchLevel());
            for (int j = 0; j < MapsDB.MapDetail[i].LevelDesign.Count; j++)
            {
                DataXMLMap.dataLevel[i].dataStage.Add(new DataEachStage());
                TranslateMapsData(i, j);

            }
        }
       
       
    }
    #endregion
}

[System.Serializable]
public class Level
{
    public string name;
    public List<Stage> LevelDesign = new List<Stage>();
    
}

[System.Serializable]
public class Stage
{
    public string name;
    public string stageMapDetail;
  
}

[System.Serializable]
public class MapsDataBase{
    public List<Level> MapDetail = new List<Level>();

}


public class CallDataMap
{
    public List<DataEchLevel> dataLevel = new List<DataEchLevel>();
}


public class DataEchLevel
{
    public List<DataEachStage> dataStage = new List<DataEachStage>();
}

public class DataEachStage
{
    public int Widthstage;
    public int Lenghtstage;
    public List<List<int>> detailMapsInt = new List<List<int>>();
}




