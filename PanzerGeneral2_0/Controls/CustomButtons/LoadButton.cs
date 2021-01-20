using PanzerGeneral2_0.Databases;
using PanzerGeneral2_0.DataModels;
using System.Collections.Generic;
using System.Windows.Controls;

namespace PanzerGeneral2_0.Controls.CustomButtons
{
    class LoadButton : Button
    {
        private readonly IDatabaseDao _sqliteDao = new SqliteDao();

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
