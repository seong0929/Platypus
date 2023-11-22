using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BanPickUI : MonoBehaviour
{
    private GameManager gameManager;

    public List<Enums.ESummon> PickableSummons;
    private List<Player> TeamA;
    private List<Player> TeamB;

    private Scroll _scroll;

    [SerializeField] GameObject TeamPanelA;
    [SerializeField] GameObject TeamPanelB;
    [SerializeField] GameObject SummonCardsPanelObject;
    [SerializeField] GameObject ScrollObject;

    private Round match;

    // Start is called before the first frame update
    void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
        match = gameManager.Round;
        TeamA = match.GroupA.SelectedPlayers;
        TeamB = match.GroupB.SelectedPlayers;
        PickableSummons = match.AvaiableSummons;
    }

    void Start()
    {
        InitializeTeamPanel(TeamPanelA, TeamA, true);
        InitializeTeamPanel(TeamPanelB, TeamB, false);
        InitializeSummonCardsPanel();
        InitializeScroll();

        List<Enums.ESummon> sumons = new List<Enums.ESummon>();
        sumons.Add(Enums.ESummon.SenorZorro);
        sumons.Add(Enums.ESummon.SenorZorro);

        SetSummons(sumons, sumons);
    }
    private void SetTurn(Enums.ETeamSide eturn, Enums.EBanPickState eBanPickState)
    {
        if(eturn == Enums.ETeamSide.TeamA)
        {
            _scroll.TurnBlueScroll();
        }
        else
        {
            _scroll.TurnBlueScroll();
        }

        string scrollText = "None";
        if(eBanPickState == Enums.EBanPickState.Ban)
        {
            scrollText = "Ban a Summon.";
        }
        else if(eBanPickState == Enums.EBanPickState.Pick)
        {
            scrollText = "Select a Summon.";
        }

        _scroll.SetText(scrollText);
    }
    private void InitializeTeamPanel(GameObject panelGameObject, List<Player> teamMembers, bool isTeamA)
    {
        TeamPanel panel = panelGameObject.GetComponent<TeamPanel>();
        panel.SetTeam(isTeamA);

        List<int> atkList = new List<int>();
        List<int> dfsList = new List<int>();
        List<int> lvList = new List<int>();
        List<string> nameList = new List<string>();

        foreach (Player member in teamMembers)
        {
            atkList.Add(member.Attack);
            dfsList.Add(member.Defense);
            lvList.Add(member.Level);
            nameList.Add(member.Name);
        }

        panel.SetATKs(atkList);
        panel.SetDFSs(dfsList);
        panel.SetLVs(lvList);
        panel.SetPlayerNames(nameList);

        panel.CreateCards(teamMembers.Count);
    }
    private void InitializeSummonCardsPanel()
    {
        SummonCardsPanel summonCardsPanel = SummonCardsPanelObject.GetComponent<SummonCardsPanel>();
        summonCardsPanel.SetPickableSummons(PickableSummons);

        summonCardsPanel.CreateCards();
    }
    private void SetSummons(List<Enums.ESummon> teamASummons, List<Enums.ESummon> teamBSummons)
    {
        match.GroupA.SelectedSummon = teamASummons;
        match.GroupB.SelectedSummon = teamBSummons;
    }
    private void InitializeScroll()
    {
        _scroll = ScrollObject.GetComponent<Scroll>();
    }
}
