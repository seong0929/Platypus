using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;
using static Enums;
using System.IO;

public class SummonCardsPanel : MonoBehaviour
{
    const string DEFAULT_PATH = Directories.SummonData;

    [SerializeField] GameObject SummonCardPrefab;
    [SerializeField] GameObject Panel;

    public List<ESummon> PickableSummons;
    private List<SummonCard> cards = new List<SummonCard>();
    private Dictionary<ESummon, SummonCard> summonCardDictionary = new Dictionary<ESummon, SummonCard>();

    void Start()
    {
        
    }
    public void SetPickableSummons(List<ESummon> pickableSummons)
    {
        PickableSummons = pickableSummons;
    }
    public void CreateCards()
    {
        foreach (ESummon eSummon in PickableSummons)
        { 
            summonCardDictionary.Add(eSummon, CreateCard(eSummon));
        }
    }

    public SummonCard GetSummonCard(ESummon eSummon)
    {
        return summonCardDictionary[eSummon];
    }
    public SummonCard CreateCard(ESummon eSummon)
    {
        GameObject instantiatedPrefab = Instantiate(SummonCardPrefab, transform);
        instantiatedPrefab.transform.SetParent(Panel.transform);
        SummonCard summonCard = instantiatedPrefab.GetComponent<SummonCard>();

        summonCard.ESummon = eSummon;

        string targetPath = DEFAULT_PATH + eSummon.ToString();
        SummonScriptableObject summonScriptableObject = Resources.Load<SummonScriptableObject>(targetPath);
        if (summonScriptableObject != null)
        {
            EElement eElement = summonScriptableObject.ElementType;
            Sprite eSprite = summonScriptableObject.DefaultSprite;
            summonCard.ElementType = eElement;
            summonCard.SummonSprite = eSprite;
            int Price= summonScriptableObject.CrystalNum;
            summonCard.SetPrice(Price);
            summonCard.ChangeSprite();
        }
        else
        {
            Debug.LogError("Failed to load SummonScriptableObject from path: " + targetPath);
        }

        cards.Add(summonCard);
        return summonCard;
    }

}