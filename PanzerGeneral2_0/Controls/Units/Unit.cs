using System.ComponentModel;
using System.IO;
using System.Media;
using System.Windows.Controls;

namespace PanzerGeneral2_0.Controls.Units
{
    public abstract class Unit : UserControl, INotifyPropertyChanged
    {

        public static int DEFAULT_UNIT_HEIGHT = 80;
        public static int DEFAULT_UNIT_WIDTH = 80;

        public static string TEAM_A_COLOR_CODE = "#FF586ACA";
        public static string TEAM_B_COLOR_CODE = "#FFD45B5B";

        public event PropertyChangedEventHandler PropertyChanged;

        public enum UnitInfo
        {
            TANK,
            INFANTRY,
            CANNON,
            BASE,
            SOFT,
            MEDIUM,
            HARD
        }

        public enum TeamInfo
        {
            TEAM_A,
            TEAM_B
        }

        //Do odwracania tekstury
        public int Scale { get; set; }
        public string HpLabelColor { get; set; }
        public string TexturePath { get; set; }


        public TeamInfo TeamCode { get; set; }
        public UnitInfo UnitKind { get; set; }
        public int MaxAmmo { get; set; }
        public int CurrentAmmo { get; set; }
        public int MaxFuel { get; set; }
        public int CurrentFuel { get; set; }
        public int SoftAttackValue { get; set; }
        public int MediumAttackValue { get; set; }
        public int HardAttackValue { get; set; }
        public int DefenceValue { get; set; }
        public UnitInfo Toughness { get; set; }
        public int MoveRange { get; set; }
        public int AttackRange { get; set; }
        int _hp;
        public int Hp
        {
            get { return _hp; }
            set
            {
                _hp = value;
                NotifyPropertyChanged("Hp");
            }
        }

        public Unit(TeamInfo team) 
        {
            TeamCode = team;

            if (TeamCode == TeamInfo.TEAM_A)
            {
                Scale = 1;
                HpLabelColor = TEAM_A_COLOR_CODE;
            } 
            else
            {
                Scale = -1;
                HpLabelColor = TEAM_B_COLOR_CODE;
            }
            
        }

        /**
         * Efekt eksplozji
        */
        public void Explosion(bool isFinalExplosion)
        {
            UnmanagedMemoryStream sound;

            if (isFinalExplosion)
            {
                sound = Properties.Resources.explosion_final;
            }
            else
            {
                switch (UnitKind)
                {
                    case UnitInfo.INFANTRY:
                        sound = Properties.Resources.explosion_infantry;
                        break;

                    case UnitInfo.CANNON:
                        sound = Properties.Resources.explosion_cannon;
                        break;

                    case UnitInfo.TANK:
                        sound = Properties.Resources.explosion_tank;
                        break;

                    default:
                        sound = null;
                        break;
                }
            }

            new SoundPlayer(sound).Play();
        }

        private void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
