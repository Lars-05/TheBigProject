using System;
using System.IO;
using UnityEngine;

public class FileDataHandler
{
    private readonly string _saveFilePath;
    private readonly string _dataFileName;
    
    public FileDataHandler(string saveFilePath, string dataFileName)
    {
        _saveFilePath = saveFilePath;
        _dataFileName = dataFileName;
    }

    public void SaveData(SaveData saveData)
    {
        string fullPath = Path.Combine(_saveFilePath,  _dataFileName);
        try
        {
            Directory.CreateDirectory(Path.GetDirectoryName(fullPath));
            string saveDataJSON = JsonUtility.ToJson(saveData, true);

            using FileStream fileStream = new FileStream(fullPath, FileMode.Create);
            using StreamWriter streamWriter = new StreamWriter(fileStream);
            streamWriter.Write(saveDataJSON);
        }
        catch(Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
    
    public SaveData LoadData()
    {
        string fullPath = Path.Combine(_saveFilePath,  _dataFileName);
        SaveData loadedData = null;

        if (!File.Exists(fullPath)) return loadedData;
        
        try
        {
            string saveDataJSON = "";
            using FileStream fileStream = new FileStream(fullPath, FileMode.Open);
            using StreamReader streamReader = new StreamReader(fileStream);
                
            saveDataJSON = streamReader.ReadToEnd();
                
            loadedData = JsonUtility.FromJson<SaveData>(saveDataJSON);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
        return loadedData;
    }
}
