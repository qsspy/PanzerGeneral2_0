using PanzerGeneral2_0.Controls.Grid.Helpers;
using PanzerGeneral2_0.Controls.Units;
using PanzerGeneral2_0.CustomEventArgs;
using PanzerGeneral2_0.DataModels;
using PanzerGeneral2_0.Factories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Input;
using static PanzerGeneral2_0.Controls.Grid.HexPoint;
using static PanzerGeneral2_0.Controls.Units.Unit;

namespace PanzerGeneral2_0.Controls.Grid
{
    public static class CheckedHexPointInfo
    {
        public const double ATTACK_CHECKED = 0.3;
        public const double TERRAIN_CHECKED = 0.6;
        public const double NOT_CHECKED = 1;
    }

    /// <summary>
    /// Logika interakcji dla klasy HexBoard.xaml
    /// </summary>
    public partial class HexBoard : UserControl
    {

        public List<HexPoint> HexPoints { get; set; } = new List<HexPoint>();
        public List<Unit> CurrentTeamUnitsToMove { get; set; } = new List<Unit>();
        public List<Unit> CurrentTeamUnitsToShot { get; set; } = new List<Unit>();
        public int LastUnitIndex { get; set; }
        public TeamInfo? CurrentTeam { get; set; }
        public int TeamMovementsCounter { get; set; }

        public event EventHandler<HexItemEventArgs> HexItemMouseEnterEvent;
        public event EventHandler<UnitCombatEventArgs> UnitCombatEvent;
        public event EventHandler<TeamMovementEventArgs> TeamMovementEvent;
        public event EventHandler<GameOverEventArgs> GameOverEvent;

        public HexBoard()
        {
            InitializeComponent();
           
            // wartości startowe dla rozgrywki
            this.LastUnitIndex = -1;
            TeamMovementsCounter = 1;
            CurrentTeam = (TeamInfo) new Random().Next(0, 2);

            DistributeHexesOnBoard();
        }

        /**
         * Metoda obsługująca zdarzenie kliknięcia lewym przyciskiem myszy na hexpoint
         */
        private void HexItem_PreviewMouseButtonDown(object sender, MouseButtonEventArgs e)
        {
            SetUnitAfterClickInteraction((HexPoint)sender, e.ChangedButton);
        }

        /*
         * Metoda po najechaniu kursorem myszy na hex point rzuca wyjątek z danymi hex pointa
         */
        private void HexItem_MouseEnter(object sender, MouseEventArgs e)
        {
            if (sender is HexPoint point)
            {
                var index = Board.ItemContainerGenerator.IndexFromContainer(point);
                int x = index / Board.ColumnCount;
                int y = index % Board.ColumnCount;
                HexPointTerrainInfo terrainInfo = HexPoints[index].Terrain;
                HexItemEventArgs args = new HexItemEventArgs(x, y, terrainInfo) { OwnedUnit = HexPoints[index].Unit };
                HexItemMouseEnterEvent?.Invoke(this, args);
            }
        }

        /**
         * Przygotowuje turę dla zaczynającego gracza
         */
        public void NextTeam()
        {
            CurrentTeam = CurrentTeam == TeamInfo.TEAM_A ? TeamInfo.TEAM_B : TeamInfo.TEAM_A;
            int roundNumber = ++TeamMovementsCounter / 2;
            TeamMovementEvent?.Invoke(this, new TeamMovementEventArgs(CurrentTeam, roundNumber));

            CurrentTeamUnitsToMove.Clear();
            CurrentTeamUnitsToShot.Clear();

            foreach (HexPoint hexPoint in HexPoints)
            {
                if (hexPoint.Unit != null && hexPoint.Unit.TeamCode == CurrentTeam)
                {
                    CurrentTeamUnitsToMove.Add(hexPoint.Unit);
                    CurrentTeamUnitsToShot.Add(hexPoint.Unit);
                }
            }
        }

