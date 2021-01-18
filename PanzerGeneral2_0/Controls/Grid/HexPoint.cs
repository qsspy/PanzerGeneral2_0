using HexGridControl;
using PanzerGeneral2_0.Controls.Other;
using PanzerGeneral2_0.Controls.Units;
using System.ComponentModel;
using System.Media;
using System.Threading;
using static PanzerGeneral2_0.Controls.Units.Unit;

namespace PanzerGeneral2_0.Controls.Grid
{
    public class HexPoint : HexItem, INotifyPropertyChanged
    {
        public enum HexPointTerrainInfo
        {
            PLAIN,
            FOREST,
            MOUNTAINS
        }

        public event PropertyChangedEventHandler PropertyChanged;

        Unit _unit;
        public Unit Unit 
        {
            get { return _unit; }
            set
            {
                _unit = value;
                NotifyPropertyChanged("Unit");
            }
        }

        public HexPointTerrainInfo Terrain { get; set; }
        public string ImageSource { get; set; }
        public int TerrainModifier { get; set; }
        public IntPoint Point { get; set; }

        public HexPoint(IntPoint point, string imageSource, HexPointTerrainInfo terrain, int terrainModifier) 
        {
            this.DataContext = this;
            this.Point = point;
            this.ImageSource = imageSource;
            this.Terrain = terrain;
            this.TerrainModifier = terrainModifier;
        }

        /**
         Przydziela podaną jednostkę do HexItem'a
         */
        public void BindUnitToHex(Unit unit)
        {
            this.Unit = unit;
            this.Unit.Height = Unit.DEFAULT_UNIT_HEIGHT;
            this.Unit.Width = Unit.DEFAULT_UNIT_WIDTH;
        }

        /**
         Odbiera jednostkę HexItem'owi
         */
        public Unit UnbindUnitFromHex()
        {
            Unit tempUnit = this.Unit;
            this.Unit = null;
            return tempUnit;
        }

        private void NotifyPropertyChanged(string v)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(v));
        }
    }
}
