using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BanPickUI : MonoBehaviour
{
    private GameManager gameManager;
    private MatchManager matchManager;

    public List<Enums.ESummon> PickableSummons;
    private List<Player> TeamA;
    private List<Player> TeamB;


    [SerializeField]
    GameObject TeamPanelA;
    [SerializeField]
    GameObject TeamPanelB;
    [SerializeField]
    GameObject SummonCardsPanelObject;

    // Start is called before the first frame update
    void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
        matchManager = gameManager.MatchManager;
        TeamA = matchManager.GroupA.SelectedPlayers;
        TeamB = matchManager.GroupB.SelectedPlayers;
        PickableSummons = matchManager.PickableSummons;
    }

    void Start()
    {
        InitializeTeamPanel(TeamPanelA, TeamA, true);
        InitializeTeamPanel(TeamPanelB, TeamB, false);
        InitializeSummonCardsPanel();

        List<Enums.ESummon> sumons = new List<Enums.ESummon>();
        sumons.Add(Enums.ESummon.SenorZorro);
        sumons.Add(Enums.ESummon.SenorZorro);

        setSummons(sumons, sumons);
    }

    private void InitializeTeamPanel(GameObject panelGameObject, List<Player> teamMembers, bool isTeamA)
    {
        TeamPanel panel = panelGameObject.GetComponent<TeamPanel>();
        panel.SetTeam(isTeamA);

        List<int> atkList = new List<int>();
        List<int> dfsList = new List<int>();
        List<int> lvList = new List<int>();

        foreach (Player member in teamMembers)
        {
            atkList.Add(member.Attack);
            dfsList.Add(member.Defense);
            lvList.Add(member.Level);
        }

        panel.SetATKs(atkList);
        panel.SetDFSs(dfsList);
        panel.SetLVs(lvList);

        panel.CreateCards(teamMembers.Count);
    }
    private void InitializeSummonCardsPanel()
    {
        SummonCardsPanel summonCardsPanel = SummonCardsPanelObject.GetComponent<SummonCardsPanel>();
        summonCardsPanel.SetPickableSummons(PickableSummons);

        summonCardsPanel.CreateCards();
    }
    private void setSummons(List<Enums.ESummon> teamASummons, List<Enums.ESummon> teamBSummons)
    {
        matchManager.GroupA.SelectedSummon = teamASummons;
        matchManager.GroupB.SelectedSummon = teamBSummons;
    }
}
