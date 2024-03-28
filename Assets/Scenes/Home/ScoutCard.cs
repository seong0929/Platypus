using Assets.PixelHeroes.Scripts.CharacterScripts;
using TMPro;
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

    [SerializeField]
    private GameObject Character;

    [SerializeField]
    private GameObject PlayerCharacterPanel;

    private Player thePlayer;

    public ScoutCard(Player player)
    {
        SetPlayer(player);

        // Access the data from GameManager and update the UI text
        Name.text = thePlayer.Name;
        Level.text = "Level:" + thePlayer.Level.ToString();
        if (PlayerCharacterPanel!= null)// && Character.GetComponent<CharacterBuilder>() != null)
        {
            PlayerCharacterPanel.GetComponent<PlayerCharacterPanel>().SetCharacterPanel(thePlayer.Appearance);
        }

        ScoutButton.GetComponent<Button>().onClick.AddListener(Scout);
    }
    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        if (gameManager == null)
        {
            Debug.Log("gameManager is null");
        }
    }


    public void SetPlayer(Player player)
    {
        thePlayer = player;
        UpdateUI();
    }

    private void UpdateUI()
    {
        Name.text = thePlayer.Name;
        Level.text = "Level:" + thePlayer.Level.ToString();
        if (PlayerCharacterPanel!= null)// && Character.GetComponent<CharacterBuilder>() != null)
        {
            PlayerCharacterPanel.GetComponent<PlayerCharacterPanel>().SetCharacterPanel(thePlayer.Appearance);
        }

        ScoutButton.GetComponent<Button>().onClick.AddListener(Scout);
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

        // Mark the player as scouted on the scout button
        ScoutButton.GetComponent<Button>().interactable = false;
        ScoutButton.GetComponentInChildren<Text>().text = "Scouted";
        ScoutButton.GetComponent<Image>().color = Color.gray; // change the color of the button


        /////////////// NEED FIX LATER /////////////
        gameManager.GetUserTeam().AddPlayerToRoster(thePlayer);
        Debug.Log("Roster player num: " + gameManager.GetUserTeam().Roster.Count);
    }

}