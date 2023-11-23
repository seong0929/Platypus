using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using static Enums;


public class SelectPlayer : MonoBehaviour
{
    private GameManager gameManager;

    private List<Player> selected;
    private List<Player> roster;

    private int SelectedNum;

    [SerializeField]
    public GameObject rosterCardPrefab;
    [SerializeField]
    private GameObject RosterBox;
    [SerializeField]
    private GameObject SelectedBox;
    [SerializeField]
    private GameObject NextButton;

    private Text SelectedCounterTxt;

    // Start is called before the first frame update
    void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
        Team team = gameManager.GetUserTeam();

        roster = team.Roster;
        selected = new List<Player>();

        SelectedNum = gameManager.Round.PlayerNum;

        SetSelectedCounterTxt();
        SetRosterCard();
        SetSelectedCard();

        Button theButton = NextButton.GetComponent<Button>();
        theButton.onClick.AddListener(() => SendMatchManager());
    }

    void SetSelectedCounterTxt()
    {
        SelectedCounterTxt = SelectedBox.GetComponent<Text>();
        SelectedCounterTxt.text = "( " + selected.Count.ToString() + " / " + SelectedNum.ToString() + " )";
    }

    void SetRosterCard()
    {
        foreach (Transform child in RosterBox.transform)
        {
            Destroy(child.gameObject);
        }

        for (int i = 0; i <roster.Count; i++)
        {
            Vector3 vector = new Vector3(0, 200 -150 * i, 0);
            GameObject rosterCard = Instantiate(rosterCardPrefab, RosterBox.transform);
            rosterCard.transform.localPosition = vector; // Set the local position of the roster card

            Button cardButton = rosterCard.GetComponentInChildren<Button>();
            Text cardText = rosterCard.GetComponentInChildren<Text>();

            cardText.text = roster[i].Name;

            // Add an event listener to the button
            int index = i; // To capture the current index value

            if(cardButton != null)
            {
                cardButton.onClick.AddListener(() => OnRosterCardClicked(index));
            }
        }
    }

    private void OnRosterCardClicked(int index)
    {
        if (selected.Count >= SelectedNum) return;
        
        // Handle card click event based on the given index
        selected.Add(roster[index]);
        roster.Remove(roster[index]);
        Debug.Log("In button has been pushed");

        SetSelectedCounterTxt();
        SetRosterCard();
        SetSelectedCard();

    }

    void SetSelectedCard()
    {
        foreach (Transform child in SelectedBox.transform)
        {
            Destroy(child.gameObject);
        }

        for (int i = 0; i < selected.Count; i++)
        {
            Vector3 vector = new Vector3(0, 200 - 150 * i, 0);
            GameObject SelectedCard = Instantiate(rosterCardPrefab, SelectedBox.transform);
            SelectedCard.transform.localPosition = vector;

            Button cardButton = SelectedCard.GetComponentInChildren<Button>();
            Text cardText = SelectedCard.GetComponentInChildren<Text>();

            // Set the text of the card based on the list order
            cardText.text = selected[i].Name;

            // Add an event listener to the button
            int index = i; // To capture the current index value
            cardButton.onClick.AddListener(() => OnSelectedCardClicked(index));
        }
    }

    private void OnSelectedCardClicked(int index)
    {
        // Handle card click event based on the given index
        roster.Add(selected[index]);
        selected.Remove(selected[index]);

        SetSelectedCounterTxt();
        SetRosterCard();
        SetSelectedCard();

    }

    private void SendMatchManager()
    {
        gameManager.MakeRound();
        gameManager.Round.GroupA.SelectedPlayers = selected;
        List<Player> SelectedPlayers = gameManager.Round.GroupA.SelectedPlayers;
        Debug.Log("selected Players:" + SelectedPlayers[0].Name + ", " + SelectedPlayers[1].Name);
    }
}
