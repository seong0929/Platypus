using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BanPickUI : MonoBehaviour
{
    private GameManager gameManager;

    private List<Player> TeamA;
    private List<Player> TeamB;

    [SerializeField]
    GameObject TeamPanelA;
    [SerializeField]
    GameObject TeamPanelB;

    // Start is called before the first frame update
    void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
        TeamA = gameManager.MatchManager.GroupA.SelectedPlayers;
        TeamB = gameManager.MatchManager.GroupB.SelectedPlayers;
    }

    void Start()
    {
        InitializeTeamPanel(TeamPanelA, TeamA, true);
        InitializeTeamPanel(TeamPanelB, TeamB, false);
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

}
