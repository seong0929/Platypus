using System.Collections;
using System.Collections.Generic;

public class Competition
{
    public string Name { get; set; }
    public int PlayerNum { get; set; }
    public int accessTeamNum { get; set; }
// public CompetitionTyped Tournament(size), Laegue+PlayOff, 
// Duration

    private List<Match> _results;
    private List<Team> _rank;
    private List<MatchPlan> _schedule;

    public bool IsQualified(){ return true; }
    private void GivePrize(){ return; }
    private void CreateSchedule(){ return; }
// privat void DropTeam(){}
}
