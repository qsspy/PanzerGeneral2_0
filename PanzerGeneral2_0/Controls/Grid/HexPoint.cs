using HexGridControl;
using PanzerGeneral2_0.Controls.Units;

namespace PanzerGeneral2_0.Controls.Grid
{
    public class HexPoint : HexItem
    {

        public enum HexPointTerrainInfo
        {
            PLAIN,
            FOREST,
            MOUNTAINS
        }

        public Unit Unit { get; set; }
        public HexPointTerrainInfo Terrain { get; set; }
        public string ImageSource { get; set; }
        public IntPoint Point { get; set; }

        public HexPoint(IntPoint point, string imageSource) 
        {
            this.DataContext = this;
            this.Point = point;
            this.ImageSource = imageSource;
        }


        /**
         Przydziela podaną jednostkę do HexItem'a
         */
        public void bindUnitToHex(Unit unit)
        {
            this.Unit = unit;
            this.Content = unit;
            this.Unit.Height = Unit.DEFAULT_UNIT_HEIGHT;
            this.Unit.Width = Unit.DEFAULT_UNIT_WIDTH;
        }

        /**
         Odbiera jednostkę HexItem'owi
         */
        public Unit unbindUnitFromHex()
        {
            Unit tempUnit = this.Unit;
            this.Unit = null;
            this.Content = null;
            return tempUnit;
        }
    }
}
