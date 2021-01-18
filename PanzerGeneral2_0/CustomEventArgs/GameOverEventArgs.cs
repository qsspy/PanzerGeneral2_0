using System;
using static PanzerGeneral2_0.Controls.Units.Unit;

namespace PanzerGeneral2_0.CustomEventArgs
{

    public class GameOverEventArgs : EventArgs
    {
        public TeamInfo? WinningTeam { get; set; }
        public GameOverEventArgs(TeamInfo? winningTeam)
        {
            WinningTeam = winningTeam;
        }
    }
}
