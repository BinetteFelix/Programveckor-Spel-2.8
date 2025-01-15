using UnityEngine;

[System.Serializable]
public class GunData : ScriptableObject
{
    public string gunName;
    public bool isAutomatic;
    public bool canAimDownSights = true;
    public float adsZoomFOV = 1.5f;

    public float damage = 10f;
    public float headshotMultiplier = 2f;
    
    public float fireRate = 0.1f;
    public float projectileSpeed = 100f;
    public float maxDistance = 100f;
    
    public float hipFireSpread = 1f;
    public float aimDownSightsSpread = 0.1f;
    
    
    public int magSize = 30;
    public float reloadTime = 1f;
    public int ammoInMag;

    public AudioClip[] shootSoundClips;
    public AudioClip[] reloadSoundClips;
    public AudioClip emptyMagSound;
}