namespace PanzerGeneral2_0.Controls.Units
{
    /// <summary>
    /// Logika interakcji dla klasy TankContol.xaml
    /// </summary>
    public partial class TankControl : Unit
    {
        public TankControl(TeamInfo team) : base(team)
        {
            DataContext = this;
            TexturePath = "/PanzerGeneral2_0;component/Resources/tank.png";
            TeamCode = team;
            UnitKind = UnitInfo.TANK;
            MaxAmmo = 10;
            CurrentAmmo = 10;
            MaxFuel = 50;
            CurrentFuel = 50;
            SoftAttackValue = 7;
            MediumAttackValue = 3;
            HardAttackValue = 5;
            DefenceValue = 4;
            Toughness = UnitInfo.HARD;
            MoveRange = 3;
            AttackRange = 2;
            Hp = 50;

            InitializeComponent();
        }
    }
}
