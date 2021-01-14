using PanzerGeneral2_0.Controls.Grid;
using PanzerGeneral2_0.Controls.Units;
using PanzerGeneral2_0.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PanzerGeneral2_0.Databases
{
    interface IDatabaseDao
    {
        void InsertUnitToDb(Unit unit, IntPoint coordinates);
        void RemoveAllUnitsFromDB();
        void InsertNewUnitSet(IEnumerable<HexPoint> listOfHexes);
        IEnumerable<UnitModel> GetAllUnitsFromDb();
        void InsertGameStateToDb(Unit.TeamInfo? currentTurn, Unit.TeamInfo? winnerTeam);
        bool IsGameStateInDb();
        void RemoveGameStateFromDb();
        void UpdateGameStateInDb(Unit.TeamInfo? currentTurn, Unit.TeamInfo? winnerTeam);
        GameStateModel GetGameStateFromDb();
    }
}
