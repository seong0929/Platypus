using UnityEngine;
using TMPro;
using System.Collections.Generic;
using static Enums;

public class BattleManager : MonoBehaviour
{
    //Singleton Instance 선언
    public static BattleManager instance = null;

    [Header("# Game Control")]
    public float GameTime;  // 경과 시간
    public float MaxGameTime = Constants.Play_TIME;   //전투시간
    public TMP_Text TimerText;       // 타이머 UI
    
    private List<ESummon> SummonListA;
    private List<ESummon> SummonListB;

    public GameObject SenorZorroPrefab;
    public GameObject SpitGliderPrefab;

    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        SetupGameSummons();
        GameTime = 0;
    }

    private void SetupGameSummons()
    {
        GetCalledSummons();
        SetSummonInScene();
    }

    private void GetCalledSummons() 
    {
        GameManager gameManager = FindObjectOfType<GameManager>();
    
        if (gameManager == null)
        {
            Debug.Log("GameManager is null");
            return;
        }
        if (gameManager.Round.GroupA == null)
        {
            Debug.Log("GroupA is null");
            return;
        }
        if (gameManager.Round.GroupB == null)
        {
            Debug.Log("GroupB is null");
            return;
        }
        if (gameManager.Round.GroupA.SelectedSummon == null)
        {
            Debug.Log("GroupA.SelectedSummon is null");
            return;
        }

        SummonListA = gameManager.Round.GroupA.SelectedSummon;
        SummonListB = gameManager.Round.GroupB.SelectedSummon;
    }
    private void SetSummonInScene()
    {
        if (SummonListA == null)
        {
            Debug.Log("SummonListA is null");
            return;
        }
        if (SummonListB == null)
        {
            Debug.Log("SummonListB is null");
            return;
        }
        foreach (ESummon summon in SummonListA)
        {
            SpawnSummon(summon, true);
        }
        foreach (ESummon summon in SummonListB)
        {
            SpawnSummon(summon, false);
        }
    }
    private void SpawnSummon(ESummon summon, bool isTeamA)
    {
        GameObject summonObject = null;

        if(summon.ToString().Contains("SenorZorro"))
        {
            summonObject = Instantiate(SenorZorroPrefab);
        }
        else if(summon.ToString().Contains("SpitGlider"))
        {
            summonObject = Instantiate(SpitGliderPrefab);
        }
        else
        {
            Debug.Log("Summon is not defined");
            return;
        }
         
        if(isTeamA)
        {
            summonObject.transform.position = new Vector3(-5, 0, 0);
        }
        else
        {
            summonObject.transform.position = new Vector3(5, 0, 0);
        }
    }
    private void Update()
    {
        GameTime += Time.deltaTime;
        if (GameTime > MaxGameTime)
        {
            // ToDo: 게임 종료
        }
        UpdateTimerUI();
    }
    //일시정지
    public void PauseGame()
    {
        Time.timeScale = 0;
    }
    // 재시작
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
            summon.GetComponent<Summon>().MyTeam = true;
            //else배틀씬에서 선택은 되지 않았지만, 넘어 온 경우
            summon.GetComponent<Summon>().MyTeam = false;
        }
    }
    // UI에 타이머 값을 표시
    private void UpdateTimerUI()
    {
        float remainingTime = Mathf.Max(MaxGameTime - GameTime, 0f);

        int minutes = Mathf.FloorToInt(remainingTime / 60f);
        int seconds = Mathf.FloorToInt(remainingTime % 60f);

        TimerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}
