// BattleManager.cs: Setting up Battle
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
    [SerializeField]
    private GameObject SpawnPoint;   // 스폰 위치

    private List<ESummon> SummonListA;
    private List<ESummon> SummonListB;
    private Transform[] TeamASpawn;
    private Transform[] TeamBSpawn;

    private List<Summon> teamASummons = new List<Summon>();
    private List<Summon> teamBSummons = new List<Summon>();

    #region Summon Prefabs
    [SerializeField]
    private GameObject SenorZorroPrefab;
    [SerializeField]
    private GameObject SpitGliderPrefab;
    [SerializeField]
    private GameObject PoToadPrefab;
    #endregion

    private Dictionary<ESummon, GameObject> summonPrefabs;

    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        IsComponentsValid();

        // Get TeamA/B Spawn from the SpawnPoint
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
            //{ ESummon.SpitGlider, SpitGliderPrefab },
            //{ ESummon.PoToad, PoToadPrefab },
            
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
            //{ ESummon.SpitGlider1, SpitGliderPrefab },
            //{ ESummon.SpitGlider2, SpitGliderPrefab },
            //{ ESummon.SpitGlider3, SpitGliderPrefab }        
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

        for(int i = 0; i < SummonListA.Count; i++)
        {
            SpawnSummon(SummonListA[i], true, i);
        }
        for (int i = 0; i < SummonListB.Count; i++)
        {
            SpawnSummon(SummonListB[i], false, i);
        }

        foreach (Summon summon in teamASummons)
        {
            summon.SetSummonTeamEnemies(teamASummons, teamBSummons);
        }
        foreach (Summon summon in teamBSummons)
        {
            summon.SetSummonTeamEnemies(teamBSummons, teamASummons);
        }
    }
    private void SpawnSummon(ESummon summon, bool isTeamA, int position)
    {
        GameObject summonObject = null;

        if(summonPrefabs.ContainsKey(summon) == false)
        {
            Debug.Log("Summon is not defined");
            return;
        }
        summonObject = Instantiate(summonPrefabs[summon]);
         
        SummonStats stats = new SummonStats();
        stats.Health = 100f;
        stats.Defence = 0f;
        stats.MoveSpeed = 0.5f;
        stats.DamageCoefficient = 1f;
        stats.ActionSpeedCoefficient = 1f;
        stats.CriticalChanceCoefficient = 1.5f;

        Vector2 spawnPosition = Vector2.zero;

        if(isTeamA)
        {
            if(TeamASpawn.Length <= position)
            {
                position = TeamASpawn.Length - 1;
            }
            if(position < 0)
            {
                position = 0;
            }
            spawnPosition = TeamASpawn[position].position;
            summonObject.GetComponent<Summon>().SetSummon(stats, teamASummons, teamBSummons, spawnPosition);
            summonObject.transform.position = spawnPosition;
            teamASummons.Add(summonObject.GetComponent<Summon>());
        }
        else
        {
            if (TeamBSpawn.Length <= position)
            {
                position = TeamBSpawn.Length - 1;
            }
            if (position < 0)
            {
                position = 0;
            }
            spawnPosition = TeamBSpawn[position].position;
            summonObject.GetComponent<Summon>().SetSummon(stats, teamBSummons, teamASummons, spawnPosition);
            summonObject.transform.position = spawnPosition;

            teamBSummons.Add(summonObject.GetComponent<Summon>());
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

    #region Helper Functions
    // check if any required components are missing
    private bool IsComponentsValid()
    {
        // check if TeamASpawn and TeamBSpawn are null
        if (SpawnPoint == null)
        {
            Debug.Log("SpawnPoint is null");
            return false;
        }
        if (SpawnPoint.transform.Find("TeamA") == null)
        {
            Debug.Log("TeamA is null");
            return false;
        }
        if (SpawnPoint.transform.Find("TeamB") == null)
        {
            Debug.Log("TeamB is null");
            return false;
        }

        // check needed UIs are null
        if (TimerText == null)
        {
            Debug.Log("TimerText is null");
            return false;
        }

        // check if Summon Prefabs are null
        if (SenorZorroPrefab == null)
        {
            Debug.Log("SenorZorroPrefab is null");
            return false;
        }
        if (SpitGliderPrefab == null)
        {
            Debug.Log("SpitGliderPrefab is null");
            return false;
        }
        if (PoToadPrefab == null)
        {
            Debug.Log("PoToadPrefab is null");
            return false;
        }

        return true;
    }
    //bool 
    #endregion
}
