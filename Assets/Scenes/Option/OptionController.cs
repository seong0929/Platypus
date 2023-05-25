using UnityEngine;
using UnityEngine.UI;

public class OptionController : MonoBehaviour
{
    public Slider bgmSlider;
    public Slider soundEffectSlider;

    void Start()
    {
        // ����� ���� �� �ε�
        SoundManager soundManager = SoundManager.instance;
        bgmSlider.value = soundManager.bgmVolume;
        soundEffectSlider.value = soundManager.soundEffectVolume;

        // �����̴��� ���� ����� ������ OnVolumeChanged �޼��带 ȣ���ϵ��� ��
        bgmSlider.onValueChanged.AddListener(OnBgmVolumeChanged);
        soundEffectSlider.onValueChanged.AddListener(OnSoundEffectVolumeChanged);
    }

    void OnBgmVolumeChanged(float value)
    {
        // BGM ���� ���� ����
        SoundManager soundManager = SoundManager.instance;
        soundManager.bgmVolume = value;
    }

    void OnSoundEffectVolumeChanged(float value)
    {
        // ���� ����Ʈ ���� ���� ����
        SoundManager soundManager = SoundManager.instance;
        soundManager.soundEffectVolume = value;
    }
}
