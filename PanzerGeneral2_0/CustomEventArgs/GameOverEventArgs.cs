using System;
using static PanzerGeneral2_0.Controls.Units.Unit;

namespace PanzerGeneral2_0.CustomEventArgs
{
    public enum WinInfo
    {
        BASE_DESTROYED,
        ALL_UNITS_DESTROYED
    }

    public class GameOverEventArgs : EventArgs
    {
        public TeamInfo? WinningTeam { get; set; }
        public WinInfo? WayToWin { get; set; }

        public GameOverEventArgs(TeamInfo? winningTeam, WinInfo wayToWin)
        {
            WinningTeam = winningTeam;
            WayToWin = wayToWin;
        }
    }
}
