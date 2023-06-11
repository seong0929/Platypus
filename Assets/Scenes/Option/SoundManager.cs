using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance; // �̱��� �ν��Ͻ�
    public float BgmVolume = 1.0f; // BGM ���� ��
    public float SoundEffectVolume = 1.0f; // ���� ����Ʈ ���� ��

    private void Awake()
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
}
