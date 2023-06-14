using UnityEngine;
using UnityEngine.UI;

public class OptionController : MonoBehaviour
{
    [SerializeField] Slider _BgmSlider;
    [SerializeField] Slider _SoundEffectSlider;

    private void Start()
    {
        // ����� ���� �� �ε�
        SoundManager soundManager = SoundManager.instance;
        _BgmSlider.value = soundManager.BgmVolume;
        _SoundEffectSlider.value = soundManager.SoundEffectVolume;

        // �����̴��� ���� ����� ������ OnVolumeChanged �޼��带 ȣ���ϵ��� ��
        _BgmSlider.onValueChanged.AddListener(OnBgmVolumeChanged);
        _SoundEffectSlider.onValueChanged.AddListener(OnSoundEffectVolumeChanged);
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
