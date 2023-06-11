using UnityEngine;
using UnityEngine.UI;

public class OptionController : MonoBehaviour
{
    [SerializeField] Slider BgmSlider;
    [SerializeField] Slider SoundEffectSlider;

    private void Start()
    {
        // ����� ���� �� �ε�
        SoundManager soundManager = SoundManager.instance;
        BgmSlider.value = soundManager.BgmVolume;
        SoundEffectSlider.value = soundManager.SoundEffectVolume;

        // �����̴��� ���� ����� ������ OnVolumeChanged �޼��带 ȣ���ϵ��� ��
        BgmSlider.onValueChanged.AddListener(OnBgmVolumeChanged);
        SoundEffectSlider.onValueChanged.AddListener(OnSoundEffectVolumeChanged);
    }

    private void OnBgmVolumeChanged(float value)
    {
        // BGM ���� ���� ����
        SoundManager soundManager = SoundManager.instance;
        soundManager.BgmVolume = value;
    }
    private void OnSoundEffectVolumeChanged(float value)
    {
        // ���� ����Ʈ ���� ���� ����
        SoundManager soundManager = SoundManager.instance;
        soundManager.SoundEffectVolume = value;
    }
}
