using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using static Enums; 

public class PlayerCard : MonoBehaviour
{
    // GameObject
    [Header ("GameObject")]
    [SerializeField] GameObject CardPanel;
    [SerializeField] GameObject PlayerPanel;
    [SerializeField] GameObject ATKPanel;
    [SerializeField] GameObject ATKText;
    [SerializeField] GameObject ATKNum;
    [SerializeField] GameObject DFSPanel;
    [SerializeField] GameObject DFSText;
    [SerializeField] GameObject DFSNum;
    [SerializeField] GameObject LVPanel;
    [SerializeField] GameObject LVText;
    [SerializeField] GameObject LVNum;
    [SerializeField] GameObject ProficientPanel0;
    [SerializeField] GameObject ProficientPanel1;
    [SerializeField] GameObject ProficientPanel2;

    // Images and is TeamA or TeamB?
    [Header("Sprite")]
    public Sprite CardPanelSprite;
    public Sprite PlayerPanelSprite;
    public Sprite ATKPanelSprite;
    public Sprite DFSPanelSprite;
    public Sprite LVPanelSprite;
    public Sprite ProficientPanelSprite0;
    public Sprite ProficientPanelSprite1;
    public Sprite ProficientPanelSprite2;

    [Header("isTeamA?")]
    public bool bA;

    // Image & int Component
    private Image cardPanelImage;
    private Image playerPanelImage;
    private Image playerImage;
    private Image atkPanelImage;
    private Color atkColor;
    private int atkNum;
    private Image dfsPanelImage;
    private Color dfsColor;
    private int dfsNum;
    private Image lvPanelImage;
    private Color  lvColor;
    private int lvNum;
    private Image proficientPanelImage0;
    private Image proficientPanelImage1;
    private Image proficientPanelImage2;
    private List<Image> proficientSummonImages;


    // Start is called before the first frame update
    void Start()
    {
        //Set Panels Images
        fetchComponents();
        UpdatePanels();

        //Set Font's color

    }

    private void fetchComponents()
    {
        cardPanelImage = CardPanel.GetComponent<Image>();
        playerPanelImage = PlayerPanel.GetComponent<Image>();
        atkPanelImage = ATKPanel.GetComponent<Image>();
        dfsPanelImage = DFSPanel.GetComponent<Image>();
        lvPanelImage = LVPanel.GetComponent<Image>();
        proficientPanelImage0 = ProficientPanel0.GetComponent<Image>();
        proficientPanelImage1 = ProficientPanel1.GetComponent<Image>();
        proficientPanelImage2 = ProficientPanel2.GetComponent<Image>();
    }
    public void UpdatePanels()
    {
        if(bA == false)
        {
            cardPanelImage.sprite = CardPanelSprite;
            playerPanelImage.sprite = PlayerPanelSprite;
            atkPanelImage.sprite = ATKPanelSprite;
            dfsPanelImage.sprite = DFSPanelSprite;
            lvPanelImage.sprite = LVPanelSprite;
            proficientPanelImage0.sprite = ProficientPanelSprite0;
            proficientPanelImage1.sprite = ProficientPanelSprite1;
            proficientPanelImage2.sprite = ProficientPanelSprite2;
        }
    }
    public void setPlayerImage(Image playerImage)
    {

    }
    public void setATK(int atk)
    {

    }
    public void setDFS(int dfs)
    {

    }
    public void setLV(int lv)
    {

    }
    public void SetProficient(List<ESummon> proficientList)
    {

    }
}
