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
        TeamPanel PanelA = TeamPanelA.GetComponent<TeamPanel>();
        PanelA.SetTeam(true);
        TeamPanel PanelB = TeamPanelB.GetComponent<TeamPanel>();
        PanelB.SetTeam(false);
    }
}
