using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance; // ½Ì±ÛÅæ ÀÎ½ºÅÏ½º
    public float BgmVolume = 1.0f; // BGM º¼·ý °ª
    public float SoundEffectVolume = 1.0f; // »ç¿îµå ÀÌÆåÆ® º¼·ý °ª

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
