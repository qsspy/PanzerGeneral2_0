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
        private void HexItem_PreviewMouseButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            SetUnitAfterClickInteraction((HexPoint) sender, e.ChangedButton);
        }

        private void HexItem_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
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
                    HexPointTerrainInfo terrain;

                    // TODO - Mozna zastąpić fabryką
                    switch (Int32.Parse(terrainLine[j]))
                    {
                        case 0:
                            bgPath = "/PanzerGeneral2_0;component/Resources/plain.png";
                            terrain = HexPointTerrainInfo.PLAIN;
                            break;
                        case 1:
                            bgPath = "/PanzerGeneral2_0;component/Resources/forest.png";
                            terrain = HexPointTerrainInfo.FOREST;
                            break;
                        default:
                            bgPath = "/PanzerGeneral2_0;component/Resources/mountains.png";
                            terrain = HexPointTerrainInfo.MOUNTAINS;
                            break;
                    }

                    var hexPoint = new HexPoint(new IntPoint(j, i), bgPath);
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
                    CombatOfUnits(hexPoints[lastUnitChecked], hexPoints[index]);
                }

                resetCheckedElements();
                return;
            }      

            resetCheckedElements();

            // zaznaczenie możliwych ruchów lub ataków
            if (checkedHexPoint.Unit != null)
            {
                lastUnitChecked = index;
                IEnumerable<IntPoint> area = GetHexPointsByRangeAndType(checkedHexPoint, mouseButton == MouseButton.Left ? checkedHexPoint.Unit.MoveRange : checkedHexPoint.Unit.AttackRange);             

                foreach (int i in Enumerable.Range(0, hexPoints.Count))
                {
                    // ruch - hexpoint musi znajdować się w zasięgu i nie może zawierać innej sojuszniczej jednostki
                    if (area.Contains(hexPoints[i].Point) && hexPoints[i].Unit == null && mouseButton == MouseButton.Left)
                    {
                        hexPoints[i].Background.Opacity = CheckedHexPointInfo.TERRAIN_CHECKED;
                    }

                    // atak - hexpoint musi znajdować się w zasięgu i musi zawierać wrogą jednostkę
                    else if (area.Contains(hexPoints[i].Point) && hexPoints[i].Unit != null && hexPoints[i].Unit.TeamCode != checkedHexPoint.Unit.TeamCode && mouseButton == MouseButton.Right)
                    {
                        hexPoints[i].Background.Opacity = CheckedHexPointInfo.ATTACK_CHECKED;
                    }
                }
            }
        }

        /**
         * Metoda realizująca walkę między dwoma jednostkami
         */
        private void CombatOfUnits(HexPoint attacker, HexPoint victim)
        {
            throw new NotImplementedException();
        }

        /**
         * Metoda zwracająca listę hexpointów dla przekazanego hexpointa i typu interakcji
         */
        private IEnumerable<IntPoint> GetHexPointsByRangeAndType(HexPoint checkedHexPoint, int range)
        {
            /* WARIANT 1 */
            ////funkcja sprawdzająca istnienie hexpointa w zakresie zaznaczonej jednostki
            //Func<IntPoint, bool> selector = delegate (IntPoint hexIntPoint)
            //{
            //    int unitMoveRange = checkedHexPoint.Unit.MoveRange;
            //    int unitX = checkedHexPoint.Point.X;
            //    int unitY = checkedHexPoint.Point.Y;

            //    //int x = Math.Abs(unitX - hexIntPoint.X);
            //    //int y = Math.Abs(unitY - hexIntPoint.Y);
            //    //int radius = (int)(Math.Sqrt(Math.Pow(x, 2) + Math.Pow(y, 2)));

            //    int radius = (int)(Math.Sqrt(Math.Pow(hexIntPoint.X - unitX, 2) + Math.Pow(hexIntPoint.Y - unitY, 2)));

            //    if (radius <= unitMoveRange)
            //    {
            //        return true;
            //    }
            //    return false;
            //};
            ////przygotowanie listy hexpointów, które należą do zakresu ruchu jednostki
            //IEnumerable<IntPoint> area = new HexArrayHelper().GetArea(
            //    new IntSize(Board.RowCount, Board.ColumnCount),
            //    checkedHexPoint.Point,
            //    selector);


            /* WARIANT 2 */
            // TODO: zoptymalizować
            HexArrayHelper arrayHelper = new HexArrayHelper();
            IEnumerable<IntPoint> area = arrayHelper.GetNeighbours(
                            new IntSize(Board.RowCount, Board.ColumnCount),
                            checkedHexPoint.Point);
            IEnumerable<IntPoint> tempArea = new List<IntPoint>();

            foreach (int i in Enumerable.Range(0, range - 1))
            {
                foreach (IntPoint intPoint in area)
                {
                    tempArea = tempArea.Concat(arrayHelper.GetNeighbours(
                        new IntSize(Board.RowCount, Board.ColumnCount),
                        intPoint));
                }
                area = area.Concat(tempArea);
                area.Distinct().ToDictionary(point => new IntPoint(point.X, point.Y));
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
