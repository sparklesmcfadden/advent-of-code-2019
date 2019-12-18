using System.Linq;
using System.IO;
using System.Collections.Generic;
using System;

class Day14
{
    public List<Element> _allElements { get; set; } = new List<Element>();

    public class Element
    {
        public string ElementName { get; set; }
        public Dictionary<Dictionary<string, int>, int> InputElements { get; set; }
        public bool isOre { get; set; }
        public bool isFuel { get; set; }
    }

    private Element FindOrCreateElement(string elementName)
    {
        var element = _allElements.FirstOrDefault(e => e.ElementName == elementName);
        if (element == null)
        {
            element = new Element
            {
                ElementName = elementName,
                InputElements = new Dictionary<Dictionary<string, int>, int>(),
                isOre = elementName == "ORE" ? true : false,
                isFuel = elementName == "FUEL" ? true : false,
            };
            _allElements.Add(element);
        }
        return element;
    }

    public void LoadReactions()
    {
        var inputFile = File.ReadAllLines("Data/Day14_Input.txt");
        foreach (var line in inputFile)
        {
            var inputs = line.Split(" => ")[0].Split(", ");
            var output = line.Split(" => ")[1];
            var elementName = output.Split(' ')[1];
            var outAmount = Convert.ToInt32(output.Split(' ')[0]);

            var element = FindOrCreateElement(elementName);

            var reactionInputs = new Dictionary<string, int>();
            foreach (var input in inputs)
            {
                var inElementName = input.Split(' ')[1];
                var inAmount = Convert.ToInt32(input.Split(' ')[0]);
                reactionInputs.Add(inElementName, inAmount);
            }
            element.InputElements.Add(reactionInputs, outAmount);
        }
    }

    public void ComputeOreNeeded()
    {

    }
}