using System;
using System.Collections.Generic;
using System.Linq;

class Day01_RocketEquation
{   
    public static Rocket CreateRocket(List<int> masses)
    {
        var rocket = new Rocket { Modules = new List<RocketModule>() };
        foreach (var mass in masses)
        {
            var newModule = new RocketModule { Mass = mass };
            rocket.Modules.Add(newModule);
        }

        return rocket;
    }

    public class Rocket
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

    public class RocketModule
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