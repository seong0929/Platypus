using UnityEngine;
using TMPro;
using System.Collections.Generic;
using System.Collections;
using static Enums;

public class BattleManager : MonoBehaviour
{
    //Singleton Instance 선언
    public static BattleManager instance = null;

    [Header("# Game Control")]
    public float GameTime;  // 경과 시간
    public float MaxGameTime = Constants.Play_TIME;   //전투시간
    public TMP_Text TimerText;       // 타이머 UI
    public GameObject SpawnPoint;   // 스폰 위치

    private List<ESummon> SummonListA;
    private List<ESummon> SummonListB;
    private Transform[] TeamASpawn;
    private Transform[] TeamBSpawn;

    public GameObject SenorZorroPrefab;
    public GameObject SpitGliderPrefab;
    public GameObject PoToadPrefab;

    private Dictionary<ESummon, GameObject> summonPrefabs;

    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        TeamASpawn = GetChildTransforms(SpawnPoint.transform.Find("TeamA"));
        TeamBSpawn = GetChildTransforms(SpawnPoint.transform.Find("TeamB"));

        InitializeSummonPrefabDictionary();
        SetupGameSummons();
        GameTime = 0;
    }

    private void SetupGameSummons()
    {
        GetCalledSummons();
        SetSummonInScene();
    }

    private void InitializeSummonPrefabDictionary()
    {
        summonPrefabs = new Dictionary<ESummon, GameObject> 
        { 
            { ESummon.SenorZorro, SenorZorroPrefab },
            { ESummon.SpitGlider, SpitGliderPrefab },
            { ESummon.PoToad, PoToadPrefab },
            
            // added for test
            { ESummon.SenorZorro2, SenorZorroPrefab },
            { ESummon.SenorZorro3, SenorZorroPrefab },
            { ESummon.SenorZorro4, SenorZorroPrefab },
            { ESummon.SenorZorro5, SenorZorroPrefab },
            { ESummon.SenorZorro6, SenorZorroPrefab },
            { ESummon.SenorZorro7, SenorZorroPrefab },
            { ESummon.SenorZorro8, SenorZorroPrefab },
            { ESummon.SenorZorro9, SenorZorroPrefab },
            { ESummon.SenorZorro10, SenorZorroPrefab },
            { ESummon.SpitGlider1, SpitGliderPrefab },
            { ESummon.SpitGlider2, SpitGliderPrefab },
            { ESummon.SpitGlider3, SpitGliderPrefab }        
        };
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

        if(summonPrefabs.ContainsKey(summon) == false)
        {
            Debug.Log("Summon is not defined");
            return;
        }
        summonObject = Instantiate(summonPrefabs[summon]);
         
        if(isTeamA)
        {
            foreach (Transform point in ASpawn)
            {
                Collider2D[] colliders = Physics2D.OverlapCircleAll(point.position, 1f); // 조절 가능한 반지름 사용

                if (colliders.Length == 0)
                {
                    summonObject.transform.position = point.position;
                    break;
                }
            }
        }
        else
        {
            foreach (Transform point in BSpawn)
            {
                Collider2D[] colliders = Physics2D.OverlapCircleAll(point.position, 1f); // 조절 가능한 반지름 사용

                if (colliders.Length == 0)
                {
                    summonObject.transform.position = point.position;
                    break;
                }
            }
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
    private Transform[] GetChildTransforms(Transform parent)
    {
        int childCount = parent.childCount;
        Transform[] childTransforms = new Transform[childCount];

        for (int i = 0; i < childCount; i++)
        {
            childTransforms[i] = parent.GetChild(i);
        }

        return childTransforms;
    }
    public Transform[] ASpawn
    {
        get { return TeamASpawn; }
        set { TeamASpawn = value; }
    }
    public Transform[] BSpawn
    {
        get { return TeamBSpawn; }
        set { TeamBSpawn = value; }
    }
}
