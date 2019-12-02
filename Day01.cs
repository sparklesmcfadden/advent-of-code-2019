using System;
using System.Collections.Generic;
using System.Linq;

class Day01_RocketEquation
{   
    class Rocket
    {
        public List<RocketModule> Modules { get; set; }
        public int TotalFuelRequirement 
        {
            get { return Modules.Sum(x => x.FuelRequirement); }
        }
        public int TotalMass
        {
            get { return Modules.Sum(x => x.Mass); }
        }
    }

    class RocketModule
    {
        public int Mass { get; set; }
        public int FuelRequirement
        {
            get { return CalculateFuelRequirement(this.Mass); }
        }

        static int CalculateFuelRequirement(int mass)
        {
            return (int)Math.Floor((double)mass/3) - 2;
        }
    }
}