﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode
{
    class Program
    {
        static void Main(string[] args)
        {
            // Day01();
            // Day02();
            Day03();
        }

        static void Day03()
        {
            var wire1 = new Day03_CrossedWires.Wire("R1004,U518,R309,D991,R436,D360,L322,U627,R94,D636,L846,D385,R563,U220,L312,D605,L612,D843,R848,U193,L671,D852,L129,D680,L946,D261,L804,D482,R196,U960,L234,U577,R206,D973,R407,D400,R44,D103,R463,U907,L972,U628,L962,U856,L564,D25,L425,U332,R931,U837,R556,U435,R88,U860,L982,D393,R793,D86,R647,D337,R514,D361,L777,U640,R833,D674,L817,D260,R382,U168,R161,U449,L670,U814,L42,U461,R570,U855,L111,U734,L699,U602,R628,D79,L982,D494,L616,D484,R259,U429,L917,D321,R429,U854,R735,D373,L508,D59,L207,D192,L120,D943,R648,U245,L670,D571,L46,D195,L989,U589,L34,D177,L682,U468,L783,D143,L940,U412,R875,D604,R867,D951,L82,U851,L550,D21,L425,D81,L659,D231,R92,D232,R27,D269,L351,D369,R622,U737,R531,U693,R295,U217,R249,U994,R635,U267,L863,U690,L398,U576,R982,U252,L649,U321,L814,U516,R827,U74,L80,U624,L802,D620,L544,U249,R983,U424,R564,D217,R151,U8,L813,D311,R203,U478,R999,U495,R957,U641,R40,U431,L830,U67,L31,U532,R345,U878,L996,D223,L76,D264,R823,U27,L776,U936,L614,U421,L398,U168,L90,U525,R640,U95,L761,U938,R296,D463,L349,D709,R428,U818,L376,D444,L748,D527,L755,U750,R175,U495,R587,D767,L332,U665,L84,D747,L183,D969,R37,D514,R949,U985,R548,U939,L170,U415,R857,D480,R836,D363,R763,D997,R721,D140,R699,U673,L724,U375,R55,U758,R634,D590,L608,U674,R809,U308,L681,D957,R30,D913,L633,D939,L474,D567,R290,D615,L646,D478,L822,D471,L952,D937,R306,U380,R695,U788,R555,D64,R769,D785,R115,U474,R232,U353,R534,D268,L434,U790,L777,D223,L168,U21,L411,D524,R862,D43,L979,U65,R771,U872,L983,U765,R162");
            var wire2 = new Day03_CrossedWires.Wire("L998,U952,R204,U266,R353,U227,L209,D718,L28,D989,R535,U517,L934,D711,R878,U268,L895,D766,L423,U543,L636,D808,L176,U493,R22,D222,R956,U347,R953,U468,R657,D907,R464,U875,L162,U225,L410,U704,R76,D985,L711,U176,R496,D720,L395,U907,R223,D144,R292,D523,R514,D942,R838,U551,L487,D518,L159,D880,R53,D519,L173,D449,R525,U645,L65,D568,R327,U667,R790,U131,R402,U869,R287,D411,R576,D265,R639,D783,R629,U107,L571,D247,L61,D548,L916,D397,R715,U138,R399,D159,L523,U2,R794,U699,R854,U731,L234,D135,L98,U702,L179,D364,R123,D900,L548,U880,R560,D648,L701,D928,R256,D970,L396,U201,L47,U156,R723,D759,R663,D306,L436,U508,R371,D494,L147,U131,R946,D207,L516,U514,R992,D592,L356,D869,L299,U10,R744,D13,L52,U749,R400,D146,L193,U720,L226,U973,R971,U691,R657,D604,L984,U652,L378,D811,L325,D714,R131,D428,R418,U750,L706,D855,L947,U557,L985,D688,L615,D114,R202,D746,R987,U353,R268,U14,R709,U595,R982,U332,R84,D620,L75,D885,L269,D544,L137,U124,R361,U502,L290,D710,L108,D254,R278,U47,R74,U293,R237,U83,L80,U661,R550,U886,L201,D527,L351,U668,R366,D384,L937,D768,L906,D388,L604,U515,R632,D486,L404,D980,L652,U404,L224,U957,L197,D496,R690,U407,L448,U953,R391,U446,L964,U372,R351,D786,L187,D643,L911,D557,R254,D135,L150,U833,R876,U114,R688,D654,L991,U717,R649,U464,R551,U886,L780,U293,L656,U681,L532,U184,L903,D42,L417,D917,L8,U910,L600,D872,L632,D221,R980,U438,R183,D973,L321,D652,L540,D163,R796,U404,L507,D495,R707,U322,R16,U59,L421,D255,L463,U462,L524,D703,L702,D904,L597,D385,L374,U411,L702,U804,R706,D56,L288");

            var intersectionDistance = Day03_CrossedWires.IntersectWires(wire1, wire2);

            Console.WriteLine($"Day 3, Part 1: Crossed Wires: closest intersection distance is {intersectionDistance}");
        }

        static void Day02()
        {
            var program = new List<int> { 1,0,0,3,1,1,2,3,1,3,4,3,1,5,0,3,2,1,10,19,2,6,19,23,1,23,5,
                27,1,27,13,31,2,6,31,35,1,5,35,39,1,39,10,43,2,6,43,47,1,47,5,51,1,51,9,55,2,55,6,59,1,59,
                10,63,2,63,9,67,1,67,5,71,1,71,5,75,2,75,6,79,1,5,79,83,1,10,83,87,2,13,87,91,1,10,91,95,
                2,13,95,99,1,99,9,103,1,5,103,107,1,107,10,111,1,111,5,115,1,115,6,119,1,119,10,123,1,123,
                10,127,2,127,13,131,1,13,131,135,1,135,10,139,2,139,6,143,1,143,9,147,2,147,6,151,1,5,151,
                155,1,9,155,159,2,159,6,163,1,163,2,167,1,10,167,0,99,2,14,0,0
            };

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
                        Console.WriteLine($"Day 02: Gravity Assist: {100 * noun + verb} noun/verb");
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
