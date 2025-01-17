using UnityEngine;
using System;
using System.IO;
using System.Collections.Generic;

public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance { get; private set; }

    [SerializeField] private bool autoSave = true;
    [SerializeField] private float autoSaveInterval = 300f; // 5 minutes
    
    private const string SAVE_FOLDER = "/Saves/";
    private const string SAVE_EXTENSION = ".sav";
    private float nextAutoSaveTime;
    
    private SaveData currentSaveData;
    private string SavePath => Application.persistentDataPath + SAVE_FOLDER;
    private List<ISaveable> saveableObjects = new List<ISaveable>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeSaveSystem();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void InitializeSaveSystem()
    {
        if (!Directory.Exists(SavePath))
        {
            Directory.CreateDirectory(SavePath);
        }
        currentSaveData = new SaveData();
        nextAutoSaveTime = Time.time + autoSaveInterval;
    }

    private void Update()
    {
        if (autoSave && Time.time >= nextAutoSaveTime)
        {
            SaveGame("autosave");
            nextAutoSaveTime = Time.time + autoSaveInterval;
        }
    }

    public void RegisterSaveable(ISaveable saveable)
    {
        if (!saveableObjects.Contains(saveable))
        {
            saveableObjects.Add(saveable);
        }
    }

    public void UnregisterSaveable(ISaveable saveable)
    {
        saveableObjects.Remove(saveable);
    }

    public void SaveGame(string saveSlot)
    {
        try
        {
            // Update save time
            currentSaveData.gameState.lastSaveTime = DateTime.Now;

            // Collect data from all registered objects
            foreach (var saveable in saveableObjects)
            {
                saveable.Save(currentSaveData);
            }

            // Serialize and save
            string json = JsonUtility.ToJson(currentSaveData, true);
            string filePath = SavePath + saveSlot + SAVE_EXTENSION;
            File.WriteAllText(filePath, json);

            Debug.Log($"Game saved successfully to {filePath}");
        }
        catch (Exception e)
        {
            Debug.LogError($"Error saving game: {e.Message}");
        }
    }

    public void LoadGame(string saveSlot)
    {
        try
        {
            string filePath = SavePath + saveSlot + SAVE_EXTENSION;
            if (!File.Exists(filePath))
            {
                Debug.LogWarning($"Save file not found: {filePath}");
                return;
            }

            string json = File.ReadAllText(filePath);
            currentSaveData = JsonUtility.FromJson<SaveData>(json);

            // Load data into all registered objects
            foreach (var saveable in saveableObjects)
            {
                saveable.Load(currentSaveData);
            }

            Debug.Log($"Game loaded successfully from {filePath}");
        }
        catch (Exception e)
        {
            Debug.LogError($"Error loading game: {e.Message}");
        }
    }

    public string[] GetAllSaveFiles()
    {
        try
        {
            string[] files = Directory.GetFiles(SavePath, "*" + SAVE_EXTENSION);
            for (int i = 0; i < files.Length; i++)
            {
                files[i] = Path.GetFileNameWithoutExtension(files[i]);
            }
            return files;
        }
        catch (Exception e)
        {
            Debug.LogError($"Error getting save files: {e.Message}");
            return new string[0];
        }
    }

    private void OnApplicationQuit()
    {
        SaveGame("quicksave");
    }
} 