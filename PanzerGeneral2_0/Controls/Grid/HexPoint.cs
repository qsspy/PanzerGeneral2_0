using System;

namespace PanzerGeneral2_0.Controls.Grid
{
    public struct HexPoint : IEquatable<HexPoint>
    {
        public enum HexPointTerrainInfo
        {
            PLAIN,
            FOREST,
            MOUNTAINS
        }

        public int X { get; }
        public int Y { get; }

        public string ImageSource { get; }

        public HexPoint(int x, int y)
        {
            this.X = x;
            this.Y = y;
            this.ImageSource = "/PanzerGeneral2_0;component/Resources/plain.png";
        }

        public static bool operator ==(HexPoint a, HexPoint b)
        {
            return a.X == b.X && a.Y == b.Y;
        }

        public static bool operator !=(HexPoint a, HexPoint b)
        {
            return !(a == b);
        }

        public bool Equals(HexPoint other)
        {
            return this == other;
        }
    }
}
