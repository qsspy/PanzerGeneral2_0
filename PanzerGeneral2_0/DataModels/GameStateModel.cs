using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static PanzerGeneral2_0.Controls.Units.Unit;

namespace PanzerGeneral2_0.DataModels
{

    class GameStateModel
    {
        //dlatego ze gra będzie mieć tylko jeden slot na zapis
        //to bedziemy miec zawsze maksymalnie jeden wiersz w tabeli stanu gry
        //id tego wiersza widnieje ponizej
        public static int GAME_STATE_SINGLE_ROW_ID = 1;

        public int Id { get; set; }
        public TeamInfo CurrentTurn { get; set; }

        //jezeli uzytkownik zapisal gre po wygraniu ktorejs z druzyn (na wszelki wypadek), moze byc nullem
        public TeamInfo WinnerTeamCode { get; set; }
    }
}
