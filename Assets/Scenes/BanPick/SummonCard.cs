using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;
using static Enums;

public class SummonCard : MonoBehaviour
{
    public Enums.EElement ElementType;
    public Enums.ESummon ESummon;
    [Header("GameObject")]
    [SerializeField] GameObject Panel;
    [SerializeField] GameObject ButtonObject;
    [SerializeField] GameObject Summon;
    [SerializeField] GameObject Banner;

    [Header("Sprite")]
    public Sprite SepiaPanel;
    public Sprite AquaPanel;
    public Sprite EmerladPanel;
    public Sprite SepiaButton;
    public Sprite AquaButton;
    public Sprite EmerladButton;
    public Sprite SummonSprite;
    [SerializeField] Sprite Banned;
    [SerializeField] Sprite Picked;

    [SerializeField] Color teamASelectedColor;
    [SerializeField] Color teamBSelectedColor;

    private Image PanelImage;
    private Image ButtonImage;
    private Image SummonImage;
    private Image BannerImage;

    private Button _selectButton;

    void Awake()
    {
        //Set Panels Images
        fetchComponents();
        ChangeSprite();

        _selectButton.onClick.AddListener(() => SelectButtonClicked());
    }

    public void CardSelected(Enums.ETeam eTeam)
    {
        _selectButton.interactable = false;
        Banner.SetActive(true);

        BannerImage.sprite = Picked;

        Color bannerColor;
        if(eTeam == Enums.ETeam.TeamA)
        {
            bannerColor = teamASelectedColor;
        }
        else
        {
            bannerColor = teamBSelectedColor;
        }
        BannerImage.color = bannerColor;
    }
    public void CardBanned()
    {
        _selectButton.interactable = false;
        Banner.SetActive(true);

        BannerImage.sprite = Banned;
    }

    private void fetchComponents()
    {
        PanelImage = Panel.GetComponent<Image>();
        ButtonImage = ButtonObject.GetComponent<Image>();
        SummonImage = Summon.GetComponent<Image>();
        BannerImage = Banner.GetComponent<Image>();

        _selectButton = ButtonObject.GetComponent<Button>();
    }

    public void ChangeSprite()
    {
        switch(ElementType)
        {
            case Enums.EElement.Emerald:
                PanelImage.sprite = EmerladPanel;
                ButtonImage.sprite = EmerladButton;
                break;
            case Enums.EElement.Aqua:
                PanelImage.sprite = AquaPanel;
                ButtonImage.sprite = AquaButton;
                break;
            case Enums.EElement.Sepia:
                PanelImage.sprite = SepiaPanel;
                ButtonImage.sprite = SepiaButton;
                break;
            default:
                PanelImage.sprite = EmerladPanel;
                ButtonImage.sprite = EmerladButton;
                break;
        }

        if(SummonSprite != null)
        {
            SummonImage.sprite = SummonSprite;
        }
    }

    private void SelectButtonClicked()
    {
//        MatchManager.RecieveBanPickSummon.Invoke(ESummon);
    }
}
