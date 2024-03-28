using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using static Enums;
using static Constants;
/*
 * BanPickRunner is a class that manages the ban pick process.
 * - It has a list of RoundStatePair that contains the order of the ban pick process.
 * - It has a list of ESummon that contains the pickable summons.
 */
public class BanPickRunner
{

    public Round Round;
    public RoundStatePair CurrentRoundState => RoundStates[RoundStateIndex];
    public List<RoundStatePair> RoundStates;
    public int RoundStateIndex = 0;
    public int CrystalMAXNum;

    public int PlayerNum;
    public List<ESummon> AvaiableSummons; // All possible the summons that can be picked in this round.
    
    public List<ESummon> PickableSummonsA; // pickable summons in this state.
    public List<ESummon> PickableSummonsB; // pickable summons in this state.

    public List<ESummon> BannedSummonsTeamA;
    public List<ESummon> BannedSummonsTeamB;

    public List<ESummon> PickedSummonsTeamA;
    public List<ESummon> PickedSummonsTeamB;

    public int RemainCrystalNumA;
    public int RemainCrystalNumB;

    public Dictionary<ESummon, int> SummonPriceDict;

    public static UnityEvent<ESummon, Image> RecieveBanPickSummon = new UnityEvent<ESummon, Image>();


    //RecieveBanPickSummon
    public BanPickRunner(int playerNum, List<ESummon> avaiableSummons)
    {
        PlayerNum = playerNum;
        AvaiableSummons = avaiableSummons;
        SetRoundStates();
        BannedSummonsTeamA = new List<ESummon>();
        BannedSummonsTeamB = new List<ESummon>();
        PickedSummonsTeamA = new List<ESummon>();
        PickedSummonsTeamB = new List<ESummon>();
        UpdatePickableSummons();
        RoundStateIndex = 0;
    }
    public BanPickRunner(Round round)
    {
        Round = round;

        PlayerNum = round.PlayerNum;
        AvaiableSummons = round.AvaiableSummons;
        CrystalMAXNum = round.CrystalMAXNum;
        
        RemainCrystalNumA = CrystalMAXNum;
        RemainCrystalNumB = CrystalMAXNum;

        SetPriceDictionary();
        SetRoundStates();

        BannedSummonsTeamA = new List<ESummon>();
        BannedSummonsTeamB = new List<ESummon>();
        PickedSummonsTeamA = new List<ESummon>();
        PickedSummonsTeamB = new List<ESummon>();

        RoundStateIndex = 0;

        UpdatePickableSummons();

        initializeEvents();
    }

    private void SetRoundStates()
    {
        RoundStates = new List<RoundStatePair>();
        
        switch(PlayerNum)
        {
            case 1:
                RoundStates = BanPickOrder.OnePlayerBanPickOrder;
                break;
            case 2:
                RoundStates = BanPickOrder.TwoPlayerMatchOrder;
                break;
            case 3:
                RoundStates = BanPickOrder.ThreePlayerMatchOrder;
                break;
            case 4:
                RoundStates = BanPickOrder.FourPlayerMatchOrder;
                break;
            default:
                break;
        }
    }

    public void AddBannedSummon(ETeamSide team, ESummon summon)
    {
        if(team == ETeamSide.TeamA)
        {
            BannedSummonsTeamA.Add(summon);
        }
        else
        {
            BannedSummonsTeamB.Add(summon);
        }
    }

    public void AddPickedSummon(ETeamSide team, ESummon summon)
    {
        if (team == ETeamSide.TeamA)
        {
            PickedSummonsTeamA.Add(summon);
            RemainCrystalNumA -= SummonPriceDict[summon];
        }
        else
        {
            PickedSummonsTeamB.Add(summon);
            RemainCrystalNumB -= SummonPriceDict[summon];
        }
    }

