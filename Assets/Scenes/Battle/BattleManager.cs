using UnityEngine;

public class BattleManager : MonoBehaviour
{
    //Singleton Instance 선언
    public static BattleManager instance = null;

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
    //ToDo: 배틀씬에서 넘어온 소환수를 인수에 넣기
    public void AssignTeam(GameObject[] summons)
    {
        //ToDo: 배틀씬에서 user가 선택했는 지 안 했는 지 판단하는 함수 필요
        foreach (GameObject summon in summons)
        {
            //if() 배틀씬에서 선택을 했다면
            summon.GetComponent<Summon>().myTeam = true;
            //else배틀씬에서 선택은 되지 않았지만, 넘어 온 경우
            summon.GetComponent<Summon>().myTeam = false;
        }
    }
}
