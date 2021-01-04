using PanzerGeneral2_0.Controls.Units;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static PanzerGeneral2_0.Controls.Grid.HexPoint;

namespace PanzerGeneral2_0.CustomEventArgs
{
    public class HexItemEventArgs : EventArgs
    { 

        public int HexXPos { get; set; }
        public int HexYPos { get; set; }
        public HexPointTerrainInfo TerrainInfo { get; set; }

        public Unit OwnedUnit { get; set; }

        public HexItemEventArgs(int hexXPos, int hexYPos, HexPointTerrainInfo terrainInfo)
        {
            HexXPos = hexXPos;
            HexYPos = hexYPos;
            TerrainInfo = terrainInfo;
        }
    }
}
