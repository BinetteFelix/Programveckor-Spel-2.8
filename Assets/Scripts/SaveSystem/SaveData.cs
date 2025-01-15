using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SaveData
{
    // Player Data
    public PlayerSaveData playerData = new PlayerSaveData();
    
    // Weapon Data
    public WeaponSaveData weaponData = new WeaponSaveData();
    
    // Game State
    public GameStateSaveData gameState = new GameStateSaveData();
}

[Serializable]
public class PlayerSaveData
{
    public Vector3 position;
    public Quaternion rotation;
    public float currentHealth;
    public float currentStamina;
    // Add more player-specific data as needed
}

[Serializable]
public class WeaponSaveData
{
    public string equippedGunName;
    public Dictionary<string, int> gunAmmoData = new Dictionary<string, int>();
    // Add more weapon-specific data as needed
}

[Serializable]
public class GameStateSaveData
{
    public float playTime;
    public int scoreCount;
    public string currentLevel;
    public DateTime lastSaveTime;
    // Add more game state data as needed
} 