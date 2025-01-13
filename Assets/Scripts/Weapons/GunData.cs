using UnityEngine;

[CreateAssetMenu(fileName = "New Gun Data", menuName = "Gun Data")]
public class GunData : ScriptableObject
{
    public string gunName;
    public float damage;
    public float fireRate;
    public int magSize;
    public float reloadTime;
    public float maxDistance;
    public int currentAmmo;
    public bool reloading;
    public float projectileSpeed = 30f;
    public float bulletArc = 0f;  
    public bool canAimDownSights = true;
    public float aimDownSightsSpread = 0.1f;
    public float hipFireSpread = 0.5f;       
    public float headshotMultiplier = 2.0f;

    public AudioClip shootSFX;
    public AudioClip reloadSFX;
}