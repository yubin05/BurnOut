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

    public DataTable(string TableName, Dictionary<int, Data> DataContainer)
    {
        this.TableName = TableName;
        this.DataContainer = DataContainer;
    }

    public void AddData(int id, Data data)
    {
        // data.InstanceId = ++currentInstanceId;
        DataContainer.Add(id, data);
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
