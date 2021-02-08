using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    private static readonly float muteVolume = -80f;

    [SerializeField]
    private AudioMixer masterMixer;
    [SerializeField]
    private AudioMixer soundMixer;

    [SerializeField]
    private AudioSource audioSource;

    [SerializeField, Range(-80f, 20f)]
    private float masterVolume;
    [SerializeField, Range(-80f, 20f)]
    private float musicVolume;
    [SerializeField, Range(-80f, 20f)]
    private float soundVolume;

    public bool muteMaster;
    public bool muteMusic;
    public bool muteSound;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
#if UNITY_EDITOR
        MasterVolume = masterVolume;
        MusicVolume = musicVolume;
        SoundVolume = soundVolume;
#endif
    }

    public float MasterVolume
    {
        get
        {
            if (muteMaster)
            {
                return muteVolume;
            }
            else
            {
                return masterVolume;
            }
        }
        set
        {
            masterVolume = value;
            if (muteMaster)
            {
                masterMixer.SetFloat("masterVolume", muteVolume);
            }
            else
            {
                masterMixer.SetFloat("masterVolume", masterVolume);
            }
        }
    }
    
    public float MusicVolume
    {
        get
        {
            if (muteMusic)
            {
                return muteVolume;
            }
            else
            {
                return musicVolume;
            }
        }
        set
        {
            musicVolume = value;
            if (muteMusic)
            {
                masterMixer.SetFloat("musicVolume", muteVolume);
            }
            else
            {
                masterMixer.SetFloat("musicVolume", musicVolume);
            }
        }
    }

    public float SoundVolume
    {
        get
        {
            if (muteSound)
            {
                return muteVolume;
            }
            else
            {
                return soundVolume;
            }
        }
        set
        {
            soundVolume = value;
            if (muteSound)
            {
                masterMixer.SetFloat("soundVolume", muteVolume);
            }
            else
            {
                masterMixer.SetFloat("soundVolume", soundVolume);
            }
        }
    }

    public AudioClip Music
    {
        get => audioSource.clip;
        set => audioSource.clip = value;
    }

    public void PlayMusic()
    {
        if (audioSource.isPlaying)
        {
            return;
        }
        audioSource.Play();
    }

    public void PauseMusic()
    {
        audioSource.Pause();
    }

    public void StopMusic()
    {
        audioSource.Stop();
    }
}
