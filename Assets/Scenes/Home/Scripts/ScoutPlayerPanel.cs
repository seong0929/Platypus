using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoutPlayerPanel : MonoBehaviour
{
    private GameManager gameManager;
    private List<Player> scoutablePlayers;
    private List<GameObject> scoutCards;

    [SerializeField]
    private GameObject scoutCardPrefab;

    // get the list of FA Players
    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        if (gameManager == null)
        {
            Debug.Log("gameManager is null");
        }
        scoutablePlayers = gameManager.TeamManager.FAs.Players;
        scoutCards = new List<GameObject>();
        for (int i = 0; i < scoutablePlayers.Count; i++)
        {
            GameObject scoutCard = Instantiate(scoutCardPrefab, transform);
            scoutCard.GetComponent<ScoutCard>().SetPlayer(scoutablePlayers[i]);
            scoutCards.Add(scoutCard);
        }
    }
// Assign ScoutCard to each of the FA Players

// initialize the ScoutCard

}
