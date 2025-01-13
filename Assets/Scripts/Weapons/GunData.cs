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
    public float headshotMultiplier = 2;
    public bool reloading;
}
