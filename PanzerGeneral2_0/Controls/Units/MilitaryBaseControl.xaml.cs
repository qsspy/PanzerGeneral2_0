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
            UnitKind = UnitInfo.BASE;
            MaxAmmo = 0;
            CurrentAmmo = 0;
            MaxFuel = 0;
            CurrentFuel = 0;
            DefenceValue = 3;
            Toughness = UnitInfo.HARD;
            MoveRange = 0;
            AttackRange = 0;
            Hp = 100;

            InitializeComponent();
        }
    }
}
