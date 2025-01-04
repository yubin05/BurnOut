using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DataTablePath
{
    public static string CSVFilePath { get; } = "Data/TableList/";
    public static string JsonFilePath { get; } = "Data/Json/";
}

public class DataTable
{
    public string TableName { get; set; }
    private Dictionary<int, Data> DataContainer;
    private int instanceId;

    public DataTable(string TableName, Dictionary<int, Data> DataContainer)
    {
        this.TableName = TableName;
        this.DataContainer = DataContainer;
        instanceId = 0;
    }

    public void AddData(int id, Data data)
    {
        DataContainer.Add(id, data);
    }
    public void AddData(Data data)
    {
        data.Id = ++instanceId;
        DataContainer.Add(data.Id, data);
    }

    public Data GetData(int id)
    {
        return DataContainer[id];
    }
    public Data[] GetDatas()
    {
        return DataContainer.Values.Select(x => x).ToArray();
    }
}