        /**
         Przypisuje HexItem'om teren oraz jednostkę opisane w plikach tekstowych
         */
        private void DistributeHexesOnBoard()
        {

            // dystrybucja terenu w pliku z legendą terrain_distribution.txt
            String terrainDistribution = Properties.Resources.terrain_distribution;
            // dystrybucja jednostek w plku z legendą unit_distribution.txt
            String unitDistribution = Properties.Resources.unit_distribution;

            var rowCount = Board.RowCount;
            var columnCount = Board.ColumnCount;

            string[] terrainLines = terrainDistribution.Split('\n');
            string[] unitLines = unitDistribution.Split('\n');

            //dla kazdej lini z pliku
            foreach (int i in Enumerable.Range(0, rowCount))
            {
                var terrainLine = terrainLines[i].Split(' ');
                var unitLine = unitLines[i].Split(' ');

                //dla kazdej cyfry z pliku
                foreach (int j in Enumerable.Range(0, columnCount))
                {
                    HexPoint hexPoint;
                    IntPoint intPoint = new IntPoint(j, i);

                    switch (Int32.Parse(terrainLine[j]))
                    {
                        case 0:
                            hexPoint = HexPointFactory.BuildHexPoint("Plain", intPoint);
                            break;
                        case 1:
                            hexPoint = HexPointFactory.BuildHexPoint("Forest", intPoint);
                            break;
                        default:
                            hexPoint = HexPointFactory.BuildHexPoint("Mountains", intPoint);
                            break;
                    }

                    switch (Int32.Parse(unitLine[j]))
                    {
                        case 1:
                            hexPoint.BindUnitToHex(UnitFactory.BuildUnit("Infantry", Unit.TeamInfo.TEAM_A));
                            break;
                        case 2:
                            hexPoint.BindUnitToHex(UnitFactory.BuildUnit("Cannon", Unit.TeamInfo.TEAM_A));
                            break;
                        case 3:
                            hexPoint.BindUnitToHex(UnitFactory.BuildUnit("Tank", Unit.TeamInfo.TEAM_A));
                            break;
                        case 4:
                            hexPoint.BindUnitToHex(UnitFactory.BuildUnit("Base", Unit.TeamInfo.TEAM_A));
                            break;
                        case 5:
                            hexPoint.BindUnitToHex(UnitFactory.BuildUnit("Infantry", Unit.TeamInfo.TEAM_B));
                            break;
                        case 6:
                            hexPoint.BindUnitToHex(UnitFactory.BuildUnit("Cannon", Unit.TeamInfo.TEAM_B));
                            break;
                        case 7:
                            hexPoint.BindUnitToHex(UnitFactory.BuildUnit("Tank", Unit.TeamInfo.TEAM_B));
                            break;
                        case 8:
                            hexPoint.BindUnitToHex(UnitFactory.BuildUnit("Base", Unit.TeamInfo.TEAM_B));
                            break;
                        default:
                            break;
                    }

                    HexPoints.Add(hexPoint);
                }
            }

            Board.ItemsSource = HexPoints;
        }

        /**
         * Metoda realizująca rozmieszczenie hexpointów na hexboardzie
         */
        public void DistributeLoadedUnitsOnBoard(IEnumerable<UnitModel> unitModels, GameStateModel stateModel)
        {
            CurrentTeam = stateModel.CurrentTurn;
            TeamMovementsCounter = stateModel.RoundNumber;
            TeamMovementEvent?.Invoke(this, new TeamMovementEventArgs(CurrentTeam, TeamMovementsCounter));

            CurrentTeamUnitsToMove.Clear();
            CurrentTeamUnitsToShot.Clear();

            foreach (var point in HexPoints)
            {
                point.UnbindUnitFromHex();
            }

            foreach(var model in unitModels)
            {
                var loadedUnit = UnitFactory.BuildUnit(model.UnitKind, model.TeamCode);
                loadedUnit.Hp = model.Hp;
                if (model.CanAttack) CurrentTeamUnitsToShot.Add(loadedUnit);
                if (model.CanMove) CurrentTeamUnitsToMove.Add(loadedUnit);
                GetHexAt(model.XPosition, model.YPosition).BindUnitToHex(loadedUnit);
            }
        }

