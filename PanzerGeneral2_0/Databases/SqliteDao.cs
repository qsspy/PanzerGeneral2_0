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
    public class SqliteDao : IDatabaseDao
    {

        public void InsertUnitToDb(Unit unit, IntPoint coordinates)
        {
            using (var _dbContext = new SqliteDbContext())
            {
                var model = new UnitModel()
                {
                    TeamCode = unit.TeamCode,
                    UnitKind = unit.UnitKind,
                    XPosition = coordinates.X,
                    YPosition = coordinates.Y,
                    Hp = unit.Hp
                };
                _dbContext.Add(model);
                _dbContext.SaveChanges();
            }
        }

        public void RemoveAllUnitsFromDB()
        {
            using(var _dbContext = new SqliteDbContext())
            {
                _dbContext.Unit.RemoveRange(_dbContext.Unit);
                _dbContext.SaveChanges();
            }
        }


        //Kiedy chcemy zapisac wszystkie jednostki za jednym zamachem
        public void InsertNewUnitSet(IEnumerable<HexPoint> listOfHexes)
        {
            RemoveAllUnitsFromDB();
            foreach(var hex in listOfHexes)
            {
                if(hex.Unit != null)
                {
                    InsertUnitToDb(hex.Unit, hex.Point);
                }
            }
        }

        public IEnumerable<UnitModel> GetAllUnitsFromDb()
        {
            using(var _dbContext = new SqliteDbContext())
            {
                return _dbContext.Unit.ToList();
            }
        }

        public void InsertGameStateToDb(Unit.TeamInfo? currentTurn, Unit.TeamInfo? winnerTeam)
        {
            using(var _dbContext = new SqliteDbContext())
            {
                var model = new GameStateModel()
                {
                    Id = GameStateModel.GAME_STATE_SINGLE_ROW_ID,
                    CurrentTurn = currentTurn,
                    WinnerTeamCode = winnerTeam
                };

                _dbContext.Add(model);
                _dbContext.SaveChanges();
            }
        }

        public bool IsGameStateInDb()
        {
            using (var _dbContext = new SqliteDbContext())
            {
                return _dbContext.GameState.Find(GameStateModel.GAME_STATE_SINGLE_ROW_ID) != null; 
            }
        }

        public void RemoveGameStateFromDb()
        {
            using (var _dbContext = new SqliteDbContext())
            {
                _dbContext.GameState.RemoveRange(_dbContext.GameState);
                _dbContext.SaveChanges();
            }
        }


        //Wstawia aktualny stan gry do bazy
        public void UpdateGameStateInDb(Unit.TeamInfo? currentTurn, Unit.TeamInfo? winnerTeam)
        {
            if(IsGameStateInDb())
            {
                RemoveGameStateFromDb();
            }

            InsertGameStateToDb(currentTurn, winnerTeam);
        }

        public GameStateModel GetGameStateFromDb()
        {
            using(var _dbContext = new SqliteDbContext())
            {
                return _dbContext.GameState.Find(GameStateModel.GAME_STATE_SINGLE_ROW_ID);
            }
        }
    }
}
