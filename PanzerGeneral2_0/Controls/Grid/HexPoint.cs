using System;

namespace PanzerGeneral2_0.Controls.Grid
{
    public class HexPoint
    {
        public enum HexPointTerrainInfo
        {
            PLAIN,
            FOREST,
            MOUNTAINS
        }

        public HexPointTerrainInfo Terrain { get; set; }
        public string ImageSource { get; set; }
        public IntPoint Point { get; set; }

        public HexPoint(IntPoint point, string imageSource) 
        {
            this.Point = point;
            this.ImageSource = imageSource;
        }
    }
}
