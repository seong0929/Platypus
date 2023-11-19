using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;
using static Enums; 

public class PlayerCard : MonoBehaviour
{
    [Header("Stat Control")]
    public int ATKnum;
    public int DFSnum;
    public int LVnum;

    public string PlayerName;

    // GameObject
    [Header ("GameObject")]
    [SerializeField] GameObject CardPanel;
    [SerializeField] GameObject PlayerPanel;
    [SerializeField] GameObject ATKPanel;
    [SerializeField] TextMeshProUGUI ATKText;
    [SerializeField] TextMeshProUGUI ATKNum;
    [SerializeField] GameObject DFSPanel;
    [SerializeField] TextMeshProUGUI DFSText;
    [SerializeField] TextMeshProUGUI DFSNum;
    [SerializeField] GameObject LVPanel;
    [SerializeField] TextMeshProUGUI LVText;
    [SerializeField] TextMeshProUGUI LVNum;
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

    private Color textColor0 = new Color(1f, 1f, 0.9f);
    private Color textColor1 = new Color(0.6f, 0.2f, 0.2f);
    private Color statColor0 = new Color(1f, 0.56f, 0.4f);
    private Color statColor1 = new Color(0.6f, 0.2f, 0.2f);
    private Color statColor2 = new Color(0.23f, 0.63f, 0.43f);

    // Start is called before the first frame update
    void Awake()
    {
        //Set Panels Images
        fetchComponents();
        SetTeam(bA);

        SetStats();
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
    public void SetTeam(bool isA)
    {
        bA = isA;
        chnageSprite();
    }
    private void chnageSprite()
    {
        // Update Panels if it is team B's cards.
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

            //Update Text's color if it is the team B's cards.
            ATKText.color = textColor0;
            DFSText.color = textColor1;
            LVText.color = textColor0;

            ATKNum.color = statColor0;
            DFSNum.color = statColor1;
            LVNum.color = statColor2;
        }
    }
    public void setPlayerImage(Image playerImage)
    {

    }
    private void SetStats()
    {
        ATKNum.text = ATKnum.ToString();
        DFSNum.text = DFSnum.ToString();
        LVNum.text = LVnum.ToString();
    }
    public void SetATK(int atk)
    {
        ATKnum = atk;
        ATKNum.text = ATKnum.ToString();
    }
    public void SetDFS(int dfs)
    {
        DFSnum = dfs;
        DFSNum.text = DFSnum.ToString();

    }
    public void SetLV(int lv)
    {
        LVnum = lv;
        LVNum.text = LVnum.ToString();

    }

    public void SetName(string name)
    {
        PlayerName = name;
    }
    public void SetProficient(List<ESummon> proficientList)
    {

    }
}
