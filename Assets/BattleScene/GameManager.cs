using UnityEngine;

public class GameManager : MonoBehaviour
{
    //Singleton Instance 선언
    public static GameManager instance = null;

    [Header("# Game Control")]
    public float GameTime;
    public float MaxGameTime = Constants.playtime;   //전투시간

    private void Awake()
    {
        instance = this;
        //Scene 이동 시 삭제 되지 않도록 처리
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