        /**
         * Metoda resetuje ułożenie gry do stanu pierwotnego
         */
        public void ResetGame()
        {

            foreach(var point in HexPoints)
            {
                point.UnbindUnitFromHex();
            }

            ResetCheckedElements();

            String unitDistribution = Properties.Resources.unit_distribution;

            var rowCount = Board.RowCount;
            var columnCount = Board.ColumnCount;

            string[] unitLines = unitDistribution.Split('\n');

            //dla kazdej lini z pliku
            foreach (int i in Enumerable.Range(0, rowCount))
            {
                var unitLine = unitLines[i].Split(' ');

                //dla kazdej cyfry z pliku
                foreach (int j in Enumerable.Range(0, columnCount))
                {
                    var currentHex = GetHexAt(j, i);
                    switch (Int32.Parse(unitLine[j]))
                    {
                        case 1:
                            currentHex.BindUnitToHex(UnitFactory.BuildUnit("Infantry", Unit.TeamInfo.TEAM_A));
                            break;
                        case 2:
                            currentHex.BindUnitToHex(UnitFactory.BuildUnit("Cannon", Unit.TeamInfo.TEAM_A));
                            break;
                        case 3:
                            currentHex.BindUnitToHex(UnitFactory.BuildUnit("Tank", Unit.TeamInfo.TEAM_A));
                            break;
                        case 4:
                            currentHex.BindUnitToHex(UnitFactory.BuildUnit("Base", Unit.TeamInfo.TEAM_A));
                            break;
                        case 5:
                            currentHex.BindUnitToHex(UnitFactory.BuildUnit("Infantry", Unit.TeamInfo.TEAM_B));
                            break;
                        case 6:
                            currentHex.BindUnitToHex(UnitFactory.BuildUnit("Cannon", Unit.TeamInfo.TEAM_B));
                            break;
                        case 7:
                            currentHex.BindUnitToHex(UnitFactory.BuildUnit("Tank", Unit.TeamInfo.TEAM_B));
                            break;
                        case 8:
                            currentHex.BindUnitToHex(UnitFactory.BuildUnit("Base", Unit.TeamInfo.TEAM_B));
                            break;
                        default:
                            break;
                    }
                   
                }
            }
            if ((TeamInfo)new Random().Next(0, 2) == CurrentTeam)
            {
                NextTeam();   
            }
            else
            {
                NextTeam();
                NextTeam();
            }
        }

        /**
         * Metoda realizująca interakcję klikniętego hexpointa w zależności od użytego klawisza myszy
         */
        private void SetUnitAfterClickInteraction(HexPoint checkedHexPoint, MouseButton mouseButton)
        {
            var index = Board.ItemContainerGenerator.IndexFromContainer(checkedHexPoint);

            if (checkedHexPoint.Background.Opacity != CheckedHexPointInfo.NOT_CHECKED)
            {
                // przeniesienie jednostki jeśli kliknięto zaznaczony hexpoint terenu lewym przyciskiem myszy
                if (checkedHexPoint.Background.Opacity == CheckedHexPointInfo.TERRAIN_CHECKED && mouseButton == MouseButton.Left)
                {
                    CurrentTeamUnitsToMove.Remove(HexPoints[LastUnitIndex].Unit);
                    HexPoints[index].BindUnitToHex(HexPoints[LastUnitIndex].UnbindUnitFromHex());
                }

                // walka jednostek jeśli kliknięto zaznaczony hexpoint z wrogą jednostką prawym przyciskiem myszy
                else if (checkedHexPoint.Background.Opacity == CheckedHexPointInfo.ATTACK_CHECKED && mouseButton == MouseButton.Right)
                {
                    // atak
                    HexPoints[index] = CombatOfUnits(HexPoints[LastUnitIndex], HexPoints[index], false);

                    // kontratak
                    HexPoints[LastUnitIndex] = CombatOfUnits(HexPoints[index], HexPoints[LastUnitIndex], true);

                    CurrentTeamUnitsToShot.Remove(HexPoints[LastUnitIndex].Unit);
                }

                ResetCheckedElements();
                return;
            }

            ResetCheckedElements();

            // zaznaczenie możliwych ruchów lub ataków
            if (checkedHexPoint.Unit != null 
                && checkedHexPoint.Unit.UnitKind != Unit.UnitInfo.BASE
                && (mouseButton == MouseButton.Left ? CurrentTeamUnitsToMove.Contains(checkedHexPoint.Unit) : CurrentTeamUnitsToShot.Contains(checkedHexPoint.Unit)))
            {
                LastUnitIndex = index;
                IEnumerable<HexPoint> area = GetMovementRange(checkedHexPoint, mouseButton == MouseButton.Left ? checkedHexPoint.Unit.MoveRange : checkedHexPoint.Unit.AttackRange);

                foreach (int i in Enumerable.Range(0, HexPoints.Count))
                {
                    // ruch - hexpoint musi znajdować się w zasięgu i nie może zawierać innej sojuszniczej jednostki
                    if (area.Contains(HexPoints[i])
                        && HexPoints[i].Unit == null 
                        && mouseButton == MouseButton.Left)
                    {
                        HexPoints[i].Background.Opacity = CheckedHexPointInfo.TERRAIN_CHECKED;
                    }

                    // atak - hexpoint musi znajdować się w zasięgu i musi zawierać wrogą jednostkę
                    else if (area.Contains(HexPoints[i]) 
                        && HexPoints[i].Unit != null 
                        && HexPoints[i].Unit.TeamCode != checkedHexPoint.Unit.TeamCode 
                        && mouseButton == MouseButton.Right)
                    {
                        HexPoints[i].Background.Opacity = CheckedHexPointInfo.ATTACK_CHECKED;
                    }
                }
            }
        }

