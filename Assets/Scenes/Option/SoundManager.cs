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
    #region ����
    // ���� �� �ҷ�����
    private void LoadSettings()
    {
        // BGM ���� �ε�
        BgmVolume = PlayerPrefs.GetFloat("SoundManager_MasterVolume", 1.0f);

        // BGM ���� �ε�
        BgmVolume = PlayerPrefs.GetFloat("SoundManager_BgmVolume", 1.0f);

        // ���� ����Ʈ ���� �ε�
        SoundEffectVolume = PlayerPrefs.GetFloat("SoundManager_SoundEffectVolume", 1.0f);
    }
    // ���� �� �����ϱ�
    public void SaveSettings()
    {
        // BGM ���� ����
        PlayerPrefs.SetFloat("SoundManager_MasterVolume", MasterVolume);

        // BGM ���� ����
        PlayerPrefs.SetFloat("SoundManager_BgmVolume", BgmVolume);

        // ���� ����Ʈ ���� ����
        PlayerPrefs.SetFloat("SoundManager_SoundEffectVolume", SoundEffectVolume);

        // PlayerPrefs �����͸� ��ũ�� ����
        PlayerPrefs.Save();
    }
    #endregion
    #region ����
    // BGM ���
    public void PlayBGM(AudioClip bgmClip)
    {
        _bgmAudioSource.clip = bgmClip;
        _bgmAudioSource.volume = MasterVolume * BgmVolume;
        _bgmAudioSource.loop = true;
        _bgmAudioSource.Play();
    }
    // BGM ���߱�
    public void StopBGM()
    {
        _bgmAudioSource.Stop();
    }
    // ���� ����Ʈ ���
    public void PlaySoundEffect(AudioClip soundEffectClip)
    {
        _soundEffectAudioSource.PlayOneShot(soundEffectClip, MasterVolume * SoundEffectVolume);
    }
    #endregion
}