using System;
using System.Collections.Generic;
using System.Linq;

class Day16
{
    private static List<int> BasePattern = new List<int> { 0, 1, 0, -1 };
    private List<long> _inputSignal;
    private List<long> _outputSignal = new List<long>();
    private int _lastBasePatternValue;
    private int _basePatternCount = 1;
    private int _basePatternPosition;

    public Day16(string path)
    {
        var inputFile = Utilities.LoadFile(path);
        _inputSignal = Utilities.NumberStringToList(inputFile);
    }

    public Day16(string signal, bool directLoad)
    {
        _inputSignal = Utilities.NumberStringToList(signal);
    }

    public List<long> ProcessSignal(int repeats, int phases)
    {
        RepeatList(repeats);
        var finalIndex = Convert.ToInt32(String.Join("", _inputSignal.Take(8)));
        Console.WriteLine($"Final Index: {finalIndex}");
        var processedSignal = ProcessPhases(phases);
        var result = processedSignal.GetRange(finalIndex, finalIndex + 8);
        Console.WriteLine(String.Join("", result));
        return result;
    }

    public List<long> ProcessPhases(int phaseCount)
    {
        for (int i = 0; i < phaseCount; i++)
        {
            Console.WriteLine($"Phase Number {i}");
            var newSignal = ProcessPhase();
            _inputSignal =  newSignal;
        }
        return _inputSignal;
    }

    public void RepeatList(int repeats)
    {
        var newInput = new List<long>();
        for (int i = 0; i < repeats; i++)
        {
            newInput.AddRange(_inputSignal);
        }
        _inputSignal = newInput;
    }

    private int GetBasePatternPosition(int loopCount, int currentPosition)
    {
        if (loopCount == 0) return ((currentPosition + 1) % 4);
        if (_basePatternPosition > BasePattern.Count - 1)
        {
            _basePatternPosition = 0;
        }

        var returnVal = _basePatternCount != 0 ? _lastBasePatternValue : _basePatternPosition;
        _lastBasePatternValue = returnVal;
        if (_basePatternCount > loopCount - 1)
        {
            _basePatternCount = 0;
            _basePatternPosition++;
        }
        else
        {
            _basePatternCount++;
        }
        
        return returnVal;
    }

    private List<long> ProcessPhase()
    {
        var outSignal = new List<long>();
        var outputPosition = 0;
        var loopCount = 0;
        for (int i = 0; i < _inputSignal.Count; i++)
        {
            long outputValue = 0;
            for (int j = 0; j < _inputSignal.Count; j++)
            {
                var basePatternPosition = GetBasePatternPosition(loopCount, j);
                var basePatternValue = BasePattern[basePatternPosition];
                var inputValue = _inputSignal[j];
                var outComponent = basePatternValue * inputValue;

                outputPosition++;
                outputValue += outComponent;
            }
            _lastBasePatternValue = 0;
            _basePatternPosition = 0;
            _basePatternCount = 1;
            outSignal.Add(Utilities.NumberStringToList(Math.Abs(outputValue).ToString()).Last());
            loopCount++;
        }
        return outSignal;
    }
}