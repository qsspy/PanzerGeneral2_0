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
    /// 


    public enum DialogType
    {
        UNIT_INFO_DIALOG,
        LOADING_DIALOG,
        ALERT_DIALOG
    }

    public partial class MainWindow : Window
    {

        private DialogType? _activeDialog = null;

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

                if (selectedHexIndex >= 0 && GameplayFrame.GetHexAt(selectedHexIndex).Unit != null)
                {

                    if(_activeDialog == DialogType.UNIT_INFO_DIALOG)
                    {
                        DetailsButton.Content = "UNIT DETAILS";
                        GameplayFrame.UnitDetailsWindow.Children.Clear();
                        _activeDialog = null;
                    }
                    else if(_activeDialog == null)
                    {
                        
                         DetailsButton.Content = "HIDE DETAILS";
                         var unit = GameplayFrame.getHexAt(selectedHexIndex).Unit;
                         GameplayFrame.UnitDetailsWindow.Children.Add(new UnitDetailsControl(unit));
                        _activeDialog = DialogType.UNIT_INFO_DIALOG;
                        
                    }
                }
            }
        }

        private void onResetButtonClick(object sender, RoutedEventArgs e)
        {
            if(sender is Button)
            {
                if(_activeDialog == null)
                {
                    _activeDialog = DialogType.ALERT_DIALOG;
           
                    new PanzerAlertDialog.Builder()
                           .SetMessage("Are you sure you want to reset the game?")
                           .setOnPositiveClickButtonListener("YES", (btnSender, args) => {
                               //TODO
                               GameplayFrame.UnitDetailsWindow.Children.Clear();
                               _activeDialog = null;
                           })
                           .setOnNegativeClickButtonListener("NO", (btnSender, args) => {
                               GameplayFrame.UnitDetailsWindow.Children.Clear();
                               _activeDialog = null;
                           })
                           .create()
                           .attachToPanel(GameplayFrame.UnitDetailsWindow);
                }
            }
        }

        private void onQuitButtonClick(object sender, RoutedEventArgs e)
        {
            if (sender is Button)
            {
                if (_activeDialog == null)
                {
                    _activeDialog = DialogType.ALERT_DIALOG;

                    new PanzerAlertDialog.Builder()
                           .SetMessage("Are you sure you want to quit the game?")
                           .setOnPositiveClickButtonListener("YES", (btnSender, args) => {
                               Application.Current.Shutdown();
                           })
                           .setOnNegativeClickButtonListener("NOT YET!", (btnSender, args) => {
                               GameplayFrame.UnitDetailsWindow.Children.Clear();
                               _activeDialog = null;
                           })
                           .create()
                           .attachToPanel(GameplayFrame.UnitDetailsWindow);
                }
            }
        }

        private void onLoadButtonClick(object sender, RoutedEventArgs e)
        {
            if (_activeDialog == null)
            {
                _activeDialog = DialogType.LOADING_DIALOG;

                new PanzerLoadingDialogControl.Builder()
                    .setSimulatedWaitTime(5000)
                    .setOnFinishButtonClickListener((btnSender, args) =>
                    {
                        GameplayFrame.UnitDetailsWindow.Children.Clear();
                        _activeDialog = null;
                    })
                    .setOnLoadingListener(() =>
                    {
                        //TODO - ladowanie z bazy danych
                    })
                    .create()
                    .attachToPanel(GameplayFrame.UnitDetailsWindow)
                    .startLoading(); 
            }
        }

        private void onSaveButtonClick(object sender, RoutedEventArgs e)
        {
            if (_activeDialog == null)
            {
                _activeDialog = DialogType.LOADING_DIALOG;

                new PanzerLoadingDialogControl.Builder()
                    .setSimulatedWaitTime(5000)
                    .setWaitingMessage("Saving")
                    .setFinishLoadingMessage("Saving Finished!")
                    .setOnFinishButtonClickListener((btnSender, args) =>
                    {
                        GameplayFrame.UnitDetailsWindow.Children.Clear();
                        _activeDialog = null;
                    })
                    .setOnLoadingListener(() =>
                    {
                        //TODO - zapisywanie do bazy danych
                    })
                    .create()
                    .attachToPanel(GameplayFrame.UnitDetailsWindow)
                    .startLoading();
            }
        }
    }
}
