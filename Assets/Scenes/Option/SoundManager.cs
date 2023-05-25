using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance; // �̱��� �ν��Ͻ�
    public float bgmVolume = Constants.initialLoudness; // BGM ���� ��
    public float soundEffectVolume = Constants.initialLoudness; // ���� ����Ʈ ���� ��

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
    // ����
    // summon.GetComponent<AudioSource>().volume = GameManager.instance.volume; // �ش� ĳ���Ϳ� ����� �ҽð� ���� ��� �̰͸� �־ ��
    // summon.GetComponent<AudioSource>().Play();   // �÷��̰� �ʿ��� ���
}
