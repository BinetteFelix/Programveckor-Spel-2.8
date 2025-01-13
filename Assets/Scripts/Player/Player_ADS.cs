using TMPro.Examples;
using UnityEngine;

public class Player_ADS : MonoBehaviour
{
    public CameraController cameraController;
    [SerializeField] GameObject Weapon;
    [SerializeField] Transform ADSPosition;
    [SerializeField] Transform DefaultPos;
    private float AnimationSpeed = 9.5f;

    void Update()
    {
        if (cameraController._LockStateLocked == true)
        {
            if (Input.GetMouseButton(1))
            {
                Weapon.transform.position = Vector3.Slerp(Weapon.transform.position, ADSPosition.transform.position, AnimationSpeed * Time.deltaTime);
            }
            else
            {
                Weapon.transform.position = Vector3.Slerp(Weapon.transform.position, DefaultPos.transform.position, AnimationSpeed * Time.deltaTime);
            }
        }
    }
}
