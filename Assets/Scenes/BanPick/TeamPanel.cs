using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class TeamPanel : MonoBehaviour
{
    [SerializeField]
    GameObject PlayerCardPrefab;

    public int CardNum = 3;
    private bool bA;
    private List<PlayerCard> cards = new List<PlayerCard>();
    private List<int> atkList = new List<int>();
    private List<int> dfsList = new List<int>();
    private List<int> lvList = new List<int>();
    private List<string> nameList = new List<string>();

    // Start is called before the first frame update
    void Start()
    {

        destroyChildObjects();
        // Create Cards
        //createCards();
    }
    


    public void SetATKs(List<int> atks)
    {
        atkList = atks;
    }
    public void SetDFSs(List<int> dfss)
    {
        dfsList = dfss;
    }
    public void SetLVs(List<int> lvs)
    {
        lvList = lvs;
    }
    public void SetTeam(bool team)
    {
        bA = team;

        int childCount = cards.Count;
        for (int i = 0; i < childCount ; i++)
        {
            PlayerCard playerCard = cards[i];

            playerCard.SetTeam(bA);
        }
    }
    public void SetPlayerNames(List<string> names)
    {
        nameList = names;
    }

    private void destroyChildObjects()
    {
        int childCount = transform.childCount;
        for (int i = childCount - 1; i >= 0; i--)
        {
            GameObject child = transform.GetChild(i).gameObject;
            Destroy(child);
        }
    }
    public void CreateCards(int cardNum)
    {
        CardNum = cardNum;
        for (int i = 0; i < CardNum; i++)
        {
            GameObject instantiatedPrefab = Instantiate(PlayerCardPrefab, transform);
            PlayerCard playerCard = instantiatedPrefab.GetComponent<PlayerCard>();


            playerCard.SetATK(atkList[i]);
            playerCard.SetDFS(dfsList[i]);
            playerCard.SetLV(lvList[i]);
            playerCard.SetTeam(bA);
            playerCard.SetName(nameList[i]);

            cards.Add(playerCard);
        }
    }

    }
