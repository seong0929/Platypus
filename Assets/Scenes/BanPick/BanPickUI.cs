using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BanPickUI : MonoBehaviour
{
    private GameManager gameManager;

    private List<Player> TeamA;
    private List<Player> TeamB;

    [SerializeField]
    public GameObject PlayerCardPrefab;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        TeamA = gameManager.GroupA.SelectedPlayers;
        Team team = gameManager.User.Team;

        TeamA = team.Roster;

    }


}
