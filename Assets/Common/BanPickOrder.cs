using System.Collections.Generic;

public class BanPickOrder 
{
    public static readonly List<RoundStatePair> OnePlayerBanPickOrder = new List<RoundStatePair>
    {
//        new RoundStatePair { State = Enums.EBanPickState.SelectStage, Turn = Enums.ETeam.TeamB },

        new RoundStatePair { State = Enums.EBanPickState.Ban, Turn = Enums.ETeam.TeamA},
        new RoundStatePair { State = Enums.EBanPickState.Ban, Turn = Enums.ETeam.TeamB },

        new RoundStatePair { State = Enums.EBanPickState.Pick, Turn = Enums.ETeam.TeamA },
        new RoundStatePair { State = Enums.EBanPickState.Pick, Turn = Enums.ETeam.TeamB },

        //new RoundStatePair { State = Enums.EBanPickState.SetPair, Turn = Enums.ETeam.TeamA },
        //new RoundStatePair { State = Enums.EBanPickState.SetPair, Turn = Enums.ETeam.TeamB },

        //// SetStrategy
        //new RoundStatePair { State = Enums.EBanPickState.SelectStrategy, Turn = Enums.ETeam.TeamA },
        //new RoundStatePair { State = Enums.EBanPickState.SelectStrategy, Turn = Enums.ETeam.TeamB }

    };

    public static readonly List<RoundStatePair> TwoPlayerMatchOrder = new List<RoundStatePair>
    {
        new RoundStatePair { State = Enums.EBanPickState.Ban, Turn = Enums.ETeam.TeamA},
        new RoundStatePair { State = Enums.EBanPickState.Ban, Turn = Enums.ETeam.TeamB },
        new RoundStatePair { State = Enums.EBanPickState.SelectStage, Turn = Enums.ETeam.TeamB },
        // Ban
        new RoundStatePair { State = Enums.EBanPickState.Ban, Turn = Enums.ETeam.TeamA },
        new RoundStatePair { State = Enums.EBanPickState.Ban, Turn = Enums.ETeam.TeamB },
        // Pick
        new RoundStatePair { State = Enums.EBanPickState.Pick, Turn = Enums.ETeam.TeamA },
        new RoundStatePair { State = Enums.EBanPickState.Pick, Turn = Enums.ETeam.TeamB },
        new RoundStatePair { State = Enums.EBanPickState.Pick, Turn = Enums.ETeam.TeamB },
        new RoundStatePair { State = Enums.EBanPickState.Pick, Turn = Enums.ETeam.TeamA },
        // SetPair
        new RoundStatePair { State = Enums.EBanPickState.SetPair, Turn = Enums.ETeam.TeamA },
        new RoundStatePair { State = Enums.EBanPickState.SetPair, Turn = Enums.ETeam.TeamB },
        // SetStrategy
        new RoundStatePair { State = Enums.EBanPickState.SelectStrategy, Turn = Enums.ETeam.TeamA },
        new RoundStatePair { State = Enums.EBanPickState.SelectStrategy, Turn = Enums.ETeam.TeamB }
    };

