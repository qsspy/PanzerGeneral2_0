using PanzerGeneral2_0.Controls.Grid;
using PanzerGeneral2_0.Controls.Units;
using PanzerGeneral2_0.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PanzerGeneral2_0.Databases
{
    public class SqliteDao : IDatabaseDao
    {

        public void InsertUnitToDb(Unit unit, IntPoint coordinates, bool unitCanAttack, bool unitCanMove)
        {
            using (var _dbContext = new SqliteDbContext())
            {
                var model = new UnitModel()
                {
                    TeamCode = unit.TeamCode,
                    UnitKind = unit.UnitKind,
                    XPosition = coordinates.X,
                    YPosition = coordinates.Y,
                    Hp = unit.Hp,
                    CanAttack = unitCanAttack,
                    CanMove = unitCanMove
                };
                _dbContext.Add(model);
                _dbContext.SaveChanges();
            }
        }

        /**
         * Metoda usuwająca wszystkie jednostki na raz
         */
        public void RemoveAllUnitsFromDB()
        {
            using(var _dbContext = new SqliteDbContext())
            {
                _dbContext.Unit.RemoveRange(_dbContext.Unit);
                _dbContext.SaveChanges();
            }
        }


        /**
         * Metoda zapisująca wszystkie jednostki na raz
         */
        public void InsertNewUnitSet(IEnumerable<HexPoint> listOfHexes, IEnumerable<Unit> movingUnits, IEnumerable<Unit> attackingUnits)
        {
            RemoveAllUnitsFromDB();
            foreach(var hex in listOfHexes)
            {
                if(hex.Unit != null)
                {
                    bool canMove = movingUnits.Contains(hex.Unit);
                    bool canAttack = attackingUnits.Contains(hex.Unit);
                    InsertUnitToDb(hex.Unit, hex.Point,canAttack,canMove);
                }
            }
        }

        /**
         * Metoda zwracająca wszystkie jednostki na raz
         */
        public IEnumerable<UnitModel> GetAllUnitsFromDb()
        {
            using(var _dbContext = new SqliteDbContext())
            {
                return _dbContext.Unit.ToList();
            }
        }

        /**
         * Zapisz stan gry
         */
        public void InsertGameStateToDb(Unit.TeamInfo? currentTurn, Unit.TeamInfo? winnerTeam, int roundCount)
        {
            using(var _dbContext = new SqliteDbContext())
            {
                var model = new GameStateModel()
                {
                    Id = GameStateModel.GAME_STATE_SINGLE_ROW_ID,
                    CurrentTurn = currentTurn,
                    WinnerTeamCode = winnerTeam,
                    RoundNumber = roundCount
                };

                _dbContext.Add(model);
                _dbContext.SaveChanges();
            }
        }

        /**
         * Sprawdź czy zapis jest w bazie
         */
        public bool IsGameStateInDb()
        {
            using (var _dbContext = new SqliteDbContext())
            {
                return _dbContext.GameState.Find(GameStateModel.GAME_STATE_SINGLE_ROW_ID) != null; 
            }
        }

        /**
         * Usuń zapis z bazy danych
         */
        public void RemoveGameStateFromDb()
        {
            using (var _dbContext = new SqliteDbContext())
            {
                _dbContext.GameState.RemoveRange(_dbContext.GameState);
                _dbContext.SaveChanges();
            }
        }


        /**
         * Wstaw aktualny stan gry do bazy
         */
        public void UpdateGameStateInDb(Unit.TeamInfo? currentTurn, Unit.TeamInfo? winnerTeam, int roundCount)
        {
            if(IsGameStateInDb())
            {
                RemoveGameStateFromDb();
            }

            InsertGameStateToDb(currentTurn, winnerTeam, roundCount);
        }

        /**
         * Pobierz aktualny zapis gry
         */
        public GameStateModel GetGameStateFromDb()
        {
            using(var _dbContext = new SqliteDbContext())
            {
                return _dbContext.GameState.Find(GameStateModel.GAME_STATE_SINGLE_ROW_ID);
            }
        }
    }
}
