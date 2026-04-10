using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SaveDataManager : MonoBehaviour
{
    [Header("Save File Config")]
    [SerializeField] private string _fileName;

    public static SaveDataManager Instance { get; private set; }
    private SaveData _saveData;
    private List<ISaveData> _saveDataObjects;
    private FileDataHandler _fileDataHandler;

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(gameObject);
        else
            Instance = this;
        
        transform.parent = null;
        DontDestroyOnLoad(gameObject);
    }
    
    private void Start()
    {
        _fileDataHandler = new FileDataHandler(Application.persistentDataPath ,_fileName);
        _saveDataObjects = FindAllSaveDataObjects();
        LoadData();
    }

    private void OnApplicationQuit()
    {
        SaveData();
    }
    
    public void SaveData()
    {
        foreach (ISaveData saveDataObject in _saveDataObjects)
            saveDataObject.SaveData(ref _saveData);
        
        _fileDataHandler.SaveData(_saveData);
    }

    public void LoadData()
    {
        _saveData = _fileDataHandler.LoadData();
        
        if(_saveData == null)
            _saveData = new ();

        foreach (ISaveData saveDataObject in _saveDataObjects)
            saveDataObject.LoadData(_saveData);
    }

    private List<ISaveData> FindAllSaveDataObjects()
    {
        IEnumerable<ISaveData> saveDataObjects = FindObjectsOfType<MonoBehaviour>().OfType<ISaveData>();
        return new List<ISaveData>(saveDataObjects);
    }
}
