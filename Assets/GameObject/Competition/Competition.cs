using System.Collections;
using System.Collections.Generic;
using static Enums;

// # Competition:
// Competition is a tournament or league
// - Contains Data of the Competition
//   - Name of competition
//   - Number of players in matches
//   - Number of teams in competition
//   - Matches that will be held in competition
//   - Teams that are in competition
//   - Ranking of teams in competition
//   - Schedule of matches in competition
//
// - Manage the competition
//   - Create matches
//   - Create schedule
//   - Compute ranking
//   - Give prize
//   - check if team is qualified
//
public class Competition
{
    public string Name { get; set; }
    public int PlayerNum { get; set; }
    public int AccessTeamNum { get; set; }
    public List<ECompetitionType> CompetitionTypes { get; set; }

    public Date StartDate { get; set; }
    public Date EndDate { get; set; }

    public List<Date> PlayDates { get; set; }

    public int CrystalMAXNum { get; set; }
    public int WinPoint { get; set; }

    // public CompetitionTyped Tournament(size), Laegue+PlayOff, 
    // Duration
    private List<Match> _matches;
    private List<Team> _teams;
    private List<Team> _rank;
    private List<ESummon> _pickableSummon;

    Competition(string name, int playerNum, int accessTeamNum, List<ECompetitionType> competitionTypes, List<Date> playDates, int crystalMAXNum, int winPoint)
    {
        Name = name;
        PlayerNum = playerNum;
        AccessTeamNum = accessTeamNum;
        CompetitionTypes = competitionTypes;
        PlayDates = playDates;
        CrystalMAXNum = crystalMAXNum;
        WinPoint = winPoint;
    }

    private void GenerateMatches()
    {
        // Generate full league matches

        // Check if there are enough teams to generate matches
        if (_teams.Count < 2)
        {
            // Not enough teams for matches
            return;
        }

        // Calculate the number of matches needed
        int totalMatches = (_teams.Count * (_teams.Count - 1)) / 2;

        // Check if there are enough play dates
        if (PlayDates.Count == 0)
        {
            // No play dates specified
            return;
        }

        // Create matches for a full league
        _matches = new List<Match>();
        int matchCounter = 0;

        for (int i = 0; i < PlayDates.Count; i++)
        {
            int matchesPerDate = totalMatches / PlayDates.Count;
            int remainingMatches = totalMatches % PlayDates.Count;

            for (int j = 0; j < matchesPerDate; j++)
            {
                if (matchCounter >= totalMatches)
                {
                    break;
                }

                // Create a match between team i and team j with the current play date
                Match match = new Match(PlayerNum, CrystalMAXNum, WinPoint, _teams[i], _teams[(i + j + 1) % _teams.Count], PlayDates[i], _pickableSummon);
                _matches.Add(match);
                matchCounter++;
            }

            // Distribute any remaining matches evenly
            if (remainingMatches > 0)
            {
                if (matchCounter >= totalMatches)
                {
                    break;
                }

                // Create a match with the current play date
                Match match = new Match(PlayerNum, CrystalMAXNum, WinPoint, _teams[i], _teams[(i + matchesPerDate + 1) % _teams.Count], PlayDates[i], _pickableSummon);
                _matches.Add(match);
                matchCounter++;
                remainingMatches--;
            }
        }

    }

    public bool IsQualified(){ return true; }
    private void GivePrize(){ return; }
    private void CreateMatches(){ return; }

}
