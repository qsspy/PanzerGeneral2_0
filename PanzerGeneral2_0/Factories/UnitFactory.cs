using PanzerGeneral2_0.Controls.Units;
using System;

namespace PanzerGeneral2_0.Factories
{
    public static class UnitFactory
    {

        public static Unit buildUnit(string type, Unit.TeamInfo team)
        {
            if(type == "Infantry")
            {
                return new InfantryControl(team);
            }
            else if(type == "Cannon")
            {
                return new CannonControl(team);
            }
            else if(type == "Tank")
            {
                return new TankContol(team);
            }
            else
            {
                throw new InvalidOperationException("Factory does not produce unit called \"" + type + "\"!");
            }
        }
    }
}
