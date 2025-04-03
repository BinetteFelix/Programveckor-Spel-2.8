using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//andv�nd GunLibrary.Instance.EquipGun("GunName") f�r att lyckas equippa en ny pistol...

public class Gun : MonoBehaviour
{
    [SerializeField] public List<LayerMask> IgnoreLayers = new List<LayerMask>();

    [SerializeField] private float BobScale = 0.1f;
    [SerializeField] private float BobbingSpeed = 0.8f;

    [SerializeField] GameObject Player;
    [SerializeField] private Transform muzzle;
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform weapon;
    private bool reloading;
    private Vector3 aimPoint;
    Vector3 lastHitPoint;
    private float timeSinceLastShot;
    private bool isAiming = false;
    [SerializeField] private MuzzleFlash muzzleFlash;
    private float spreadMultiplier = 1f;

    [SerializeField] private float swayAmount = 0.02f;
    [SerializeField] private float smoothing = 8f;
    [SerializeField] private float maxSway = 0.06f;
    
    private Vector3 initialWeaponPosition;
    private Vector3 targetWeaponPosition;
    private Transform cameraHolder;

    private void Start()
    {
        if (GunLibrary.Instance == null)
        {
            Debug.LogError("GunLibrary instance not found!");
            enabled = false;
            return;
        }

        initialWeaponPosition = weapon.localPosition;
        targetWeaponPosition = initialWeaponPosition;
        cameraHolder = transform.root.Find("CameraHolder");
        if (cameraHolder == null)
            Debug.LogError("CameraHolder not found!");
    }

    private void FixedUpdate()
    {
        GetAimPoint();
        aimPoint = GetAimPoint();
        weapon.LookAt(aimPoint);
        timeSinceLastShot += Time.deltaTime;

        //check aiming state
        isAiming = Input.GetButton("Fire2") && GunLibrary.Instance.GetEquippedGun().canAimDownSights;

        HandleWeaponSway();
        HandleWeaponBob();
    }
    public void Shoot()
    {
        GunData gunData = GunLibrary.Instance.GetEquippedGun();

        if (!CanShoot())
        {
            if (gunData.ammoInMag <= 0 && !reloading)
            {
                // Play empty mag sound
                if (gunData.emptyMagSound != null)
                {
                    SoundFXManager.instance.PlaySoundFXclip(gunData.emptyMagSound, transform, 1f);
                }
                StartCoroutine(Reload());
            }
            return;
        }

        // Play shoot sound
        if (gunData.shootSounds != null && gunData.shootSounds.Length > 0)
        {
            SoundFXManager.instance.PlayRandomSoundFXclip(gunData.shootSounds, transform, 1f);
        }

        // Trigger muzzle flash
        if (muzzleFlash != null)
        {
            muzzleFlash.Flash();
        }

        Vector3 aimPoint = GetAimPoint();
        
        // Apply spread
        float currentSpread = (Player_ADS.Instance.IsAiming ? gunData.aimDownSightsSpread : gunData.hipFireSpread) * spreadMultiplier;

        Quaternion randomRotation = Quaternion.Euler(
            Random.Range(-currentSpread, currentSpread),
            Random.Range(-currentSpread, currentSpread),
            0f
        );

        // Apply the spread to the muzzle rotation
        Quaternion finalRotation = muzzle.rotation * randomRotation;
        
        GameObject projectile = Instantiate(projectilePrefab, muzzle.position, finalRotation);

        if (projectile.TryGetComponent<Projectile>(out var projectileComponent))
        {

            projectileComponent.Initialize(gunData.damage, gunData.headshotMultiplier);
        }

        if (projectile.TryGetComponent<Rigidbody>(out var rb))
        {
            rb.linearVelocity = finalRotation * Vector3.forward * gunData.projectileSpeed;
        }

        gunData.ammoInMag -= 1;
        timeSinceLastShot = 0;
    }

    public bool CanShoot()
    {
        return !reloading && timeSinceLastShot > GunLibrary.Instance.GetEquippedGun().fireRate;
    }

