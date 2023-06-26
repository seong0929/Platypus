using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;
    public float BgmVolume = 1.0f;
    public float SoundEffectVolume = 1.0f;

    private AudioSource bgmAudioSource;
    private AudioSource soundEffectAudioSource;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void Start()
    {
        bgmAudioSource = gameObject.AddComponent<AudioSource>();
        soundEffectAudioSource = gameObject.AddComponent<AudioSource>();
    }
    public void PlayBGM(AudioClip bgmClip)
    {
        bgmAudioSource.clip = bgmClip;
        bgmAudioSource.volume = BgmVolume;
        bgmAudioSource.loop = true;
        bgmAudioSource.Play();
    }
    public void StopBGM()
    {
        bgmAudioSource.Stop();
    }
    public void PlaySoundEffect(AudioClip soundEffectClip)
    {
        soundEffectAudioSource.PlayOneShot(soundEffectClip, SoundEffectVolume);
    }
}