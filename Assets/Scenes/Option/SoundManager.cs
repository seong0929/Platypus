using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance; // 싱글톤 인스턴스
    public float bgmVolume = Constants.initialLoudness; // BGM 볼륨 값
    public float soundEffectVolume = Constants.initialLoudness; // 사운드 이펙트 볼륨 값

    void Awake()
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
    // 사용법
    // summon.GetComponent<AudioSource>().volume = GameManager.instance.volume; // 해당 캐릭터에 오디오 소시가 있을 경우 이것만 있어도 됨
    // summon.GetComponent<AudioSource>().Play();   // 플레이가 필요한 경우
}
