using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;
    public float MasterVolume = 1.0f;
    public float BgmVolume = 1.0f;
    public float SoundEffectVolume = 1.0f;

    private AudioSource _bgmAudioSource;
    private AudioSource _soundEffectAudioSource;

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
        _bgmAudioSource = gameObject.AddComponent<AudioSource>();
        _soundEffectAudioSource = gameObject.AddComponent<AudioSource>();
    }
    public void PlayBGM(AudioClip bgmClip)
    {
        _bgmAudioSource.clip = bgmClip;
        _bgmAudioSource.volume = MasterVolume * BgmVolume;
        _bgmAudioSource.loop = true;
        _bgmAudioSource.Play();
    }
    public void StopBGM()
    {
        _bgmAudioSource.Stop();
    }
    public void PlaySoundEffect(AudioClip soundEffectClip)
    {
        _soundEffectAudioSource.PlayOneShot(soundEffectClip, MasterVolume * SoundEffectVolume);
    }
}