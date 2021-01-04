namespace PanzerGeneral2_0.Controls.Units
{
    /// <summary>
    /// Logika interakcji dla klasy Infantry.xaml
    /// </summary>
    public partial class InfantryControl : Unit
    {
        public InfantryControl(TeamInfo team) : base(team)
        {
            DataContext = this;
            TeamCode = team;
            UnitKind = UnitInfo.INFANTRY;
            MaxAmmo = 10;
            CurrentAmmo = 10;
            MaxFuel = 50;
            CurrentFuel = 50;
            SoftAttackValue = 5;
            MediumAttackValue = 3;
            HardAttackValue = 1;
            SoftDefenceValue = 5;
            MediumDefenceValue = 3;
            HardDefenceValue = 1;
            Toughness = UnitInfo.SOFT;
            MoveRange = 4;
            AttackRange = 1;
            Hp = 50;

            InitializeComponent();
        }
    }
}
