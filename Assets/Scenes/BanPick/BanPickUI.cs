using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using static Enums;

public class BanPickUI : MonoBehaviour
{
    private GameManager gameManager;
    private BanPickRunner banPickRunner;

    public List<Enums.ESummon> PickableSummons;
    private List<Player> TeamA;
    private List<Player> TeamB;

    private Scroll _scroll;

    [SerializeField] public GameObject TeamPanelA;
    [SerializeField] public GameObject TeamPanelB;
    [SerializeField] GameObject SummonCardsPanelObject;
    [SerializeField] GameObject ScrollObject;

    private Round round;
    public static UnityEvent UpdateBanPickUIs = new UnityEvent();
    public static UnityEvent<ETeamSide, Image> UpdateTeamPicked = new UnityEvent<ETeamSide, Image>();

    // Start is called before the first frame update
    void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();

        round = gameManager.Round;
        TeamA = round.GroupA.SelectedPlayers;
        TeamB = round.GroupB.SelectedPlayers;
        PickableSummons = round.AvaiableSummons;

        banPickRunner = new BanPickRunner(round);
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

        initializeEvents();
        UpdateBanPickUI();
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
        UpdateBanPickUI();
    }
    private void InitializeTeamPanel(GameObject panelGameObject, List<Player> teamMembers, bool isTeamA)
    {
        TeamPanel panel = panelGameObject.GetComponent<TeamPanel>();
        panel.SetTeam(isTeamA);

        List<int> atkList = new List<int>();
        List<int> dfsList = new List<int>();
        List<int> lvList = new List<int>();
        List<string> nameList = new List<string>();
        List<CharacterAppearance> characterAppearanceList = new List<CharacterAppearance>();

        foreach (Player member in teamMembers)
        {
            atkList.Add(member.Attack);
            dfsList.Add(member.Defense);
            lvList.Add(member.Level);
            nameList.Add(member.Name);
            characterAppearanceList.Add(member.Appearance);
        }

        panel.SetATKs(atkList);
        panel.SetDFSs(dfsList);
        panel.SetLVs(lvList);
        panel.SetPlayerNames(nameList);
        panel.SetPlayerCharacter(characterAppearanceList);

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
        round.GroupA.SelectedSummon = teamASummons;
        round.GroupB.SelectedSummon = teamBSummons;
    }
    private void InitializeScroll()
    {
        _scroll = ScrollObject.GetComponent<Scroll>();
    }

    private void UpdateScroll()
    {
        RoundStatePair roundStatePair = banPickRunner.GetCurrentRoundState();
        
        ETeamSide turn = roundStatePair.Turn;
        EBanPickState eBanPickState = roundStatePair.State;

        if (turn == ETeamSide.TeamA)
        {
            _scroll.TurnBlueScroll();
        }
        else
        {
            _scroll.TurnRedScroll();
        }

        string scrollText = "None";
        if (eBanPickState == EBanPickState.Ban)
        {
            scrollText = "Ban a Summon.";
        }
        else if (eBanPickState == EBanPickState.Pick)
        {
            scrollText = "Select a Summon.";
        }
        if(eBanPickState == EBanPickState.Done)
        {
            scrollText = "Done.";
        }
        _scroll.SetText(scrollText);
    }
    public void UpdateBanPickUI()
    {
        UpdateScroll();
        UpdateSummonCards();
    }
    private void UpdateSummonCards()
    {
        SummonCardsPanel summonCardsPanel = SummonCardsPanelObject.GetComponent<SummonCardsPanel>();
        
        List<ESummon> PickableSummons = banPickRunner.GetPickableSummons();
        foreach (ESummon eSummon in PickableSummons)
        {
            SummonCard summonCard = summonCardsPanel.GetSummonCard(eSummon);
            summonCard.CardSelectable();
        }

        List<ESummon> BannedSummonsTeamA = banPickRunner.BannedSummonsTeamA;
        List<ESummon> BannedSummonsTeamB = banPickRunner.BannedSummonsTeamB;
        foreach (ESummon eSummon in BannedSummonsTeamA)
        {
            SummonCard summonCard = summonCardsPanel.GetSummonCard(eSummon);
            summonCard.CardBanned();
        }
        foreach (ESummon eSummon in BannedSummonsTeamB)
        {
            SummonCard summonCard = summonCardsPanel.GetSummonCard(eSummon);
            summonCard.CardBanned();
        }

        List<ESummon> PickedSummonsTeamA = banPickRunner.PickedSummonsTeamA;
        List<ESummon> PickedSummonsTeamB = banPickRunner.PickedSummonsTeamB;
        foreach (ESummon eSummon in PickedSummonsTeamA)
        {
            SummonCard summonCard = summonCardsPanel.GetSummonCard(eSummon);
            summonCard.CardSelected(ETeamSide.TeamA);
        }
        foreach (ESummon eSummon in PickedSummonsTeamB)
        {
            SummonCard summonCard = summonCardsPanel.GetSummonCard(eSummon);
            summonCard.CardSelected(ETeamSide.TeamB);
        }
    }

    public void AddTeamPickedSummon(ETeamSide eTeamSide, Image image)
    {
        TeamPanel teamPanel = new TeamPanel();
        GameObject teamPanelObject = (eTeamSide == ETeamSide.TeamA) ? TeamPanelA : TeamPanelB;
        teamPanel = teamPanelObject.GetComponent<TeamPanel>();
        teamPanel.AddPickedSummonImage(image);
    }

    private void initializeEvents()
    {
        UpdateBanPickUIs = new UnityEvent();
        UpdateBanPickUIs.AddListener(() => UpdateBanPickUI());
        UpdateTeamPicked = new UnityEvent<ETeamSide, Image>();
        UpdateTeamPicked.AddListener((ETeamSide eTeamSide, Image image) => AddTeamPickedSummon(eTeamSide, image));
    }

}
