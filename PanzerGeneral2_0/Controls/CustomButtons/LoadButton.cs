using PanzerGeneral2_0.Databases;
using PanzerGeneral2_0.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace PanzerGeneral2_0.Controls.CustomButtons
{
    class LoadButton : Button
    {
        private IDatabaseDao _sqliteDao = new SqliteDao();

        public IEnumerable<UnitModel> GetAllUnitsFromDb()
        {
            return _sqliteDao.GetAllUnitsFromDb();
        }

        public GameStateModel GetGameStateModelFromDb()
        {
            return _sqliteDao.GetGameStateFromDb();
        }
    }
}
