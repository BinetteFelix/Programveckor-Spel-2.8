using System;
using UnityEngine;

public class WeaponBobTest : MonoBehaviour
{
    Vector3 LeftBobTarget = new Vector3 (-0.5f, 0f, 0f);
    Vector3 RightBobTarget = new Vector3(0.5f, 0f, 0f);

    bool _LeftRightBobChecker = false;

    Vector3 BobbingX = new Vector3(0, 0, 0);
    Vector3 BobbingY = new Vector3(0 ,0 ,0);

    [SerializeField] private float BobbingRadius = 0.2f;
    [SerializeField] private float BobbingSpeed = 1.0f;
    [SerializeField] private float BobbingAngleX = 2.0f;
    [SerializeField] private float BobbingAngleY = 5.0f;

    Vector3 weaponposition;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        HandleWeaponBob();
    }

    public void HandleWeaponBob()
    {
        BobbingAngleX += BobbingSpeed * Time.time;
        BobbingAngleY += BobbingSpeed * Time.time;

        if (transform.position.x == RightBobTarget.x && _LeftRightBobChecker)
        {
            _LeftRightBobChecker = false;
            Debug.Log("Bobbing Left");
        }
        else if (transform.position.x == LeftBobTarget.x && !_LeftRightBobChecker)
        {
            _LeftRightBobChecker = true;
            Debug.Log("Bobbing right");
        }

        if(!_LeftRightBobChecker)
        {
            BobbingX -= transform.right * -Mathf.Cos(BobbingAngleX) * BobbingRadius;
            BobbingY += transform.up * Mathf.Sin(BobbingAngleY) * BobbingRadius;
            
        }
        else if (_LeftRightBobChecker)
        {
            BobbingX -= transform.right * -Mathf.Cos(BobbingAngleX) * BobbingRadius;
            BobbingY += transform.up * Mathf.Sin(BobbingAngleY) * BobbingRadius;
            
        }

        transform.position = new Vector3(BobbingX.x, BobbingY.y, 0);
    }
}
