using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [HideInInspector] public static AudioManager Instance;

    [SerializeField] private AudioSource SprintBreathing;
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    public void PlayBreathingWhileSprinting(int State)
    {
        if(State == 1)
        {
            SprintBreathing.Play();
        }
        else if(State == 2)
        {
            SprintBreathing.Pause();
        }
        else if (State == 3) 
        {
            SprintBreathing.UnPause();
        }
    }
}
