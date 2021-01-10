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
            TexturePath = "/PanzerGeneral2_0;component/Resources/soldier.png";
            TeamCode = team;
            UnitKind = UnitInfo.INFANTRY;
            MaxAmmo = 10;
            CurrentAmmo = 10;
            MaxFuel = 50;
            CurrentFuel = 50;
            SoftAttackValue = 7;
            MediumAttackValue = 5;
            HardAttackValue = 2;
            DefenceValue = 2;
            Toughness = UnitInfo.SOFT;
            MoveRange = 4;
            AttackRange = 1;
            Hp = 50;

            InitializeComponent();
        }
    }
}
