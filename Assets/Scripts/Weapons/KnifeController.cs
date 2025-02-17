using UnityEngine;

public class KnifeController : MonoBehaviour
{
    [Header("Knife Settings")]
    [SerializeField] private float damage = 50f;
    [SerializeField] private float range = 2f;
    [SerializeField] private float attackRate = 0.5f;
    
    [Header("Position Settings")]
    [SerializeField] private Transform knifeModel;
    [SerializeField] private Vector3 rotationOffset = new Vector3(0f, -90f, 0f);
    
    [Header("Swing Settings")]
    [SerializeField] private float swingSpeed = 15f;
    [SerializeField] private Vector3 swingRotation = new Vector3(45f, -60f, 30f);

    [Header("Audio")]
    [SerializeField] private AudioClip[] swingSounds;
    [SerializeField] private AudioClip[] hitSounds;
    [SerializeField] private float swingVolume = 1f;
    [SerializeField] private float hitVolume = 1f;
    
    private bool isSwinging;
    private float lastAttackTime;
    private Camera mainCamera;
    private Quaternion targetRotation;
    private Quaternion defaultRotation;

    private void Start()
    {
        mainCamera = Camera.main;
        defaultRotation = Quaternion.Euler(rotationOffset);
    }

    private void Update()
    {
        if (mainCamera == null) return;

        // Handle attack input
        if (Input.GetMouseButtonDown(0) && Time.time >= lastAttackTime + attackRate)
        {
            Swing();
        }

        // Handle swing animation
        if (isSwinging)
        {
            knifeModel.localRotation = Quaternion.Lerp(knifeModel.localRotation, targetRotation, Time.deltaTime * swingSpeed);
            
            if (Quaternion.Angle(knifeModel.localRotation, targetRotation) < 1f)
            {
                isSwinging = false;
                targetRotation = defaultRotation;
            }
        }
        else
        {
            // Smooth return to default rotation
            knifeModel.localRotation = Quaternion.Lerp(knifeModel.localRotation, defaultRotation, Time.deltaTime * swingSpeed);
        }
    }

    private void Swing()
    {
        if (isSwinging) return;

        isSwinging = true;
        lastAttackTime = Time.time;

        // Play swing sound
        if (swingSounds != null && swingSounds.Length > 0)
        {
            AudioClip swingSound = swingSounds[Random.Range(0, swingSounds.Length)];
            SoundFXManager.instance.PlaySoundFXclip(swingSound, transform, swingVolume);
        }

        // Set diagonal swing rotation
        targetRotation = Quaternion.Euler(rotationOffset + swingRotation);

        // Perform the attack raycast
        RaycastHit hit;
        if (Physics.Raycast(mainCamera.transform.position, mainCamera.transform.forward, out hit, range))
        {
            if (hit.collider.TryGetComponent<IDamageable>(out var target))
            {
                target.TakeDamage(damage);
                
                // Play hit sound
                if (hitSounds != null && hitSounds.Length > 0)
                {
                    AudioClip hitSound = hitSounds[Random.Range(0, hitSounds.Length)];
                    SoundFXManager.instance.PlaySoundFXclip(hitSound, transform, hitVolume);
                }
                
                Debug.Log($"Hit {hit.collider.name} for {damage} damage");
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (mainCamera != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawRay(mainCamera.transform.position, mainCamera.transform.forward * range);
        }
    }
} 