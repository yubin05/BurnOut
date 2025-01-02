using System;
using System.IO;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

// CSV 데이터를 보관하고 있는 클래스
public class CSVDataContainer
{
    private Dictionary<string, DataTable> dataTables = new Dictionary<string, DataTable>();

    public void LoadData<T>(string dataTableName, string path) where T : Data
    {
        var instances = GetData<T>(path);
        foreach (var instance in instances)
        {
            AddData(dataTableName, instance.Id, instance);
        }
    }

    public T ReturnData<T>(string dataTableName, int id) where T : Data
    {
        var dataTable = dataTables[dataTableName];
        return (T)dataTable.GetData(id);
    }
    public T[] ReturnDatas<T>(string dataTableName) where T : Data
    {
        var dataTable = dataTables[dataTableName];
        return dataTable.GetDatas().Select(x => (T)x).ToArray();
    }

    public void AddData(string dataTableName, int id, Data data)
    {
        if (!dataTables.ContainsKey(dataTableName))
            dataTables.Add(dataTableName, new DataTable(dataTableName, new Dictionary<int, Data>()));

        data.Id = id;
        data.TableModel = dataTables[dataTableName];;
        dataTables[dataTableName].AddData(id, data);
    }

    public List<T> GetData<T>(string _file)
    {
        var list = new List<T>();
        // var lines = File.ReadLines(_file).ToList();
        var lines = GetCSVLines(_file);
        var headerLine = lines.First();
        var colNames = headerLine.Split(',');
        var rows = lines.Skip(1);

        var properties = typeof(T).GetProperties();

        rows.ToList().ForEach(r =>
        {
            var cells = r.Split(',');

            var obj = (T)Activator.CreateInstance(typeof(T));

            var index = 0;

            foreach (var colName in colNames)
            {
                var prop = properties.SingleOrDefault(p => p.Name == colName);
                //Debug.Log(colName); Debug.Log(prop);
                Type propertyType = prop.PropertyType;
                var value = cells[index++];

                if (!propertyType.IsEnum)
                    prop.SetValue(obj, Convert.ChangeType(value, propertyType));
                else
                    prop.SetValue(obj, Enum.Parse(propertyType, value));
            }

            list.Add(obj);
        });

        return list;
    }

    public List<string> GetCSVLines(string _file)
    {
        return Regex.Split(_file, @"\r\n|\n\r|\n|\r").ToList();
    }
}
