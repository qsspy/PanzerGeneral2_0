namespace PanzerGeneral2_0.Controls.Units
{
    /// <summary>
    /// Logika interakcji dla klasy Cannon.xaml
    /// </summary>

    public partial class CannonControl : Unit
    {
        public CannonControl(TeamInfo team) : base(team)
        {
            DataContext = this;
            TexturePath = "/PanzerGeneral2_0;component/Resources/cannon.png";
            TeamCode = team;
            UnitKind = UnitInfo.CANNON;
            MaxAmmo = 10;
            CurrentAmmo = 10;
            MaxFuel = 50;
            CurrentFuel = 50;
            SoftAttackValue = 3;
            MediumAttackValue = 5;
            HardAttackValue = 7;
            DefenceValue = 3;
            Toughness = UnitInfo.MEDIUM;
            MoveRange = 2;
            AttackRange = 3;
            Hp = 50;

            InitializeComponent();
        }
    }
}
