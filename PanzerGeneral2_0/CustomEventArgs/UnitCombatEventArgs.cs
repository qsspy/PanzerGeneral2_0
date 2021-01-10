using PanzerGeneral2_0.Controls.Units;
using System;

namespace PanzerGeneral2_0.CustomEventArgs
{
    public class UnitCombatEventArgs : EventArgs
    {
        public Unit Attacker { get; set; }
        public Unit Defender { get; set; }
        public double DamagePoints { get; set; }
        public bool IsCounterattack { get; set; }

        public UnitCombatEventArgs(Unit attacker, Unit defender, double damagePoints, bool isCounterattack)
        {
            Attacker = attacker;
            Defender = defender;
            DamagePoints = damagePoints;
            IsCounterattack = isCounterattack;
        }
    }
}
