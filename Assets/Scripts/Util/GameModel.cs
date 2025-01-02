using System.IO;
using UnityEngine;

// CSV, Json 등의 데이터를 관리하는 클래스
public class GameModel
{
    public CSVDataContainer PresetData { get; } = new CSVDataContainer();
    public CSVDataContainer RuntimeData { get; } = new CSVDataContainer();

    public JsonDataContainer JsonTable { get; } = new JsonDataContainer();
    public ClientData ClientData { get; private set; } = new ClientData();
    
    public void Init()
    {
        LoadCSV<TextInfo>("TextList");
        ClientData = LoadJson<ClientData>("ClientData");
    }

    private void LoadCSV<T>(string _fileName) where T : Data
    {
        // var filePath = Application.persistentDataPath + "/" + DataTablePath.CSVFilePath;
        // if (!Directory.Exists(filePath)) Directory.CreateDirectory(filePath);

        // var file = filePath + _fileName + ".csv";
        // if (!File.Exists(file)) File.Create(file);
        
        // var dataResource = Resources.Load<TextAsset>(DataTablePath.CSVFilePath + _fileName);
        // File.WriteAllText(file, dataResource.text); 
        
        // PresetData.LoadData<T>(typeof(T).Name, file);

        var path = Resources.Load<TextAsset>(DataTablePath.CSVFilePath+_fileName);
        PresetData.LoadData<T>(typeof(T).Name, path.text);
    }

    private T LoadJson<T>(string _fileName) where T : Data
    {
        var filePath = Application.persistentDataPath + "/" + DataTablePath.JsonFilePath;
        if (!Directory.Exists(filePath)) Directory.CreateDirectory(filePath);

        var file = filePath + _fileName + ".json";
        if (!File.Exists(file))
        {
            var dataResource = Resources.Load<TextAsset>(DataTablePath.JsonFilePath + _fileName);
            File.WriteAllText(file, dataResource.text);
        }
        
        var instance = typeof(T);
        JsonTable.LoadData<T>(instance.Name, file);
        return JsonTable.ReturnDatas<T>(instance.Name)[0];
    }
}
