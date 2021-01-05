using PanzerGeneral2_0.Controls.Units;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PanzerGeneral2_0.Controls.Other
{
    
    public partial class UnitDetailsControl : UserControl
    {
        public string TeamCode { get; set; }
        public string UnitKind { get; set; }
        public string TexturePath { get; set; }
        public int MaxAmmo { get; set; }
        public int CurrentAmmo { get; set; }
        public int MaxFuel { get; set; }
        public int CurrentFuel { get; set; }
        public int SoftAttackValue { get; set; }
        public int MediumAttackValue { get; set; }
        public int HardAttackValue { get; set; }
        public int SoftDefenceValue { get; set; }
        public int MediumDefenceValue { get; set; }
        public int HardDefenceValue { get; set; }
        public string Toughness { get; set; }
        public int MoveRange { get; set; }
        public int AttackRange { get; set; }
        public int Hp { get; set; }
        public UnitDetailsControl(Unit unit)
        {
            
            DataContext = this;

            if (unit.TeamCode == Unit.TeamInfo.TEAM_A)
            {
                TeamCode = "Team A";
            }
            else
            {
                TeamCode = "Team B";
            }

            switch (unit)
            {
                case CannonControl cc:
                    UnitKind = "Cannon";
                    break;
                case TankContol tc:
                    UnitKind = "Tank";
                    break;
                case MilitaryBaseControl mbc:
                    UnitKind = "Military Base";
                    break;
                default:
                    UnitKind = "Infantry";
                    break;
            }

            switch(unit.Toughness)
            {
                case Unit.UnitInfo.SOFT:
                    Toughness = "Soft";
                    break;
                case Unit.UnitInfo.MEDIUM:
                    Toughness = "Medium";
                    break;
                default:
                    Toughness = "Hard";
                    break;
            }

            TexturePath = unit.TexturePath;
            MaxAmmo = unit.MaxAmmo;
            CurrentAmmo = unit.CurrentAmmo;
            MaxFuel = unit.MaxFuel;
            CurrentFuel = unit.CurrentFuel;
            SoftAttackValue = unit.SoftAttackValue;
            MediumAttackValue = unit.MediumAttackValue;
            HardAttackValue = unit.HardAttackValue;
            SoftDefenceValue = unit.SoftDefenceValue;
            MediumDefenceValue = unit.MediumDefenceValue;
            HardDefenceValue = unit.HardDefenceValue;
            MoveRange = unit.MoveRange;
            AttackRange = unit.AttackRange;
            Hp = unit.Hp;

            InitializeComponent();

            if (unit.TeamCode == Unit.TeamInfo.TEAM_A)
            {
                UnitTeamLabel.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString(Unit.TEAM_A_COLOR_CODE));
            }
            else
            {
                UnitTeamLabel.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString(Unit.TEAM_B_COLOR_CODE));
            }
        }
    }
}