    public List<ESummon> GetPickableSummons()
    {
        ETeamSide team = CurrentRoundState.Turn;
        if (team == ETeamSide.TeamA)
        {
            return PickableSummonsA;
        }
        else
        {
            return PickableSummonsB;
        }
    }
    public void UpdatePickableSummons()
    {
        PickableSummonsA = new List<ESummon>();
        PickableSummonsB = new List<ESummon>();

        List<ESummon> banPickedSummons = GetBanPickedSummons();

        foreach (ESummon summon in AvaiableSummons)
        {
            if (!banPickedSummons.Contains(summon) && !PickedSummonsTeamA.Contains(summon) && !PickedSummonsTeamB.Contains(summon))
            {
                if(SummonPriceDict.ContainsKey(summon))
                {
                    // Todo: check if the remaining pick is enough to pick the summon.
                    // Check if the price of the summon is less than the remaining crystal number.
                    int price = SummonPriceDict[summon];
                    if(price <= RemainCrystalNumA)
                    {
                        PickableSummonsA.Add(summon);
                    }
                    if(price <= RemainCrystalNumB)
                    {
                        PickableSummonsB.Add(summon);
                    }
                }
                else
                {
                    Debug.LogError("Failed to get price of summon: " + summon.ToString());
                }
            }
        }
    }

    private List<ESummon> GetBanPickedSummons()
    {
        List<ESummon> banPickedSummons = new List<ESummon>();
        
        banPickedSummons.AddRange(BannedSummonsTeamA);
        banPickedSummons.AddRange(BannedSummonsTeamB);

        banPickedSummons.AddRange(PickedSummonsTeamA);
        banPickedSummons.AddRange(PickedSummonsTeamB);
        
        return banPickedSummons;
    }

    public RoundStatePair GetCurrentRoundState()
    {
        return RoundStates[RoundStateIndex];
    }

    private void SetPriceDictionary()
    {
        SummonPriceDict = new Dictionary<ESummon, int>();
        foreach(ESummon eSummon in AvaiableSummons)
        {
            // Get the price of the summon from the SummonScriptableObject.
            string targetPath = Directories.SummonData + eSummon.ToString();
            SummonScriptableObject summonScriptableObject = Resources.Load<SummonScriptableObject>(targetPath);

            if (summonScriptableObject != null)
            {
                SummonPriceDict.Add(eSummon, summonScriptableObject.CrystalNum);
            }
            else
            {
                Debug.LogError("Failed to load SummonScriptableObject from path: " + targetPath);
            }
        }
    }

    public void NextRoundState()
    {
        RoundStateIndex++;
        UpdatePickableSummons();

        BanPickUI.UpdateBanPickUIs.Invoke();
        if(CurrentRoundState.State == EBanPickState.Done)
        {
            Debug.Log("BanPickRunner: BanPick is done.");
            Round.GroupA.BannedSummon = BannedSummonsTeamA;
            Round.GroupB.BannedSummon = BannedSummonsTeamB;
            Round.GroupA.SelectedSummon = PickedSummonsTeamA;
            Round.GroupB.SelectedSummon = PickedSummonsTeamB;
        }

    }

    public void GetBanPickRequest(ESummon value, Image image)
    {
        // To do : other State that not involve with summon ban/pick.
        // [] Choose Side
        // [] Choose Map
        // [] Choose Strategy
        // [] Set Summoner&Summon pair

        switch(CurrentRoundState.State)
        {
            case EBanPickState.Ban:
                if(value.GetType() == typeof(ESummon))
                {
                    if(GetBanPickedSummons().Contains((ESummon)value))
                    {
                        Debug.LogError("Summon is already banned or picked: " + value.ToString());
                        return;
                    }
                    AddBannedSummon(CurrentRoundState.Turn, (ESummon)value);
                    NextRoundState();
                }
                else
                {
                    Debug.LogError("Invalid value type: " + value.GetType().ToString());
                }
                break;

            case EBanPickState.Pick:
                if (value.GetType() == typeof(ESummon))
                {
                    if (GetBanPickedSummons().Contains((ESummon)value))
                    {
                        Debug.LogError("Summon is already banned or picked: " + value.ToString());
                        return;
                    }
                    AddPickedSummon(CurrentRoundState.Turn, (ESummon)value);
                    BanPickUI.UpdateTeamPicked.Invoke(CurrentRoundState.Turn, image);
                    NextRoundState();
                }
                else
                {
                    Debug.LogError("Invalid value type: " + value.GetType().ToString());
                }
                break;

            case EBanPickState.Done:
                break;
            default:
                break;
        }
    }
    
    private void UpdateTeamPickedSummon(Image image)
    {
    }

    private void initializeEvents()
    {
        RecieveBanPickSummon = new UnityEvent<Enums.ESummon, Image>();
        RecieveBanPickSummon.AddListener((eSummon, image) => GetBanPickRequest(eSummon, image));
    }
}
public struct RoundStatePair
{
    public Enums.EBanPickState State { get; set; }
    public Enums.ETeamSide Turn { get; set; }
}