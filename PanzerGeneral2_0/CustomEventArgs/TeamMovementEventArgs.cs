using System;
using static PanzerGeneral2_0.Controls.Units.Unit;

namespace PanzerGeneral2_0.CustomEventArgs
{
    public class TeamMovementEventArgs : EventArgs
    {
        public TeamInfo? CurrentTeam { get; set; }
        public int RoundNumber { get; set; }

        public TeamMovementEventArgs(TeamInfo? currentTeam, int roundNumber)
        {
            CurrentTeam = currentTeam;
            RoundNumber = roundNumber;
        }
    }
}