    public static readonly List<RoundStatePair> ThreePlayerMatchOrder = new List<RoundStatePair>
    {
        new RoundStatePair { State = Enums.EBanPickState.Ban, Turn = Enums.ETeam.TeamA},
        new RoundStatePair { State = Enums.EBanPickState.Ban, Turn = Enums.ETeam.TeamB },
        new RoundStatePair { State = Enums.EBanPickState.SelectStage, Turn = Enums.ETeam.TeamB },
        //Ban
        new RoundStatePair { State = Enums.EBanPickState.Ban, Turn = Enums.ETeam.TeamA },
        new RoundStatePair { State = Enums.EBanPickState.Ban, Turn = Enums.ETeam.TeamB },
        new RoundStatePair { State = Enums.EBanPickState.Ban, Turn = Enums.ETeam.TeamB },
        new RoundStatePair { State = Enums.EBanPickState.Ban, Turn = Enums.ETeam.TeamA },
        //Pick
        new RoundStatePair { State = Enums.EBanPickState.Pick, Turn = Enums.ETeam.TeamA },
        new RoundStatePair { State = Enums.EBanPickState.Pick, Turn = Enums.ETeam.TeamB },
        new RoundStatePair { State = Enums.EBanPickState.Pick, Turn = Enums.ETeam.TeamB },
        new RoundStatePair { State = Enums.EBanPickState.Pick, Turn = Enums.ETeam.TeamA },
        new RoundStatePair { State = Enums.EBanPickState.Pick, Turn = Enums.ETeam.TeamA },
        new RoundStatePair { State = Enums.EBanPickState.Pick, Turn = Enums.ETeam.TeamB },
        // SetPair
        new RoundStatePair { State = Enums.EBanPickState.SetPair, Turn = Enums.ETeam.TeamA },
        new RoundStatePair { State = Enums.EBanPickState.SetPair, Turn = Enums.ETeam.TeamB },
        // SetStrategy
        new RoundStatePair { State = Enums.EBanPickState.SelectStrategy, Turn = Enums.ETeam.TeamA },
        new RoundStatePair { State = Enums.EBanPickState.SelectStrategy, Turn = Enums.ETeam.TeamB }

    };

    public static readonly List<RoundStatePair> FourPlayerMatchOrder = new List<RoundStatePair>
    {
        //new MatchStatePair { State = Enums.EBanPickState.SelectPlayer, Turn = Enums.ETeam.TeamA},
        //new MatchStatePair { State = Enums.EBanPickState.SelectPlayer, Turn = Enums.ETeam.TeamB },
        new RoundStatePair { State = Enums.EBanPickState.SelectStage, Turn = Enums.ETeam.TeamB },
        //Ban
        new RoundStatePair { State = Enums.EBanPickState.Ban, Turn = Enums.ETeam.TeamA },
        new RoundStatePair { State = Enums.EBanPickState.Ban, Turn = Enums.ETeam.TeamB },
        //Pick
        new RoundStatePair { State = Enums.EBanPickState.Pick, Turn = Enums.ETeam.TeamA },
        new RoundStatePair { State = Enums.EBanPickState.Pick, Turn = Enums.ETeam.TeamB },
        new RoundStatePair { State = Enums.EBanPickState.Pick, Turn = Enums.ETeam.TeamB },
        new RoundStatePair { State = Enums.EBanPickState.Pick, Turn = Enums.ETeam.TeamA },
        //Ban
        new RoundStatePair { State = Enums.EBanPickState.Ban, Turn = Enums.ETeam.TeamB },
        new RoundStatePair { State = Enums.EBanPickState.Ban, Turn = Enums.ETeam.TeamA },
        //Pick
        new RoundStatePair { State = Enums.EBanPickState.Pick, Turn = Enums.ETeam.TeamB },
        new RoundStatePair { State = Enums.EBanPickState.Pick, Turn = Enums.ETeam.TeamA },
        new RoundStatePair { State = Enums.EBanPickState.Pick, Turn = Enums.ETeam.TeamA },
        new RoundStatePair { State = Enums.EBanPickState.Pick, Turn = Enums.ETeam.TeamB },
        // SetPair
        new RoundStatePair { State = Enums.EBanPickState.SetPair, Turn = Enums.ETeam.TeamA },
        new RoundStatePair { State = Enums.EBanPickState.SetPair, Turn = Enums.ETeam.TeamB },
        // SetStrategy
        new RoundStatePair { State = Enums.EBanPickState.SelectStrategy, Turn = Enums.ETeam.TeamA },
        new RoundStatePair { State = Enums.EBanPickState.SelectStrategy, Turn = Enums.ETeam.TeamB }

    };
}
