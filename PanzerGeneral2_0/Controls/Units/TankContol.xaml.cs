namespace PanzerGeneral2_0.Controls.Units
{
    /// <summary>
    /// Logika interakcji dla klasy TankContol.xaml
    /// </summary>
    public partial class TankContol : Unit
    {
        public TankContol(TeamInfo team) : base(team)
        {
            DataContext = this;
            TexturePath = "/PanzerGeneral2_0;component/Resources/tank.png";
            TeamCode = team;
            UnitKind = UnitInfo.TANK;
            MaxAmmo = 10;
            CurrentAmmo = 10;
            MaxFuel = 50;
            CurrentFuel = 50;
            SoftAttackValue = 10;
            MediumAttackValue = 5;
            HardAttackValue = 3;
            DefenceValue = 5;
            Toughness = UnitInfo.HARD;
            MoveRange = 3;
            AttackRange = 1;
            Hp = 50;

            InitializeComponent();
        }
    }
}
