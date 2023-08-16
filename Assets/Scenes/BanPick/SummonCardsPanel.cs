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

    private string _defaultPath = "SummonsData/";
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

            string targetPath = _defaultPath + eSummon.ToString();
            SummonScriptableObject summonScriptableObject = Resources.Load<SummonScriptableObject>(targetPath);
            if (summonScriptableObject != null)
            {
                Enums.EElement eElement = summonScriptableObject.ElementType;
                Sprite eSprite = summonScriptableObject.DefaultSprite;
                summonCard.ElementType = eElement;
                summonCard.SummonSprite = eSprite;
                summonCard.ChangeSprite();
            }
            else
            {
                Debug.LogError("Failed to load SummonScriptableObject from path: " + targetPath);
            }

            cards.Add(summonCard);
        }
    }

}