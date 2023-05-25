using UnityEngine;
using UnityEngine.UI;

public class OptionController : MonoBehaviour
{
    public Slider bgmSlider;
    public Slider soundEffectSlider;

    void Start()
    {
        // 저장된 볼륨 값 로드
        SoundManager soundManager = SoundManager.instance;
        bgmSlider.value = soundManager.bgmVolume;
        soundEffectSlider.value = soundManager.soundEffectVolume;

        // 슬라이더의 값이 변경될 때마다 OnVolumeChanged 메서드를 호출하도록 함
        bgmSlider.onValueChanged.AddListener(OnBgmVolumeChanged);
        soundEffectSlider.onValueChanged.AddListener(OnSoundEffectVolumeChanged);
    }

    void OnBgmVolumeChanged(float value)
    {
        // BGM 볼륨 값을 변경
        SoundManager soundManager = SoundManager.instance;
        soundManager.bgmVolume = value;
    }

    void OnSoundEffectVolumeChanged(float value)
    {
        // 사운드 이펙트 볼륨 값을 변경
        SoundManager soundManager = SoundManager.instance;
        soundManager.soundEffectVolume = value;
    }
}
