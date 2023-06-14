using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class SelectPlayer : MonoBehaviour
{
    private GameManager _gameManager;

    private List<Player> _selected;
    private List<Player> _roster;

    private int _selectedNum;

    [SerializeField] GameObject _rosterCardPrefab;
    [SerializeField] GameObject _rosterBox;
    [SerializeField] GameObject _selectedBox;
    [SerializeField] GameObject _nextButton;

    private Text _selectedCounterTxt;

    private void Start()
    {
        _gameManager = FindObjectOfType<GameManager>();
        Team team = _gameManager.User.Team;

        _roster = team.Roster;
        _selected = new List<Player>();

        _selectedNum = _gameManager.MatchManager.PlayerNum;

        SetSelectedCounterTxt();
        SetRosterCard();
        SetSelectedCard();

        Button theButton = _nextButton.GetComponent<Button>();
        theButton.onClick.AddListener(() => SendMatchManager());
    }
    private void SetSelectedCounterTxt()
    {
        _selectedCounterTxt = _selectedBox.GetComponent<Text>();
        _selectedCounterTxt.text = "( " + _selected.Count.ToString() + " / " + _selectedNum.ToString() + " )";
    }
    private void SetRosterCard()
    {
        foreach (Transform child in _rosterBox.transform)
        {
            Destroy(child.gameObject);
        }

        for (int i = 0; i <_roster.Count; i++)
        {
            Vector3 vector = new Vector3(0, 200 -150 * i, 0);
            GameObject rosterCard = Instantiate(_rosterCardPrefab, _rosterBox.transform);
            rosterCard.transform.localPosition = vector; // Set the local position of the roster card

            Button cardButton = rosterCard.GetComponentInChildren<Button>();
            Text cardText = rosterCard.GetComponentInChildren<Text>();

            cardText.text = _roster[i].Name;

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
        if (_selected.Count >= _selectedNum) return;
        
        // Handle card click event based on the given index
        _selected.Add(_roster[index]);
        _roster.Remove(_roster[index]);
        Debug.Log("In button has been pushed");

        SetSelectedCounterTxt();
        SetRosterCard();
        SetSelectedCard();

    }
    void SetSelectedCard()
    {
        foreach (Transform child in _selectedBox.transform)
        {
            Destroy(child.gameObject);
        }

        for (int i = 0; i < _selected.Count; i++)
        {
            Vector3 vector = new Vector3(0, 200 - 150 * i, 0);
            GameObject SelectedCard = Instantiate(_rosterCardPrefab, _selectedBox.transform);
            SelectedCard.transform.localPosition = vector;

            Button cardButton = SelectedCard.GetComponentInChildren<Button>();
            Text cardText = SelectedCard.GetComponentInChildren<Text>();

            // Set the text of the card based on the list order
            cardText.text = _selected[i].Name;

            // Add an event listener to the button
            int index = i; // To capture the current index value
            cardButton.onClick.AddListener(() => OnSelectedCardClicked(index));
        }
    }
    private void OnSelectedCardClicked(int index)
    {
        // Handle card click event based on the given index
        _roster.Add(_selected[index]);
        _selected.Remove(_selected[index]);

        SetSelectedCounterTxt();
        SetRosterCard();
        SetSelectedCard();

    }
    private void SendMatchManager()
    {
        _gameManager.MatchManager.GroupA.SelectedPlayers = _selected;
        List<Player> SelectedPlayers = _gameManager.MatchManager.GroupA.SelectedPlayers;
        Debug.Log("selected Players:" + SelectedPlayers[0].Name + ", " + SelectedPlayers[1].Name);
    }
}
