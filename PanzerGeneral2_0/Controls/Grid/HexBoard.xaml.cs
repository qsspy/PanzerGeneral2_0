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

        private List<HexPoint> hexPoints = new List<HexPoint>();

        public int lastUnitChecked { get; set; }    // jeśli < 0 brak zaznaczonej jednostki, w p.p. indeks pola zaznaczonej jednostki

        public event EventHandler<HexItemEventArgs> HexItemMouseEnterEvent;

        public event EventHandler<UnitCombatEventArgs> UnitCombatEvent;

        public HexBoard()
        {
            this.lastUnitChecked = -1;
            InitializeComponent();

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
            if (sender is HexPoint)
            {
                var index = Board.ItemContainerGenerator.IndexFromContainer((HexPoint)sender);
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
                    var bgPath = "";
                    var terrainModifier = 0;
                    HexPointTerrainInfo terrain;

                    // TODO - Mozna zastąpić fabryką
                    switch (Int32.Parse(terrainLine[j]))
                    {
                        case 0:
                            bgPath = "/PanzerGeneral2_0;component/Resources/plain.png";
                            terrain = HexPointTerrainInfo.PLAIN;
                            terrainModifier = 1;
                            break;
                        case 1:
                            bgPath = "/PanzerGeneral2_0;component/Resources/forest.png";
                            terrain = HexPointTerrainInfo.FOREST;
                            terrainModifier = 2;
                            break;
                        default:
                            bgPath = "/PanzerGeneral2_0;component/Resources/mountains.png";
                            terrain = HexPointTerrainInfo.MOUNTAINS;
                            terrainModifier = 3;
                            break;
                    }

                    var hexPoint = new HexPoint(new IntPoint(j, i), bgPath, terrainModifier);
                    hexPoint.Terrain = terrain;

                    switch (Int32.Parse(unitLine[j]))
                    {
                        case 1:
                            hexPoint.bindUnitToHex(UnitFactory.buildUnit("Infantry", Unit.TeamInfo.TEAM_A));
                            break;
                        case 2:
                            hexPoint.bindUnitToHex(UnitFactory.buildUnit("Cannon", Unit.TeamInfo.TEAM_A));
                            break;
                        case 3:
                            hexPoint.bindUnitToHex(UnitFactory.buildUnit("Tank", Unit.TeamInfo.TEAM_A));
                            break;
                        case 4:
                            hexPoint.bindUnitToHex(UnitFactory.buildUnit("Base", Unit.TeamInfo.TEAM_A));
                            break;
                        case 5:
                            hexPoint.bindUnitToHex(UnitFactory.buildUnit("Infantry", Unit.TeamInfo.TEAM_B));
                            break;
                        case 6:
                            hexPoint.bindUnitToHex(UnitFactory.buildUnit("Cannon", Unit.TeamInfo.TEAM_B));
                            break;
                        case 7:
                            hexPoint.bindUnitToHex(UnitFactory.buildUnit("Tank", Unit.TeamInfo.TEAM_B));
                            break;
                        case 8:
                            hexPoint.bindUnitToHex(UnitFactory.buildUnit("Base", Unit.TeamInfo.TEAM_B));
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
                    hexPoints[index].bindUnitToHex(hexPoints[lastUnitChecked].unbindUnitFromHex());
                }

                // walka jednostek jeśli kliknięto zaznaczony hexpoint z wrogą jednostką prawym przyciskiem myszy
                else if (checkedHexPoint.Background.Opacity == CheckedHexPointInfo.ATTACK_CHECKED && mouseButton == MouseButton.Right)
                {
                    hexPoints[index] = CombatOfUnits(hexPoints[lastUnitChecked], hexPoints[index]);
                    // TODO: data binding nie działa - nie zmienia się wyświetlanie życia na pasku jednostki
                }

                resetCheckedElements();
                return;
            }

            resetCheckedElements();

            // zaznaczenie możliwych ruchów lub ataków
            if (checkedHexPoint.Unit != null)
            {
                lastUnitChecked = index;
                IEnumerable<HexPoint> area = GetIntPointsByRangeAndType(checkedHexPoint, mouseButton == MouseButton.Left ? checkedHexPoint.Unit.MoveRange : checkedHexPoint.Unit.AttackRange);

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
         * Metoda realizująca walkę między dwoma jednostkami
         */
        private HexPoint CombatOfUnits(HexPoint attacker, HexPoint defender)
        {
            var attackIndicator = 0;
            var defenseIndicator = 0;

            switch (attacker.Unit.Toughness)
            {
                case Unit.UnitInfo.SOFT:
                    attackIndicator = attacker.Unit.SoftAttackValue;
                    defenseIndicator = defender.Unit.SoftDefenceValue;
                    break;

                case Unit.UnitInfo.MEDIUM:
                    attackIndicator = attacker.Unit.MediumAttackValue;
                    defenseIndicator = defender.Unit.MediumDefenceValue;
                    break;

                case Unit.UnitInfo.HARD:
                    attackIndicator = attacker.Unit.HardAttackValue;
                    defenseIndicator = defender.Unit.HardDefenceValue;
                    break;

                default:
                    throw new Exception("Brak przypisanej wytrzymałości dla jednostki atakującej!");
            }

            // jeśli współczynnik obrony jest większy niż współczynnik ataku - ustaw punkty obrażeń na 0
            var damagePoints = attackIndicator - defenseIndicator < 0 ? 0 : attackIndicator - defenseIndicator;
            defender.Unit.Hp = defender.Unit.Hp - damagePoints;

            UnitCombatEvent?.Invoke(this, new UnitCombatEventArgs(attacker.Unit, defender.Unit, damagePoints));

            return defender;
        }

        /**
         * Metoda zwracająca listę hexpointów zależną od zasięgu ruchu jednostki oraz terenu na jakim znajdować się będzie jednostka
         */
        private IEnumerable<HexPoint> GetIntPointsByRangeAndType(HexPoint checkedIntPoint, int moveReamaining)
        {
            // ustalenie listy sąsiadujących hexpointów
            HashSet<HexPoint> area = hexPoints.Where(element => (new HexArrayHelper().GetNeighbours(new IntSize(Board.RowCount, Board.ColumnCount),checkedIntPoint.Point)).Contains(element.Point)).ToHashSet();

            foreach (HexPoint hexPoint in area)
            {
                var terrainModifier = hexPoint.TerrainModifier;
                if (moveReamaining - terrainModifier > 0)
                {
                    area = area.Concat(GetIntPointsByRangeAndType(hexPoint, moveReamaining - terrainModifier)).ToHashSet();
                }
            }

            return area;
        }

        /**
         * Metoda resetująca zaznaczony zakres ruchu jednostki
         */
        private void resetCheckedElements()
        {
            foreach (int i in Enumerable.Range(0, Board.Items.Count))
            {
                hexPoints[i].Background.Opacity = CheckedHexPointInfo.NOT_CHECKED;
            }
        }

        public HexPoint getHexAt(int index)
        {
            return hexPoints[index];
        }
    }
}
