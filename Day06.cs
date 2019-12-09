using System.Collections.Generic;
using System.IO;
using System.Linq;

class Day06_UniversalOrbitMap
{
    public class GravityWell
    {
        public int Id { get; set; }
        public string BodyId { get; set; }
        public string ParentBodyId { get; set; }
        public List<GravityWell> Children { get; set; }
    }

    public static int Part1(string path)
    {
        var systemMap = GetSystemMap(path);
        return systemMap.Keys.Select(w => GetRouteToCOM(w, systemMap).Count).Sum();
    }

    public static int Part2(string path)
    {
        var systemMap = GetSystemMap(path);
        var santaRoute = GetRouteToCOM("SAN", systemMap);
        var myRoute = GetRouteToCOM("YOU", systemMap);

        var commonRoute = santaRoute.Intersect(myRoute).First();

        var jumpsToSanta= santaRoute.IndexOf(commonRoute) + myRoute.IndexOf(commonRoute);

        return jumpsToSanta;
    }

    public static Dictionary<string, GravityWell> GetSystemMap(string path)
    {
        var sytemMap = new Dictionary<string, GravityWell>();
        var input = File.ReadAllLines(path);
        var id = 0;

        foreach (var w in input)
        {
            var bodyId = w.Split(')')[1];
            var parentId = w.Split(')')[0];
            GravityWell gravityWell;
            if (sytemMap.ContainsKey(bodyId))
            {
                gravityWell = sytemMap[bodyId];
            }
            else
            {
                var well = new GravityWell
                {
                    BodyId = bodyId,
                    ParentBodyId = parentId,
                    Id = id++
                };
                sytemMap.Add(bodyId, well);
            }
        }

        return sytemMap;
    }

    public static List<string> GetRouteToCOM(string siteId, Dictionary<string, GravityWell> systemMap)
    {
        var route = new List<string>();

        var currentSiteId = siteId;
        while (currentSiteId != "COM")
        {
            var currentWell = systemMap[currentSiteId];
            currentSiteId = currentWell.ParentBodyId;
            route.Add(currentSiteId);
        }

        return route;
    }
}