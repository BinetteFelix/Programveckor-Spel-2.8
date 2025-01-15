using UnityEngine;

[CreateAssetMenu(fileName = "New Gun Data", menuName = "Scriptable Objects/Gun Data")]
public class GunData : ScriptableObject
{
    public string gunName;
    public bool isAutomatic;
    public float damage;
    public int magSize;
    public int ammoInMag;
    public float fireRate;
    public float reloadTime;

    public float maxDistance;
    public float projectileSpeed = 30f;
    public float bulletArc = 0f;
    public bool canAimDownSights = true;
    public float adsZoomFOV = 55f;
    public float aimDownSightsSpread = 0.1f;
    public float hipFireSpread = 0.5f;
    public float headshotMultiplier = 2.0f;

    public AudioClip shootSFX;
    public AudioClip reloadSFX;
}