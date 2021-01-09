using PanzerGeneral2_0.Controls.Units;
using System;

namespace PanzerGeneral2_0.CustomEventArgs
{
    public class UnitCombatEventArgs : EventArgs
    {

        public Unit Attacker { get; set; }
        public Unit Defender { get; set; }
        public double DamagePoints { get; set; }

        public UnitCombatEventArgs(Unit attacker, Unit defender, double damagePoints)
        {
            Attacker = attacker;
            Defender = defender;
            DamagePoints = damagePoints;
        }
    }
}
