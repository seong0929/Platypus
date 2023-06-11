using UnityEngine;
using UnityEngine.UI;

public class OptionController : MonoBehaviour
{
    [SerializeField] Slider BgmSlider;
    [SerializeField] Slider SoundEffectSlider;

    private void Start()
    {
        // 저장된 볼륨 값 로드
        SoundManager soundManager = SoundManager.instance;
        BgmSlider.value = soundManager.BgmVolume;
        SoundEffectSlider.value = soundManager.SoundEffectVolume;

        // 슬라이더의 값이 변경될 때마다 OnVolumeChanged 메서드를 호출하도록 함
        BgmSlider.onValueChanged.AddListener(OnBgmVolumeChanged);
        SoundEffectSlider.onValueChanged.AddListener(OnSoundEffectVolumeChanged);
    }

    private void OnBgmVolumeChanged(float value)
    {
        // BGM 볼륨 값을 변경
        SoundManager soundManager = SoundManager.instance;
        soundManager.BgmVolume = value;
    }
    private void OnSoundEffectVolumeChanged(float value)
    {
        // 사운드 이펙트 볼륨 값을 변경
        SoundManager soundManager = SoundManager.instance;
        soundManager.SoundEffectVolume = value;
    }
}
