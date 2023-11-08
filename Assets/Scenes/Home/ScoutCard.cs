using UnityEngine;
using UnityEngine.UI;


public class ScoutCard : MonoBehaviour
{
    private GameManager gameManager;

    [SerializeField]
    private int nth;

    [SerializeField]
    private Text Name;
    [SerializeField]
    private Text Level;

    [SerializeField]
    private GameObject ScoutButton;

    private Player thePlayer;

    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        if(gameManager == null)
        {
            Debug.Log("gameManager is null");
        }
        thePlayer = gameManager.TeamManager.FAs.Players[nth];

        if (gameManager != null)
        {
            // Access the data from GameManager and update the UI text
            Name.text = thePlayer.Name;
            Level.text = "Level:" + thePlayer.Level.ToString();

            ScoutButton.GetComponent<Button>().onClick.AddListener(Scout);
        }
        else
        {
            Name.text = "error";
            Level.text = "Level:" + "error";

        }
    }

    public void Scout()
    {
        if (gameManager == null)
        {
            Debug.Log("gameManager is null");
        }
        if (thePlayer == null) 
        {
        Debug.Log("thePlayer is null");
        }
        if (gameManager.GetUserTeam() == null)
        {
            Debug.Log("gameManager.GetUserTeam() is null");
        }
        gameManager.GetUserTeam().ScoutPlayer(thePlayer);
        Debug.Log("Scout"+thePlayer.Name);

        /////////////// NEED FIX LATER /////////////
        gameManager.GetUserTeam().AddPlayerToRoster(thePlayer);
        Debug.Log("Roster player num: " + gameManager.GetUserTeam().Roster.Count);
    }

}