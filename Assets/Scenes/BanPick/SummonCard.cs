using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;
using static Enums;

public class SummonCard : MonoBehaviour
{
    public Enums.EStage StageType;

    [Header("GameObject")]
    [SerializeField] GameObject Panel;
    [SerializeField] GameObject Button;
    [SerializeField] GameObject Summon;

    [Header("Sprite")]
    public Sprite SepiaPanel;
    public Sprite AquaPanel;
    public Sprite EmerladPanel;
    public Sprite SepiaButton;
    public Sprite AquaButton;
    public Sprite EmerladButton;

    private Image PanelImage;
    private Image ButtonImage;
    private Image SummonImage;

    void Awake()
    {
        //Set Panels Images
        fetchComponents();
        ChangeSprite();
    }

    private void fetchComponents()
    {
        PanelImage = Panel.GetComponent<Image>();
        ButtonImage = Button.GetComponent<Image>();
        SummonImage = Summon.GetComponent<Image>();
    }

    private void ChangeSprite()
    {
        switch(StageType)
        {
            case Enums.EStage.Emerald:
                PanelImage.sprite = EmerladPanel;
                ButtonImage.sprite = EmerladButton;
                break;
            case Enums.EStage.Aqua:
                PanelImage.sprite = AquaPanel;
                ButtonImage.sprite = AquaButton;
                break;
            case Enums.EStage.Sepia:
                PanelImage.sprite = SepiaPanel;
                ButtonImage.sprite = SepiaButton;
                break;
            default:
                PanelImage.sprite = EmerladPanel;
                ButtonImage.sprite = EmerladButton;
                break;
        }

        
    }
}
