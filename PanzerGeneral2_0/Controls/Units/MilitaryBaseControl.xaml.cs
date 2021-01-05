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

namespace PanzerGeneral2_0.Controls.Units
{
    /// <summary>
    /// Interaction logic for MilitaryBaseControl.xaml
    /// </summary>
    public partial class MilitaryBaseControl : Unit
    {

        public string BaseTexture { get; set; }
        public MilitaryBaseControl(TeamInfo teamCode) : base(teamCode)
        {
            DataContext = this;

            if(teamCode == TeamInfo.TEAM_A)
            {
                BaseTexture = "/PanzerGeneral2_0;component/Resources/base_blue.png";
            } else
            {
                BaseTexture = "/PanzerGeneral2_0;component/Resources/base_red.png";
            }

            TeamCode = teamCode;
            UnitKind = UnitInfo.MILITARY_BASE;
            MaxAmmo = 0;
            CurrentAmmo = 0;
            MaxFuel = 0;
            CurrentFuel = 0;
            SoftAttackValue = 0;
            MediumAttackValue = 0;
            HardAttackValue = 0;
            SoftDefenceValue = 3;
            MediumDefenceValue = 3;
            HardDefenceValue = 3;
            Toughness = UnitInfo.HARD;
            MoveRange = 0;
            AttackRange = 0;
            Hp = 100;

            InitializeComponent();
        }
    }
}
