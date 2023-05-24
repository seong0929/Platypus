using UnityEngine;
using UnityEngine.UI;


public class ScoutCard : MonoBehaviour
{
    private GameManager gameManager;

    private int nth = 0;

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
        gameManager.User.Team.ScoutPlayer(thePlayer);
        Debug.Log("Scout button has been clicked");
    }

}