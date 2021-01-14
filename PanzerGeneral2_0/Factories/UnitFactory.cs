using PanzerGeneral2_0.Controls.Units;
using System;

namespace PanzerGeneral2_0.Factories
{
    public static class UnitFactory
    {

        public static Unit BuildUnit(string type, Unit.TeamInfo team)
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
            else if (type == "Base")
            {
                return new MilitaryBaseControl(team);
            }
            else
            {
                throw new InvalidOperationException($"Factory does not produce unit called \"{type}\"!");
            }
        }

        public static Unit BuildUnit(Unit.UnitInfo type, Unit.TeamInfo team)
        {
            if (type == Unit.UnitInfo.INFANTRY)
            {
                return new InfantryControl(team);
            }
            else if (type == Unit.UnitInfo.CANNON)
            {
                return new CannonControl(team);
            }
            else if (type == Unit.UnitInfo.TANK)
            {
                return new TankContol(team);
            }
            else if (type == Unit.UnitInfo.BASE)
            {
                return new MilitaryBaseControl(team);
            }
            else
            {
                throw new InvalidOperationException($"Factory does not produce unit called \"{type}\"!");
            }
        }
    }
}
