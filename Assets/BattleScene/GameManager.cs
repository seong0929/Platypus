using UnityEngine;

public class GameManager : MonoBehaviour
{
    //Singleton Instance ����
    public static GameManager instance = null;

    [Header("# Game Control")]
    public float GameTime;
    public float MaxGameTime = Constants.playtime;   //�����ð�

    private void Awake()
    {
        instance = this;
        //Scene �̵� �� ���� ���� �ʵ��� ó��
        //DontDestroyOnLoad(this.gameObject);
    }
    private void Update()
    {
        GameTime += Time.deltaTime;
        if (GameTime > MaxGameTime)
        {
            GameTime = MaxGameTime;
        }
    }
    public void PauseGame()
    {
        Time.timeScale = 0;
    }
    public void ResumeGame()
    {
        Time.timeScale = 1;
    }
}