    public IEnumerator Reload()
    {
        GunData gunData = GunLibrary.Instance.GetEquippedGun();

        if (reloading) yield break;

        reloading = true;

        // Play reload sound
        if (gunData.reloadSound != null)
        {
            SoundFXManager.instance.PlaySoundFXclip(gunData.reloadSound, transform, 1f);
        }

        yield return new WaitForSeconds(gunData.reloadTime);
        gunData.ammoInMag = gunData.magSize;
        reloading = false;
    }

    private Vector3 GetAimPoint()
    {
        GunData gunData = GunLibrary.Instance.GetEquippedGun();
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        
        foreach (LayerMask ignoreLayer in IgnoreLayers)
        {
            if (Physics.Raycast(ray, out RaycastHit hitInfo, gunData.maxDistance, ~ignoreLayer))
            {
               lastHitPoint = hitInfo.point;
                return hitInfo.point;
            }
            else
            {
                return ray.GetPoint(gunData.maxDistance);
            }
        }
        return lastHitPoint;
    }
    private void HandleWeaponSway()
    {
        if (!ButtonController.Instance._IsPaused && !ButtonController.Instance._Inv_IsActive)
        {
            // Get mouse input
            float mouseX = Input.GetAxisRaw("Mouse X") * swayAmount;
            float mouseY = Input.GetAxisRaw("Mouse Y") * swayAmount;

            // Calculate target position
            targetWeaponPosition = new Vector3(
                Mathf.Clamp(mouseX, -maxSway, maxSway),
                Mathf.Clamp(mouseY, -maxSway, maxSway),
                initialWeaponPosition.z
            );

            // Apply sway with smoothing
            weapon.localPosition = Vector3.Lerp(
                weapon.localPosition,
                initialWeaponPosition + targetWeaponPosition,
                Time.deltaTime * smoothing
            );

            // Reduce sway while aiming
            if (isAiming)
            {
                weapon.localPosition = Vector3.Lerp(
                    weapon.localPosition,
                    initialWeaponPosition,
                    Time.deltaTime * smoothing * 2
                );
            }
        }
    }

    public void SetSpreadMultiplier(float multiplier)
    {
        spreadMultiplier = multiplier;
    }

    public void HandleWeaponBob()
    {
        if (PlayerMovement.Instance.isGrounded)
        {
            float scale = 1f / (3 - Mathf.Cos(2 * Time.time));

            transform.localPosition = new Vector3(
            transform.localPosition.x + BobScale * (scale * Mathf.Cos(Time.time * BobbingSpeed) * Time.deltaTime),
            transform.localPosition.y + BobScale * (scale * Mathf.Sin(2 * Time.time * BobbingSpeed) / 2 * Time.deltaTime),
            transform.localPosition.z
            );
        }

        //When only Aiming
        if (isAiming && PlayerMovement.Instance.isGrounded &&
            !PlayerMovement.Instance.isCrouching) {
            BobbingSpeed = 1.425f; BobScale = 0.1f; }

        //When Crouching and Aiming
        else if (isAiming && PlayerMovement.Instance.isGrounded &&
            PlayerMovement.Instance.isCrouching) {
            BobbingSpeed = 0.85f; BobScale = 0.1f; }

        //When only Crouching
        else if (!isAiming && PlayerMovement.Instance.isGrounded && PlayerMovement.Instance.isCrouching) {
            BobbingSpeed = 1.15f; BobScale = 0.1f; }

        //When only Sprinting
        else if (!isAiming && PlayerMovement.Instance.isGrounded &&
            !PlayerMovement.Instance.isCrouching && PlayerMovement.Instance.wasSprintingLastFrame) { 
            BobbingSpeed = 9.75f; BobScale = 0.5f; }

        //When only Walking
        if (!isAiming && PlayerMovement.Instance.isGrounded && !PlayerMovement.Instance.isCrouching &&
             PlayerMovement.Instance.currentSpeed == PlayerMovement.Instance.walkSpeed && 
             PlayerMovement.Instance.isMoving) {
            BobbingSpeed = 7.25f; BobScale = 0.1f; }
            
            //When only STANDING still
        else if (!isAiming && PlayerMovement.Instance.isGrounded && !PlayerMovement.Instance.isCrouching &&
            PlayerMovement.Instance.currentSpeed < PlayerMovement.Instance.walkSpeed && 
            !PlayerMovement.Instance.isMoving) {
            BobbingSpeed = 1.75f; BobScale = 0.1f; }
    }
}   