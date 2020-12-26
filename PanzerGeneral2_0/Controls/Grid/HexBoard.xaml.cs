using System.Linq;
using System.Windows.Controls;

namespace PanzerGeneral2_0.Controls.Grid
{
    /// <summary>
    /// Logika interakcji dla klasy HexBoard.xaml
    /// </summary>
    public partial class HexBoard : UserControl
    {
        public HexBoard()
        {
            InitializeComponent();
            Board.ItemsSource =
                Enumerable.Range(0, Board.RowCount)
                    .SelectMany(r => Enumerable.Range(0, Board.ColumnCount)
                        .Select(c => new IntPoint(c, r)))
                    .ToList();
        }
    }
}
