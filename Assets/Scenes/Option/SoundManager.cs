using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;
    public float MasterVolume { get; set; } = 1.0f;
    public float BgmVolume { get; set; } = 1.0f;
    public float SoundEffectVolume { get; set; } = 1.0f;

    private AudioSource _bgmAudioSource;
    private AudioSource _soundEffectAudioSource;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);

            _bgmAudioSource = gameObject.AddComponent<AudioSource>();
            _soundEffectAudioSource = gameObject.AddComponent<AudioSource>();
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void Start()
    {
        LoadSettings();
    }
    #region 저장
    // 설정 값 불러오기
    private void LoadSettings()
    {
        // BGM 볼륨 로드
        BgmVolume = PlayerPrefs.GetFloat("SoundManager_MasterVolume", 1.0f);

        // BGM 볼륨 로드
        BgmVolume = PlayerPrefs.GetFloat("SoundManager_BgmVolume", 1.0f);

        // 사운드 이펙트 볼륨 로드
        SoundEffectVolume = PlayerPrefs.GetFloat("SoundManager_SoundEffectVolume", 1.0f);
    }
    // 설정 값 저장하기
    public void SaveSettings()
    {
        // BGM 볼륨 저장
        PlayerPrefs.SetFloat("SoundManager_MasterVolume", MasterVolume);

        // BGM 볼륨 저장
        PlayerPrefs.SetFloat("SoundManager_BgmVolume", BgmVolume);

        // 사운드 이펙트 볼륨 저장
        PlayerPrefs.SetFloat("SoundManager_SoundEffectVolume", SoundEffectVolume);

        // PlayerPrefs 데이터를 디스크에 저장
        PlayerPrefs.Save();
    }
    #endregion
    #region 사운드
    // BGM 재생
    public void PlayBGM(AudioClip bgmClip)
    {
        _bgmAudioSource.clip = bgmClip;
        _bgmAudioSource.volume = MasterVolume * BgmVolume;
        _bgmAudioSource.loop = true;
        _bgmAudioSource.Play();
    }
    // BGM 멈추기
    public void StopBGM()
    {
        _bgmAudioSource.Stop();
    }
    // 사운드 이팩트 재생
    public void PlaySoundEffect(AudioClip soundEffectClip)
    {
        _soundEffectAudioSource.PlayOneShot(soundEffectClip, MasterVolume * SoundEffectVolume);
    }
    #endregion
}