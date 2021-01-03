using PanzerGeneral2_0.Controls.Grid.Helpers;
using PanzerGeneral2_0.Controls.Units;
using PanzerGeneral2_0.Factories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;

namespace PanzerGeneral2_0.Controls.Grid
{
    public static class CheckedHexPointInfo
    {
        public const double CHECKED = 0.5;
        public const double NOT_CHECKED = 1;
    }

    /// <summary>
    /// Logika interakcji dla klasy HexBoard.xaml
    /// </summary>
    public partial class HexBoard : UserControl
    {

        private List<HexPoint> hexPoints = new List<HexPoint>();
        private int lastUnitChecked;    // jeśli < 0 brak zaznaczonej jednostki, w p.p. indeks pola zaznaczonej jednostki

        public HexBoard()
        {
            this.lastUnitChecked = -1;
            InitializeComponent();

            DistributeHexesOnBoard();
        }

        /**
         * Metoda obsługująca zdarzenie kliknięcia lewym przyciskiem myszy na hexpoint
         */
        private void HexItem_PreviewMouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            SetCheckedHexPointArea((HexPoint)sender);
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

                    // TODO - Mozna zastąpić fabryką
                    switch (Int32.Parse(terrainLine[j]))
                    {
                        case 0:
                            bgPath = "/PanzerGeneral2_0;component/Resources/plain.png";
                            break;
                        case 1:
                            bgPath = "/PanzerGeneral2_0;component/Resources/forest.png";
                            break;
                        default:
                            bgPath = "/PanzerGeneral2_0;component/Resources/mountains.png";
                            break;
                    }

                    var hexPoint = new HexPoint(new IntPoint(j, i), bgPath);

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
                            hexPoint.bindUnitToHex(UnitFactory.buildUnit("Infantry", Unit.TeamInfo.TEAM_B));
                            break;
                        case 5:
                            hexPoint.bindUnitToHex(UnitFactory.buildUnit("Cannon", Unit.TeamInfo.TEAM_B));
                            break;
                        case 6:
                            hexPoint.bindUnitToHex(UnitFactory.buildUnit("Tank", Unit.TeamInfo.TEAM_B));
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
         * Metoda zaznaczająca na Board hexpointy, na które można przesunąć jednostkę
         */
        private void SetCheckedHexPointArea(HexPoint checkedHexPoint)
        {
            var index = Board.ItemContainerGenerator.IndexFromContainer(checkedHexPoint);

            // przeniesienie jednostki jeśli kliknięto zacieniony hexpoint
            if (checkedHexPoint.Background.Opacity == CheckedHexPointInfo.CHECKED)
            {
                hexPoints[index].bindUnitToHex(hexPoints[lastUnitChecked].unbindUnitFromHex());
                resetCheckedElements();
                return;
            }

            resetCheckedElements();

            // zaznaczenie możliwego zakresu ruchu
            if (checkedHexPoint.Unit != null)
            {
                lastUnitChecked = index;

                //funkcja sprawdzająca istnienie hexpointa w zakresie zaznaczonej jednostki
                Func<IntPoint, bool> selector = delegate (IntPoint hexIntPoint)
                {
                    int unitMoveRange = checkedHexPoint.Unit.MoveRange;
                    int unitX = checkedHexPoint.Point.X;
                    int unitY = checkedHexPoint.Point.Y;

                    int range = (int)(Math.Sqrt(Math.Pow(hexIntPoint.X - unitX, 2) + Math.Pow(hexIntPoint.Y - unitY, 2)) + 0.5);
                    if (range <= unitMoveRange)
                    {
                        return true;
                    }
                    return false;
                };

                //przygotowanie listy hexpointów, które należą do zakresu ruchu jednostki
                IEnumerable<IntPoint> area = new HexArrayHelper().GetArea(
                    new IntSize(Board.RowCount, Board.ColumnCount),
                    checkedHexPoint.Point,
                    selector);

                // TODO: zoptymalizować
                //IEnumerable<IntPoint> area = new HexArrayHelper().GetNeighbours(
                //            new IntSize(Board.RowCount, Board.ColumnCount),
                //            checkedHexPoint.Point);
                //IEnumerable<IntPoint> tempArea = new List<IntPoint>();
                //foreach (int i in Enumerable.Range(0, checkedHexPoint.Unit.MoveRange))
                //{               
                //    foreach (IntPoint intPoint in area)
                //    {
                //        tempArea = tempArea.Concat(new HexArrayHelper().GetNeighbours(
                //            new IntSize(Board.RowCount, Board.ColumnCount),
                //            intPoint));
                //    }
                //    area = area.Concat(tempArea);
                //    area.Distinct().ToDictionary(point => new IntPoint(point.X, point.Y));
                //}

                foreach (int i in Enumerable.Range(0, hexPoints.Count))
                {
                    // hexpoint musi znajdować się w zasięgu i nie może zawierać innej sojuszniczej jednostki
                    if (area.Contains(hexPoints[i].Point) && hexPoints[i].Unit == null)
                    {
                        hexPoints[i].Background.Opacity = CheckedHexPointInfo.CHECKED;
                    }
                }
            }
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
    }
}
