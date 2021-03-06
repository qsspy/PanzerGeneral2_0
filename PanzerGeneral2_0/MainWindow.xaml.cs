﻿using PanzerGeneral2_0.Controls.Grid;
using PanzerGeneral2_0.Controls.Other;
using PanzerGeneral2_0.Controls.Units;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
            GameplayFrame.NextTeam();
        }

        private void OnHexItemMouseEnter(object sender, CustomEventArgs.HexItemEventArgs e)
        {
            string x = e.HexXPos.ToString();
            string y = e.HexYPos.ToString();

            string terrainType;
            switch (e.TerrainInfo)
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
                string teamCode;
                if (ownedUnit.TeamCode == Unit.TeamInfo.TEAM_A)
                {
                    teamCode = "Team A";
                }
                else
                {
                    teamCode = "Team B";
                }

                string unitKind;
                switch (ownedUnit)
                {
                    case CannonControl cc:
                        unitKind = "Cannon";
                        break;
                    case TankControl tc:
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

        private void OnBoardMouseLeave(object sender, MouseEventArgs e)
        {
            HexNameLabel.Content = "";
            UnitNameLabel.Content = "";
        }

        private void OnUnitDetailsButtonClick(object sender, RoutedEventArgs e)
        {
            if(sender is Button)
            {
                int selectedHexIndex = GameplayFrame.LastUnitIndex;

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
                         var unit = GameplayFrame.GetHexAt(selectedHexIndex).Unit;
                         GameplayFrame.UnitDetailsWindow.Children.Add(new UnitDetailsControl(unit));
                        _activeDialog = DialogType.UNIT_INFO_DIALOG;
                        
                    }
                }
            }
        }

        private void OnResetButtonClick(object sender, RoutedEventArgs e)
        {
            if(sender is Button)
            {
                if(_activeDialog == null)
                {
                    _activeDialog = DialogType.ALERT_DIALOG;
           
                    new PanzerAlertDialog.Builder()
                           .SetMessage("Are you sure you want to reset the game?")
                           .SetOnPositiveClickButtonListener("YES", (btnSender, args) => {
                               AttackHistoryBox.Text = "";
                               GameplayFrame.ResetGame();
                               GameplayFrame.UnitDetailsWindow.Children.Clear();
                               _activeDialog = null;
                           })
                           .SetOnNegativeClickButtonListener("NO", (btnSender, args) => {
                               GameplayFrame.UnitDetailsWindow.Children.Clear();
                               _activeDialog = null;
                           })
                           .Create()
                           .AttachToPanel(GameplayFrame.UnitDetailsWindow);
                }
            }
        }

        private void OnQuitButtonClick(object sender, RoutedEventArgs e)
        {
            if (sender is Button)
            {
                if (_activeDialog == null)
                {
                    _activeDialog = DialogType.ALERT_DIALOG;

                    new PanzerAlertDialog.Builder()
                           .SetMessage("Are you sure you want to quit the game?")
                           .SetOnPositiveClickButtonListener("YES", (btnSender, args) => {
                               Application.Current.Shutdown();
                           })
                           .SetOnNegativeClickButtonListener("NOT YET!", (btnSender, args) => {
                               GameplayFrame.UnitDetailsWindow.Children.Clear();
                               _activeDialog = null;
                           })
                           .Create()
                           .AttachToPanel(GameplayFrame.UnitDetailsWindow);
                }
            }
        }

        private void OnLoadButtonClick(object sender, RoutedEventArgs e)
        {
            if (_activeDialog == null)
            {
                _activeDialog = DialogType.LOADING_DIALOG;

                new PanzerLoadingDialogControl.Builder()
                    .SetOnLoadingListener(()=> 
                    {
                        var hexPoints = LoadButton.GetAllUnitsFromDb();
                        if(hexPoints.Count() != 0)
                        {
                            var gameState = LoadButton.GetGameStateModelFromDb();

                            Application.Current.Dispatcher.Invoke(() => {
                                AttackHistoryBox.Text = "";
                                GameplayFrame.DistributeLoadedUnitsOnBoard(hexPoints,gameState);
                                GameplayFrame.ResetCheckedElements();
                            });
                        }
                    })
                    .SetOnFinishButtonClickListener((btnSender, args) =>
                    {
                        GameplayFrame.UnitDetailsWindow.Children.Clear();
                        _activeDialog = null;
                    })
                    .Create()
                    .AttachToPanel(GameplayFrame.UnitDetailsWindow)
                    .StartLoading(); 
            }
        }

        private void OnSaveButtonClick(object sender, RoutedEventArgs e)
        {
            if (_activeDialog == null)
            {
                _activeDialog = DialogType.LOADING_DIALOG;

                new PanzerLoadingDialogControl.Builder()
                    .SetWaitingMessage("Saving")
                    .SetFinishLoadingMessage("Saving Finished!")
                    .SetOnFinishButtonClickListener((btnSender, args) =>
                    {
                        GameplayFrame.UnitDetailsWindow.Children.Clear();
                        _activeDialog = null;
                    })
                    .SetOnLoadingListener(() =>
                    {
                        SaveButton.InsertNewUnitSet(GameplayFrame.HexPoints,GameplayFrame.CurrentTeamUnitsToMove, GameplayFrame.CurrentTeamUnitsToShot);
                        SaveButton.UpdateGameStateInDb(GameplayFrame.CurrentTeam,null,GameplayFrame.TeamMovementsCounter);
                    })
                    .Create()
                    .AttachToPanel(GameplayFrame.UnitDetailsWindow)
                    .StartLoading();
            }
        }

        private void OnEncounterEnd(object sender, CustomEventArgs.UnitCombatEventArgs e)
        {
            if(sender is HexBoard)
            {
                string attackerColorCode;
                string defenderColorCode;

                Unit notNullUnit;
                if (e.Attacker != null)
                {
                    notNullUnit = e.Attacker;
                }
                else
                {
                    notNullUnit = e.Defender;
                }

                if(notNullUnit.TeamCode == Unit.TeamInfo.TEAM_A)
                {
                    attackerColorCode = Unit.TEAM_A_COLOR_CODE;
                    defenderColorCode = Unit.TEAM_B_COLOR_CODE;
                }
                else
                {
                    attackerColorCode = Unit.TEAM_B_COLOR_CODE;
                    defenderColorCode = Unit.TEAM_A_COLOR_CODE;
                }
                
                
                if (AttackHistoryBox.Text.Length != 0)
                {
                    AttackHistoryBox.Inlines.Add(new Run("\n\n"));
                }

                if (e.IsCounterattack)
                {
                    AttackHistoryBox.Inlines.Add(new Run($"{e.Attacker.UnitKind}") { 
                        Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString(attackerColorCode)),
                        FontWeight = FontWeights.Bold});
                    AttackHistoryBox.Inlines.Add(new Run(" strikes back with "));
                    AttackHistoryBox.Inlines.Add(new Run($"{e.DamagePoints}") {FontWeight = FontWeights.Bold });

                    if (e.Attacker == null)
                    {
                        AttackHistoryBox.Inlines.Add(new Run(" points of damage and kills the attacker."));
                    }
                    else
                    {
                        AttackHistoryBox.Inlines.Add(new Run(" points of damage."));
                    }
                } 
                else
                {

                    AttackHistoryBox.Inlines.Add(new Run($"{e.Attacker.UnitKind}") { 
                        Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString(attackerColorCode)),
                        FontWeight = FontWeights.Bold
                    });

                    if (e.Defender == null)
                    {
                        AttackHistoryBox.Inlines.Add(new Run(" attacks with "));
                        AttackHistoryBox.Inlines.Add(new Run($"{e.DamagePoints}") { FontWeight = FontWeights.Bold });
                        AttackHistoryBox.Inlines.Add(new Run(" points of damage and kills opponent."));


                    }
                    else
                    {
                        AttackHistoryBox.Inlines.Add(new Run(" attacks "));
                        AttackHistoryBox.Inlines.Add(new Run($"{e.Defender.UnitKind}") { 
                            Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString(defenderColorCode)),
                            FontWeight = FontWeights.Bold
                        });
                        AttackHistoryBox.Inlines.Add(new Run(" with "));
                        AttackHistoryBox.Inlines.Add(new Run($"{e.DamagePoints}") { FontWeight = FontWeights.Bold });
                        AttackHistoryBox.Inlines.Add(new Run(" points of damage."));
                    }
                }

                AttackHistoryBoxScroll.ScrollToEnd();

            }
        }

        private void OnPassButtonClick(object sender, RoutedEventArgs e)
        {
            GameplayFrame.NextTeam();
            GameplayFrame.ResetCheckedElements();
        }

        private void OnTeamChange(object sender, CustomEventArgs.TeamMovementEventArgs e)
        {
            if(sender is HexBoard)
            {
                if(e.CurrentTeam == Unit.TeamInfo.TEAM_A)
                {
                    TeamLabel.Content = "Team A";
                    TeamLabel.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString(Unit.TEAM_A_COLOR_CODE));

                } else
                {
                    TeamLabel.Content = "Team B";
                    TeamLabel.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString(Unit.TEAM_B_COLOR_CODE));
                }
            }
        }

        private void OnGameOver(object sender, CustomEventArgs.GameOverEventArgs e)
        {
            if(sender is HexBoard)
            {
                if (_activeDialog == null)
                {
                    _activeDialog = DialogType.ALERT_DIALOG;


                    var message = "";

                    if(e.WinningTeam == Unit.TeamInfo.TEAM_A)
                    {
                        message = "Team A has won! Do you want to reset the game?";
                    } else
                    {
                        message = "Team B has won! Do you want to reset the game?";
                    }

                    new PanzerAlertDialog.Builder()
                           .SetMessage(message)
                           .SetOnPositiveClickButtonListener("YES", (btnSender, args) => {

                               AttackHistoryBox.Text = "";
                               GameplayFrame.ResetGame();
                               GameplayFrame.UnitDetailsWindow.Children.Clear();
                               _activeDialog = null;
                           })
                           .SetOnNegativeClickButtonListener("QUIT", (btnSender, args) => {
                               
                               Application.Current.Shutdown();
                           })
                           .Create()
                           .AttachToPanel(GameplayFrame.UnitDetailsWindow);
                }
            }
        }
    }
}
