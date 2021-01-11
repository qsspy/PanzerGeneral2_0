using PanzerGeneral2_0.Controls.Grid.Helpers;
using PanzerGeneral2_0.Controls.Units;
using PanzerGeneral2_0.CustomEventArgs;
using PanzerGeneral2_0.Factories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Input;
using static PanzerGeneral2_0.Controls.Grid.HexPoint;

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

        private readonly List<HexPoint> hexPoints = new List<HexPoint>();

        public int lastUnitChecked;   // jeśli < 0 brak zaznaczonej jednostki, w p.p. indeks pola zaznaczonej jednostki

        public event EventHandler<HexItemEventArgs> HexItemMouseEnterEvent;

        public event EventHandler<UnitCombatEventArgs> UnitCombatEvent;

        public HexBoard()
        {
            InitializeComponent();
           
            this.lastUnitChecked = -1;

            UnitDetailsWindow.Children.Add(new InfantryControl(Unit.TeamInfo.TEAM_A));
            UnitDetailsWindow.Children.Clear();

            DistributeHexesOnBoard();
        }

        /**
         * Metoda obsługująca zdarzenie kliknięcia lewym przyciskiem myszy na hexpoint
         */
        private void HexItem_PreviewMouseButtonDown(object sender, MouseButtonEventArgs e)
        {
            SetUnitAfterClickInteraction((HexPoint)sender, e.ChangedButton);
        }

        private void HexItem_MouseEnter(object sender, MouseEventArgs e)
        {
            if (sender is HexPoint point)
            {
                var index = Board.ItemContainerGenerator.IndexFromContainer(point);
                int x = index / Board.ColumnCount;
                int y = index % Board.ColumnCount;
                HexPointTerrainInfo terrainInfo = hexPoints[index].Terrain;
                HexItemEventArgs args = new HexItemEventArgs(x, y, terrainInfo);
                args.OwnedUnit = hexPoints[index].Unit;

                HexItemMouseEnterEvent?.Invoke(this, args);
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

                    hexPoints.Add(hexPoint);
                }
            }

            Board.ItemsSource = hexPoints;
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
                    hexPoints[index].BindUnitToHex(hexPoints[lastUnitChecked].UnbindUnitFromHex());
                }

                // walka jednostek jeśli kliknięto zaznaczony hexpoint z wrogą jednostką prawym przyciskiem myszy
                else if (checkedHexPoint.Background.Opacity == CheckedHexPointInfo.ATTACK_CHECKED && mouseButton == MouseButton.Right)
                {
                    // atak
                    hexPoints[index] = CombatOfUnits(hexPoints[lastUnitChecked], hexPoints[index], false);

                    // kontratak
                    hexPoints[lastUnitChecked] = CombatOfUnits(hexPoints[index], hexPoints[lastUnitChecked], true);
                }

                ResetCheckedElements();
                return;
            }

            ResetCheckedElements();

            // zaznaczenie możliwych ruchów lub ataków
            if (checkedHexPoint.Unit != null && checkedHexPoint.Unit.UnitKind != Unit.UnitInfo.MILITARY_BASE)
            {
                lastUnitChecked = index;
                IEnumerable<HexPoint> area = GetMovementRange(checkedHexPoint, mouseButton == MouseButton.Left ? checkedHexPoint.Unit.MoveRange : checkedHexPoint.Unit.AttackRange);

                foreach (int i in Enumerable.Range(0, hexPoints.Count))
                {
                    // ruch - hexpoint musi znajdować się w zasięgu i nie może zawierać innej sojuszniczej jednostki
                    if (area.Contains(hexPoints[i]) && hexPoints[i].Unit == null && mouseButton == MouseButton.Left)
                    {
                        hexPoints[i].Background.Opacity = CheckedHexPointInfo.TERRAIN_CHECKED;
                    }

                    // atak - hexpoint musi znajdować się w zasięgu i musi zawierać wrogą jednostkę
                    else if (area.Contains(hexPoints[i]) && hexPoints[i].Unit != null && hexPoints[i].Unit.TeamCode != checkedHexPoint.Unit.TeamCode && mouseButton == MouseButton.Right)
                    {
                        hexPoints[i].Background.Opacity = CheckedHexPointInfo.ATTACK_CHECKED;
                    }
                }
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
                
                // jeśli wartość ataku przekroczyła 15 - zniszcz jedną jednostkę
                if (attackValue > 15)
                {
                    damagePoints++;
                }
            }

            defender.Unit.Hp = defender.Unit.Hp - damagePoints;
            UnitCombatEvent?.Invoke(this, new UnitCombatEventArgs(attacker.Unit, defender.Unit, damagePoints, isCounterattack));

            // usuń jednostkę z planszy, jeśli straciła całe życie
            if (defender.Unit.Hp <= 0)
            {
                defender.UnbindUnitFromHex();
            }

            return defender;
        }

        /**
         * Metoda zwracająca listę hexpointów zależną od zasięgu ruchu jednostki oraz terenu na jakim znajdować się będzie jednostka
         */
        private IEnumerable<HexPoint> GetMovementRange(HexPoint checkedIntPoint, int moveReamaining)
        {
            // ustalenie listy sąsiadujących hexpointów
            HashSet<HexPoint> area = hexPoints.Where(element => (new HexArrayHelper().GetNeighbours(new IntSize(Board.RowCount, Board.ColumnCount),checkedIntPoint.Point)).Contains(element.Point)).ToHashSet();

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
        private void ResetCheckedElements()
        {
            foreach (int i in Enumerable.Range(0, Board.Items.Count))
            {
                hexPoints[i].Background.Opacity = CheckedHexPointInfo.NOT_CHECKED;
            }
        }

        public HexPoint GetHexAt(int index)
        {
            return hexPoints[index];
        }
    }
}
