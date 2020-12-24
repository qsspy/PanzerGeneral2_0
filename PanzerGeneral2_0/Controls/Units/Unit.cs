using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace PanzerGeneral2_0.Controls.Units
{
    public abstract class Unit : UserControl
    {
        public enum UnitInfo
        {
            TANK,
            INFANTRY,
            CANNON,
            SOFT,
            MEDIUM,
            HARD
        }

        public enum TeamInfo
        {
            TEAM_A,
            TEAM_B
        }

        public TeamInfo TeamCode { get; set; }
        public UnitInfo UnitKind { get; set; }
        public int? MaxAmmo { get; set; }
        public int? CurrentAmmo { get; set; }
        public int MaxFuel { get; set; }
        public int CurrentFuel { get; set; }
        public int SoftAttackValue { get; set; }
        public int MediumAttackValue { get; set; }
        public int HardAttackValue { get; set; }
        public int SoftDefenceValue { get; set; }
        public int MediumDefenceValue { get; set; }
        public int HardDefenceValue { get; set; }
        public UnitInfo Toughness { get; set; }
        public int MoveRange { get; set; }
        public int AttackRange { get; set; }
        public int Hp { get; set; }


        public Unit()
        {}
    }
}
