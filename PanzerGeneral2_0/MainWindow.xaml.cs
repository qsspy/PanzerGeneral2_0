using PanzerGeneral2_0.Controls.Other;
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
using static PanzerGeneral2_0.Controls.Grid.HexPoint;

namespace PanzerGeneral2_0
{
    /// <summary>
    /// Logika interakcji dla klasy MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void onHexItemMouseEnter(object sender, CustomEventArgs.HexItemEventArgs e)
        {
            string x = e.HexXPos.ToString();
            string y = e.HexYPos.ToString();

            string terrainType = "";

            switch(e.TerrainInfo)
            {
                case HexPointTerrainInfo.FOREST:
                    terrainType = "Forest";
                    break;
                case HexPointTerrainInfo.MOUNTAINS:
                    terrainType = "Mountains";
                    break;
                default:
                    terrainType = "Plain";
                    break;
            }

            HexNameLabel.Content = $"{terrainType}({x}, {y})";

            Unit ownedUnit = e.OwnedUnit;

            if(ownedUnit == null)
            {
                UnitNameLabel.Content = "";

            } else
            {
                string teamCode = "";
                string unitKind = "";

                if(ownedUnit.TeamCode == Unit.TeamInfo.TEAM_A)
                {
                    teamCode = "Team A";
                } else
                {
                    teamCode = "Team B";
                }

                switch(ownedUnit)
                {
                    case CannonControl cc:
                        unitKind = "Cannon";
                        break;
                    case TankContol tc:
                        unitKind = "Tank";
                        break;
                    case MilitaryBaseControl mbc:
                        unitKind = "Military Base";
                        break;
                    default:
                        unitKind = "Infantry";
                        break;
                }

                UnitNameLabel.Content = $"{unitKind}({teamCode})";
            }
        }

        private void onBoardMouseLeave(object sender, MouseEventArgs e)
        {
            HexNameLabel.Content = "";
            UnitNameLabel.Content = "";
        }

        private void onUnitDetailsButtonClick(object sender, RoutedEventArgs e)
        {
            if(sender is Button)
            {
                int selectedHexIndex = GameplayFrame.lastUnitChecked;

                if (selectedHexIndex >= 0 && GameplayFrame.getHexAt(selectedHexIndex).Unit != null)
                {
                    bool detailsWindowVisible = GameplayFrame.UnitDetailsWindow.Children.Count != 0;

                    if(detailsWindowVisible)
                    {
                        DetailsButton.Content = "UNIT DETAILS";
                        GameplayFrame.UnitDetailsWindow.Children.Clear();
                        GameplayFrame.UnitDetailsWindow.Visibility = Visibility.Hidden;
                    }
                    else
                    {
                        DetailsButton.Content = "HIDE DETAILS";
                        var unit = GameplayFrame.getHexAt(selectedHexIndex).Unit;
                        GameplayFrame.UnitDetailsWindow.Children.Add(new UnitDetailsControl(unit));
                        GameplayFrame.UnitDetailsWindow.Visibility = Visibility.Visible;

                    }
                }
            }
        }
    }
}
