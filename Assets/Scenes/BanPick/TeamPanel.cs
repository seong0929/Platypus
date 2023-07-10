using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class TeamPanel : MonoBehaviour
{
    [SerializeField]
    GameObject PlayerCardPrefab;

    public int CardNum = 3;
    private bool bA;
    private List<GameObject> cards = new List<GameObject>();
    private List<int> atkList = new List<int>();
    private List<int> dfsList = new List<int>();
    private List<int> lvList = new List<int>();

    // Start is called before the first frame update
    void Start()
    {
        // Get Stats of Players + team
        for (int i = 0; i < CardNum; i++)
        {
            atkList.Add(i);
            dfsList.Add(i+10);
            lvList.Add(i+100);
        }
        bA = false;

        destroyChildObjects();
        // Create Cards
        createCards();
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
    private void createCards()
    {
        for (int i = 0; i < CardNum; i++)
        {
            GameObject instantiatedPrefab = Instantiate(PlayerCardPrefab, transform);
            PlayerCard playerCard = instantiatedPrefab.GetComponent<PlayerCard>();


            playerCard.SetATK(atkList[i]);
            playerCard.SetDFS(dfsList[i]);
            playerCard.SetLV(lvList[i]);
            playerCard.SetTeam(bA);

            cards.Add(instantiatedPrefab);
        }
    }

    }
