using UnityEngine;

public class SoundFXManager : MonoBehaviour
{
    //TED

    public static SoundFXManager instance;
    [SerializeField] private AudioSource soundFXObject;
    AudioSource loopSource;

    private void Awake()
    {
        loopSource = GetComponent<AudioSource>();
        if (instance == null)
        {
            instance = this;
        }
    }

    public void PlayLoopingSFX(AudioClip audioClip)
    {
        if (loopSource.isPlaying == false)
        {
            loopSource.PlayOneShot(audioClip);
        }

    }


    public void PlaySoundFXclip(AudioClip audioClip, Transform spawnTransform, float volume)
    {
        //spawn in gameObject
        AudioSource audioSource = Instantiate(soundFXObject, spawnTransform.position, Quaternion.identity);

        //Assign audioclip
        audioSource.clip = audioClip;

        //play sound
        audioSource.Play();

        //get lenght of sound fx clip
        float clipLenght = audioSource.clip.length;

        //destroy object
        Destroy(audioSource.gameObject, clipLenght);
    }


    public void PlayRandomSoundFXclip(AudioClip[] audioClip, Transform spawnTransform, float volume)
    {
        //assign a random index
        int rand = Random.Range(0, audioClip.Length);

        //spawn in gameObject
        AudioSource audioSource = Instantiate(soundFXObject, spawnTransform.position, Quaternion.identity);

        //Assign audioclip
        audioSource.clip = audioClip[rand];

        //play sound
        audioSource.Play();

        //get lenght of sound fx clip
        float clipLenght = audioSource.clip.length;

        //destroy object
        Destroy(audioSource.gameObject, clipLenght);
    }
}
