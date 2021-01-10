using PanzerGeneral2_0.Controls.Grid;
using System;

namespace PanzerGeneral2_0.Factories
{
    public static class HexPointFactory
    {

        public static HexPoint BuildHexPoint(string type, IntPoint point)
        {
            switch (type)
            {
                case "Plain":
                    return new HexPoint(point, "/PanzerGeneral2_0;component/Resources/plain.png", HexPoint.HexPointTerrainInfo.PLAIN, 1);

                case "Forest":
                    return new HexPoint(point, "/PanzerGeneral2_0;component/Resources/forest.png", HexPoint.HexPointTerrainInfo.PLAIN, 2);

                case "Mountains":
                    return new HexPoint(point, "/PanzerGeneral2_0;component/Resources/mountains.png", HexPoint.HexPointTerrainInfo.MOUNTAINS, 3);

                default:
                    throw new InvalidOperationException($"Factory does not produce unit called \"{type}\"!");
            }
        }
    }
}
