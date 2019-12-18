using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace AdventOfCode
{
    class Program
    {
        static void Main(string[] args)
        {
            // Day01(); // 5148424
            // Day02(); // 5098658/5064
            // Day03(); // 227/20286
            // Day04(); // 2081/1411
            // Day05(); // 6731945/9571668
            // Day06(); // 194721/316
            // Day07(); // 398674/39431233
            // Day08(); // 1703/HCFGE
            // Day09(); // 3100786347/87023
            // Day11(); // 1885/BFEAGHAF
            // Day12(); // 12490/392733896255168
            // Day13(); // 341
            // Day16();
        }

        static void Day16()
        {
            var day16 = new Day16("Data/Day16_Input.txt");
            var part1Result = day16.ProcessPhases(10);
            Console.WriteLine($"Day 16: Part 1: Test Output: {String.Join("", part1Result.Take(8))}");
            // var result = day16.ProcessSignal(100, 100);
        }

        static void Day13()
        {
            var day13 = new Day13_CarePackage();
            // var blockCount = day13.GetBlockCount();
            // Console.WriteLine($"Day 13; Part 1: Block count: {blockCount}");
            day13 = new Day13_CarePackage();
            day13.PlayGame();
        }

        static void Day12()
        {
            var day12 = new Day12();
            day12.Part1("Data/Day12_Input.txt", 1000);
            day12.Part2("Data/Day12_Input.txt");
        }

        static void Day11()
        {
            var day11Part1 = new Day11_SpacePolice("Data/Day11_Input.txt");
            var part1Panels = day11Part1.PaintSpaceship(new Queue<long>());
            var paintedPanels = part1Panels.GroupBy(p => new {p.X, p.Y}).Count();
            Console.WriteLine($"Day 11; Part 1: Panels painted: {paintedPanels}");
            var day11Part2 = new Day11_SpacePolice("Data/Day11_Input.txt");

            var input = new Queue<long>();
            input.Enqueue(1);
            var part2Panels = day11Part2.PaintSpaceship(input);
            day11Part2.PaintRegCode(part2Panels);
        }

        static void Day09()
        {
            var program = Utilities.LoadProgram("Data/Day09_Input.txt");
            var processor = new IntCodeComputer(program, 1, false);
            var input = new Queue<long>();
            input.Enqueue(1);
            processor.RunProgram(input);
            Console.WriteLine($"Day 09: Part 1: BOOST keycode: {processor.OutputString}");
            var part2Processor = new IntCodeComputer(program, 1, false);
            input = new Queue<long>();
            input.Enqueue(2);
            part2Processor.RunProgram(input);
            Console.WriteLine($"Day 09: Part 2: Distress Call Coordinates: {part2Processor.OutputString}");
        }

        static void Day08()
        {
            var day8 = new Day08_SpaceImageFormat();
            var imageData = Utilities.LoadFile("Data/Day08_Input.txt");
            day8.LoadData(imageData, 25, 6);
            var part1Result = day8.FindPart1Layer();
            Console.WriteLine($"Day 08: Part 1: Result: {part1Result}"); // 1703
            day8.RenderImage();
        }

        static void Day07()
        {
            var day7 = new Day07_AmplificationCircuit("Data/Day07_Input.txt");
            var maxOutput = day7.FindHighestOutput();
            Console.WriteLine($"Day 07: Part1: Highest Thruster Output: {maxOutput}"); // 398674
            var day7Part2Test = new Day07_Part2(Utilities.LoadProgram("Data/Day07_Input.txt"));
            var loopOutput = day7Part2Test.GetHighestLoopOutput();
            Console.WriteLine($"Day 07: Part2: Highest Thruster Loop Output: {loopOutput}"); // 39431233
        }

        static void Day06()
        {
            var orbitCount = Day06_UniversalOrbitMap.Part1("systemMap.txt");
            Console.WriteLine($"Day 06: Part1: Total Orbits: {orbitCount}");
            var toSanta = Day06_UniversalOrbitMap.Part2("systemMap.txt");
            Console.WriteLine($"Day 06: Part2: Transfers to Santa: {toSanta}");
        }

        static void Day05()
        {
            var program = "3,225,1,225,6,6,1100,1,238,225,104,0,101,14,135,224,101,-69,224,224,4,224,1002,223,8,223,101,3,224,224,1,224,223,223,102,90,169,224,1001,224,-4590,224,4,224,1002,223,8,223,1001,224,1,224,1,224,223,223,1102,90,45,224,1001,224,-4050,224,4,224,102,8,223,223,101,5,224,224,1,224,223,223,1001,144,32,224,101,-72,224,224,4,224,102,8,223,223,101,3,224,224,1,223,224,223,1102,36,93,225,1101,88,52,225,1002,102,38,224,101,-3534,224,224,4,224,102,8,223,223,101,4,224,224,1,223,224,223,1102,15,57,225,1102,55,49,225,1102,11,33,225,1101,56,40,225,1,131,105,224,101,-103,224,224,4,224,102,8,223,223,1001,224,2,224,1,224,223,223,1102,51,39,225,1101,45,90,225,2,173,139,224,101,-495,224,224,4,224,1002,223,8,223,1001,224,5,224,1,223,224,223,1101,68,86,224,1001,224,-154,224,4,224,102,8,223,223,1001,224,1,224,1,224,223,223,4,223,99,0,0,0,677,0,0,0,0,0,0,0,0,0,0,0,1105,0,99999,1105,227,247,1105,1,99999,1005,227,99999,1005,0,256,1105,1,99999,1106,227,99999,1106,0,265,1105,1,99999,1006,0,99999,1006,227,274,1105,1,99999,1105,1,280,1105,1,99999,1,225,225,225,1101,294,0,0,105,1,0,1105,1,99999,1106,0,300,1105,1,99999,1,225,225,225,1101,314,0,0,106,0,0,1105,1,99999,108,226,677,224,1002,223,2,223,1006,224,329,1001,223,1,223,1007,226,226,224,1002,223,2,223,1006,224,344,101,1,223,223,1008,226,226,224,102,2,223,223,1006,224,359,1001,223,1,223,107,226,677,224,1002,223,2,223,1005,224,374,101,1,223,223,1107,677,226,224,102,2,223,223,1006,224,389,101,1,223,223,108,677,677,224,102,2,223,223,1006,224,404,1001,223,1,223,1108,677,226,224,102,2,223,223,1005,224,419,101,1,223,223,1007,677,226,224,1002,223,2,223,1006,224,434,101,1,223,223,1107,226,226,224,1002,223,2,223,1006,224,449,101,1,223,223,8,677,226,224,102,2,223,223,1006,224,464,1001,223,1,223,1107,226,677,224,102,2,223,223,1005,224,479,1001,223,1,223,1007,677,677,224,102,2,223,223,1005,224,494,1001,223,1,223,1108,677,677,224,102,2,223,223,1006,224,509,101,1,223,223,1008,677,677,224,102,2,223,223,1005,224,524,1001,223,1,223,107,226,226,224,1002,223,2,223,1005,224,539,101,1,223,223,7,226,226,224,102,2,223,223,1005,224,554,101,1,223,223,1108,226,677,224,1002,223,2,223,1006,224,569,1001,223,1,223,107,677,677,224,102,2,223,223,1005,224,584,101,1,223,223,7,677,226,224,1002,223,2,223,1005,224,599,101,1,223,223,108,226,226,224,1002,223,2,223,1005,224,614,101,1,223,223,1008,677,226,224,1002,223,2,223,1005,224,629,1001,223,1,223,7,226,677,224,102,2,223,223,1005,224,644,101,1,223,223,8,677,677,224,102,2,223,223,1005,224,659,1001,223,1,223,8,226,677,224,102,2,223,223,1006,224,674,1001,223,1,223,4,223,99,226";

            Console.WriteLine($"Day 05, Part 1: Diagnostic Codes:");
            Day05_SunnyWithAChanceOfAsteroids.RunProgram(program, 1);
            Console.WriteLine($"Day 05, Part 2: Diagnostic Codes");
            Day05_SunnyWithAChanceOfAsteroids.RunProgram(program, 5);
        }

        static void Day04()
        {
            var min = 125730;
            var max = 579381;

            var codeList_Part1 = Day04_SecureContainer.GetValidCodesInRange_Part1(min, max);
            var codeList_Part2 = Day04_SecureContainer.GetValidCodesInRange_Part2(min, max);

            Console.WriteLine($"Day 04, Part 1: Secure Container: {codeList_Part1.Count} valid codes");
            Console.WriteLine($"Day 04, Part 2: Secure Container: {codeList_Part2.Count} valid codes");
        }

        static void Day03()
        {
            var timer = new Stopwatch();
            var wire1 = new Day03_CrossedWires.Wire("R1004,U518,R309,D991,R436,D360,L322,U627,R94,D636,L846,D385,R563,U220,L312,D605,L612,D843,R848,U193,L671,D852,L129,D680,L946,D261,L804,D482,R196,U960,L234,U577,R206,D973,R407,D400,R44,D103,R463,U907,L972,U628,L962,U856,L564,D25,L425,U332,R931,U837,R556,U435,R88,U860,L982,D393,R793,D86,R647,D337,R514,D361,L777,U640,R833,D674,L817,D260,R382,U168,R161,U449,L670,U814,L42,U461,R570,U855,L111,U734,L699,U602,R628,D79,L982,D494,L616,D484,R259,U429,L917,D321,R429,U854,R735,D373,L508,D59,L207,D192,L120,D943,R648,U245,L670,D571,L46,D195,L989,U589,L34,D177,L682,U468,L783,D143,L940,U412,R875,D604,R867,D951,L82,U851,L550,D21,L425,D81,L659,D231,R92,D232,R27,D269,L351,D369,R622,U737,R531,U693,R295,U217,R249,U994,R635,U267,L863,U690,L398,U576,R982,U252,L649,U321,L814,U516,R827,U74,L80,U624,L802,D620,L544,U249,R983,U424,R564,D217,R151,U8,L813,D311,R203,U478,R999,U495,R957,U641,R40,U431,L830,U67,L31,U532,R345,U878,L996,D223,L76,D264,R823,U27,L776,U936,L614,U421,L398,U168,L90,U525,R640,U95,L761,U938,R296,D463,L349,D709,R428,U818,L376,D444,L748,D527,L755,U750,R175,U495,R587,D767,L332,U665,L84,D747,L183,D969,R37,D514,R949,U985,R548,U939,L170,U415,R857,D480,R836,D363,R763,D997,R721,D140,R699,U673,L724,U375,R55,U758,R634,D590,L608,U674,R809,U308,L681,D957,R30,D913,L633,D939,L474,D567,R290,D615,L646,D478,L822,D471,L952,D937,R306,U380,R695,U788,R555,D64,R769,D785,R115,U474,R232,U353,R534,D268,L434,U790,L777,D223,L168,U21,L411,D524,R862,D43,L979,U65,R771,U872,L983,U765,R162");
            var wire2 = new Day03_CrossedWires.Wire("L998,U952,R204,U266,R353,U227,L209,D718,L28,D989,R535,U517,L934,D711,R878,U268,L895,D766,L423,U543,L636,D808,L176,U493,R22,D222,R956,U347,R953,U468,R657,D907,R464,U875,L162,U225,L410,U704,R76,D985,L711,U176,R496,D720,L395,U907,R223,D144,R292,D523,R514,D942,R838,U551,L487,D518,L159,D880,R53,D519,L173,D449,R525,U645,L65,D568,R327,U667,R790,U131,R402,U869,R287,D411,R576,D265,R639,D783,R629,U107,L571,D247,L61,D548,L916,D397,R715,U138,R399,D159,L523,U2,R794,U699,R854,U731,L234,D135,L98,U702,L179,D364,R123,D900,L548,U880,R560,D648,L701,D928,R256,D970,L396,U201,L47,U156,R723,D759,R663,D306,L436,U508,R371,D494,L147,U131,R946,D207,L516,U514,R992,D592,L356,D869,L299,U10,R744,D13,L52,U749,R400,D146,L193,U720,L226,U973,R971,U691,R657,D604,L984,U652,L378,D811,L325,D714,R131,D428,R418,U750,L706,D855,L947,U557,L985,D688,L615,D114,R202,D746,R987,U353,R268,U14,R709,U595,R982,U332,R84,D620,L75,D885,L269,D544,L137,U124,R361,U502,L290,D710,L108,D254,R278,U47,R74,U293,R237,U83,L80,U661,R550,U886,L201,D527,L351,U668,R366,D384,L937,D768,L906,D388,L604,U515,R632,D486,L404,D980,L652,U404,L224,U957,L197,D496,R690,U407,L448,U953,R391,U446,L964,U372,R351,D786,L187,D643,L911,D557,R254,D135,L150,U833,R876,U114,R688,D654,L991,U717,R649,U464,R551,U886,L780,U293,L656,U681,L532,U184,L903,D42,L417,D917,L8,U910,L600,D872,L632,D221,R980,U438,R183,D973,L321,D652,L540,D163,R796,U404,L507,D495,R707,U322,R16,U59,L421,D255,L463,U462,L524,D703,L702,D904,L597,D385,L374,U411,L702,U804,R706,D56,L288");

            var intersectionDistance = Day03_CrossedWires.IntersectWires(wire1, wire2);
            var minTotalDistance = Day03_CrossedWires.GetShortestWireLength(wire1, wire2);

            Console.WriteLine($"Day 03, Part 1: Crossed Wires: closest intersection distance is {intersectionDistance}");
            Console.WriteLine($"Day 03, Part 2: Crossed Wires: shortest intersection distance is {minTotalDistance}");
        }

        static void Day02()
        {
            var program = new List<int> { 1,12,2,3,1,1,2,3,1,3,4,3,1,5,0,3,2,1,10,19,2,6,19,23,1,23,5,
                27,1,27,13,31,2,6,31,35,1,5,35,39,1,39,10,43,2,6,43,47,1,47,5,51,1,51,9,55,2,55,6,59,1,59,
                10,63,2,63,9,67,1,67,5,71,1,71,5,75,2,75,6,79,1,5,79,83,1,10,83,87,2,13,87,91,1,10,91,95,
                2,13,95,99,1,99,9,103,1,5,103,107,1,107,10,111,1,111,5,115,1,115,6,119,1,119,10,123,1,123,
                10,127,2,127,13,131,1,13,131,135,1,135,10,139,2,139,6,143,1,143,9,147,2,147,6,151,1,5,151,
                155,1,9,155,159,2,159,6,163,1,163,2,167,1,10,167,0,99,2,14,0,0
            };
            
            var firstProgram = new List<int>();
            firstProgram.AddRange(program);
            Console.WriteLine($"Day 02, Part 1: Gravity Assist: {Day02_1202ProgramAlarm.RunProgram(firstProgram)[0]}");
            for (int noun = 0; noun <= 99; noun++)
            {
                for (int verb = 0; verb <= 99; verb++)
                {
                    var newProgram = new List<int>();
                    newProgram.AddRange(program);
                    newProgram[1] = noun;
                    newProgram[2] = verb;
                    var output = Day02_1202ProgramAlarm.RunProgram(newProgram)[0];
                    if (output == 19690720)
                    {
                        Console.WriteLine($"Day 02, Part 2: Gravity Assist: {100 * noun + verb} noun/verb");
                    }
                }
            }
        }

        static void Day01()
        {
            var modulesMasses = new List<int> {
                98541, 129056, 134974, 66390, 121382, 94570, 107586, 98767, 65101, 56320, 63431, 112200, 119262, 142745, 143941,
                148764, 70301, 149623, 125170, 114562, 136701, 76971, 52292, 127671, 107547, 77460, 55268, 119986, 104257, 82814, 
                64527, 74279, 98542, 54710, 96317, 105670, 146248, 134587, 104028, 65286, 91788, 106723, 137825, 139949, 74403, 
                106574, 133990, 96165, 121316, 94072, 76612, 109470, 147556, 113157, 67117, 85237, 134232, 94622, 76160, 107532, 
                120637, 51505, 82847, 105600, 97719, 113114, 68177, 149213, 116125, 145577, 83921, 134810, 138804, 90125, 70621, 
                103245, 51584, 93437, 125352, 100578, 53497, 112023, 92999, 107998, 148030, 101185, 65777, 92272, 145846, 81488, 
                61957, 69551, 125625, 146328, 123666, 102629, 121996, 94172, 128023, 123472
            };

            var rocket = Day01_RocketEquation.CreateRocket(modulesMasses);
            Console.WriteLine($"Day 01: Fuel Equation: {rocket.TotalFuelRequirement} fuel required");
        }
    }
}
