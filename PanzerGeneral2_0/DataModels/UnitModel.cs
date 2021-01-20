using System.ComponentModel.DataAnnotations;
using static PanzerGeneral2_0.Controls.Units.Unit;

namespace PanzerGeneral2_0.DataModels
{
    public class UnitModel
    {
        [Key]
        public int Id { get; set; }
        public TeamInfo TeamCode { get; set; }
        public UnitInfo UnitKind { get; set; }
        public int XPosition { get; set; }
        public int YPosition { get; set; }
        public int Hp { get; set; }
        public bool CanMove { get; set; }
        public bool CanAttack { get; set; }
    }
}
