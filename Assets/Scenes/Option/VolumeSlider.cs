using UnityEngine;
using UnityEngine.UI;

public class VolumeSlider : MonoBehaviour
{
    public Slider volumeSlider; // �����̴� UI ������Ʈ
    private SoundManager soundManager; // SoundManager ����

    private void Start()
    {
        // SoundManager �ν��Ͻ� ��������
        soundManager = SoundManager.instance;

        // �����̴��� �� ����
        volumeSlider.value = soundManager.bgmVolume;

        // �����̴� �� ���� �̺�Ʈ�� �Լ� ����
        volumeSlider.onValueChanged.AddListener(OnVolumeChanged);
    }

    // ���� ���� �̺�Ʈ �ڵ鷯
    private void OnVolumeChanged(float value)
    {
        // SoundManager�� BGM ���� �� ������Ʈ
        soundManager.bgmVolume = value;
    }
}