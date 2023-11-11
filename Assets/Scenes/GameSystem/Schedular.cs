using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Schedular
{
    public Dictionary<Date, List<Schedule>> Schedules;

    public Schedular(Team teamA, Team teamB)
    {
        Date targetDate = new Date() { Year = 2023, Month = 8, Day = 18 };
        Schedule schedule0 = new Schedule(Enums.ESchedule.Break);
        MatchPlan matchPlan = new MatchPlan()
        { PlayerNum = 2, CrystalMAXNum = 15, WinPoint = 2, TeamA = teamA, TeamB = teamB};
        Schedule schedule1 = new Schedule(Enums.ESchedule.Match, matchPlan);

        Schedules[targetDate] = new List<Schedule> { schedule0, schedule1 };
//        Schedules.Add(targetDate, schedule1 );

    }

}

public class Schedule
{
    public Enums.ESchedule ScheduleType;
    public MatchPlan MatchPlan;
    public Schedule(Enums.ESchedule scheduleType, MatchPlan matchPlan)
    {
        ScheduleType = scheduleType;
        MatchPlan = matchPlan;
    }
    public Schedule(Enums.ESchedule scheduleType)
    {
        ScheduleType = scheduleType;
    }
}
public struct Date
{
    public int Year;
    public int Month;
    public int Day;
}