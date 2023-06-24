using UnityEngine;
using UnityEngine.UI;

public class ScoutCard : MonoBehaviour
{
    private GameManager gameManager;

    [SerializeField] int _nth;
    [SerializeField] Text _name;
    [SerializeField] Text _level;
    [SerializeField] GameObject _scoutButton;

    private Player _thePlayer;

    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        _thePlayer = gameManager.TeamManager.FAs.Players[_nth];

        if (gameManager != null)
        {
            // Access the data from GameManager and update the UI text
            _name.text = _thePlayer.Name;
            _level.text = "Level:" + _thePlayer.Level.ToString();

            _scoutButton.GetComponent<Button>().onClick.AddListener(Scout);
        }
        else
        {
            _name.text = "error";
            _level.text = "Level:" + "error";

        }
    }

    public void Scout()
    {

        gameManager.User.Team.ScoutPlayer(_thePlayer);
        Debug.Log("Scout"+_thePlayer.Name);

        /////////////// NEED FIX LATER /////////////
        gameManager.User.Team.AddPlayerToRoster(_thePlayer);
        Debug.Log("Roster player num: " + gameManager.User.Team.Roster.Count);
    }

}