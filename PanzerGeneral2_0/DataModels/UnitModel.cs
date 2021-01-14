using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static PanzerGeneral2_0.Controls.Units.Unit;

namespace PanzerGeneral2_0.DataModels
{
    class UnitModel
    {
        public int Id { get; set; }
        public TeamInfo TeamCode { get; set; }
        public UnitInfo UnitKind { get; set; }
        public int XPosition { get; set; }
        public int YPosition { get; set; }
        public int Hp { get; set; }
    }
}