        /**
         * Metoda sprawdza czy nastąpił koniec gry, czyli zniszczenie bazy.
         */
        private void CheckIfGameOver(Unit unit)
        {
            if (unit.UnitKind == UnitInfo.BASE)
            {
                GameOverEvent?.Invoke(this, new GameOverEventArgs(unit.TeamCode == TeamInfo.TEAM_A ? TeamInfo.TEAM_B : TeamInfo.TEAM_A));
            }
        }

        /**
         * Metoda realizująca walkę między dwoma jednostkami.
         * Jeśli nie jest to kontratak, to atak zyskuje losowo bonus 0% - 20% swojej wartości.
         */
        private HexPoint CombatOfUnits(HexPoint attacker, HexPoint defender, bool isCounterattack)
        {
            // wykluczenie sytuacji w której zniszczona jednostka kontratakuje
            if (attacker.Unit == null)
            { 
                return defender; 
            }

            Random random = new Random();
            int attackIndicator, defenseIndicator, damagePoints = 0, counterattackIndicator = isCounterattack == true ? -1 : 1;

            // ustalenie współczynników ataku i obrony jednostek
            defenseIndicator = defender.Unit.DefenceValue;
            switch (defender.Unit.Toughness)
            {
                case Unit.UnitInfo.SOFT:
                    attackIndicator = attacker.Unit.SoftAttackValue;
                    break;

                case Unit.UnitInfo.MEDIUM:
                    attackIndicator = attacker.Unit.MediumAttackValue;
                    break;

                case Unit.UnitInfo.HARD:
                    attackIndicator = attacker.Unit.HardAttackValue;
                    break;

                default:
                    throw new Exception("Brak przypisanej wytrzymałości dla jednostki atakującej!");
            }

            foreach (int i in Enumerable.Range(0, attacker.Unit.Hp))
            {
                int attackValue = random.Next(1, 21) + attackIndicator - defenseIndicator + counterattackIndicator;
                
                // jeśli wartość ataku przekroczyła 15 - strzał celny
                if (attackValue > 15)
                {
                    damagePoints++;
                }
            }

            defender.Unit.Hp = defender.Unit.Hp - damagePoints;
            UnitCombatEvent?.Invoke(this, new UnitCombatEventArgs(attacker.Unit, defender.Unit, damagePoints, isCounterattack));

            // usuń jednostkę z planszy, jeśli straciła całe życie
            // włącz dźwięk wybuchu jednostki
            // sprawdź czy to koniec gry
            if (defender.Unit.Hp <= 0)
            {
                Unit tempUnit = defender.Unit;
                attacker.Unit.PlayExplosionSound(true);
                defender.DisplayExplosion();
                CheckIfGameOver(tempUnit);
            }

            // jeśli jednostka nie jest zniszczona włącz dźwięk strzału atakującej jednostki
            else if (!isCounterattack && damagePoints > 0)
            {
                attacker.Unit.PlayExplosionSound(false);
            }

            return defender;
        }

        /**
         * Metoda zwracająca listę hexpointów zależną od zasięgu ruchu jednostki oraz terenu na jakim znajdować się będzie jednostka
         */
        private IEnumerable<HexPoint> GetMovementRange(HexPoint checkedIntPoint, int moveReamaining)
        {
            // ustalenie listy sąsiadujących hexpointów
            HashSet<HexPoint> area = HexPoints.Where(element => (new HexArrayHelper().GetNeighbours(new IntSize(Board.RowCount, Board.ColumnCount), checkedIntPoint.Point)).Contains(element.Point)).ToHashSet();

            foreach (HexPoint hexPoint in area)
            {
                int terrainModifier = hexPoint.TerrainModifier;
                if (moveReamaining - terrainModifier > 0)
                {
                    area = area.Concat(GetMovementRange(hexPoint, moveReamaining - terrainModifier)).ToHashSet();
                }
            }
            return area;
        }

        /**
         * Metoda resetująca zaznaczony zakres ruchu jednostki
         */
        public void ResetCheckedElements()
        {
            foreach (int i in Enumerable.Range(0, Board.Items.Count))
            {
                HexPoints[i].Background.Opacity = CheckedHexPointInfo.NOT_CHECKED;
            }
        }

        public HexPoint GetHexAt(int index)
        {
            return HexPoints[index];
        }

        public HexPoint GetHexAt(int x, int y)
        {
            return HexPoints[x + y * Board.ColumnCount];
        }
    }
}
