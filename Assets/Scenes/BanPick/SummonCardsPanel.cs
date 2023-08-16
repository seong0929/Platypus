using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;
using static Enums;


public class SummonCardsPanel : MonoBehaviour
{
    [SerializeField] GameObject SummonCardPrefab;
    [SerializeField] GameObject Panel;

    public List<Enums.ESummon> PickableSummons;
    private List<SummonCard> cards = new List<SummonCard>();

    void Start()
    {
        
    }
    public void SetPickableSummons(List<Enums.ESummon> pickableSummons)
    {
        PickableSummons = pickableSummons;
    }
    public void CreateCards()
    {
        foreach(Enums.ESummon eSummon in PickableSummons)
        {
            GameObject instantiatedPrefab = Instantiate(SummonCardPrefab, transform);
            instantiatedPrefab.transform.SetParent(Panel.transform);
            SummonCard summonCard = instantiatedPrefab.GetComponent<SummonCard>();

            cards.Add(summonCard);
        }
    }

}
