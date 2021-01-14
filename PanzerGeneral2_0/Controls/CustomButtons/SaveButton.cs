using PanzerGeneral2_0.Controls.Grid;
using PanzerGeneral2_0.Controls.Units;
using PanzerGeneral2_0.Databases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace PanzerGeneral2_0.Controls.CustomButtons
{

    class SaveButton : Button
    {
        private IDatabaseDao _sqliteDao = new SqliteDao();

        public void InsertNewUnitSet(IEnumerable<HexPoint> listOfHexes)
        {
            _sqliteDao.InsertNewUnitSet(listOfHexes);
        }

        public void UpdateGameStateInDb(Unit.TeamInfo? currentTurn, Unit.TeamInfo? winnerTeam)
        {
            _sqliteDao.UpdateGameStateInDb(currentTurn, winnerTeam);
        }
    }
}
