using System;
using System.Collections.Generic;
using System.Linq;

class Day08_SpaceImageFormat
{
    private int[] _imageData;
    private int _height;
    private int _width;
    private List<int[]> _rawLayers = new List<int[]>();
    public List<Layer> Layers = new List<Layer>();
    public List<List<int>> Image = new List<List<int>>();

    public class Layer
    {
        private int _height;
        private int _width;
        public int[,] Contents; 

        public Layer(int height, int width)
        {
            _height = height;
            _width = width;
            Contents = new int[height, width];
        }
    }

    public void LoadData(string rawData, int width, int height)
    {
        _height = height;
        _width = width;
        _imageData = rawData.Select(c => Convert.ToInt32(c.ToString())).ToArray();
        GetRawLayers();
    }

    public void RenderImage()
    {
        CreateLayers();
        FlattenImage();

        foreach (var pixelRow in Image)
        {
            var imageRow = pixelRow.Select(p => p == 0 ? " " : "@");
            Console.WriteLine(String.Join("", imageRow));
        }
    }

    public int FindPart1Layer()
    {
        var selectedLayer = _rawLayers.Select(l => 
        new {
            Zeros = l.Count(i => i == 0),
            Layer = l
        }).OrderByDescending(l => l.Zeros).Last();

        var ones = selectedLayer.Layer.Count(i => i == 1);
        var twos = selectedLayer.Layer.Count(i => i == 2);

        return ones * twos;
    }

    private void GetRawLayers()
    {
        var layerLength = _width * _height;
        for (int i = 0; i < _imageData.Count(); i+=layerLength)
        {
            var layerEnd = i + layerLength;
            var layer = _imageData[i..layerEnd];
            _rawLayers.Add(layer);
        }
    }

    private void FlattenImage()
    {
        CreateLayers();

        for (int y = 0; y < _height; y++)
        {
            var pixelRow = new List<int>();
            for (int x = 0; x < _width; x++)
            {
                pixelRow.Add(EvaluatePixel(x, y));
            }
            Image.Add(pixelRow);
        }
    }

    private int EvaluatePixel(int x, int y)
    {
        foreach (var layer in Layers)
        {
            var layerPixelValue = layer.Contents[x, y];
            if (layerPixelValue == 1 || layerPixelValue == 0)
            {
                return layerPixelValue;
            }
        }
        return 2;
    }

    private void CreateLayers()
    {
        foreach (var rawLayer in _rawLayers)
        {
            var layer = new Layer(_height, _width);
            var layerData = new List<int[]>();
            for (int p = 0; p < rawLayer.Count(); p+=_width)
            {
                var rowEnd = p + _width;
                var pixelRow = rawLayer[p..rowEnd];
                layerData.Add(pixelRow);
            }
            
            var layerContent = new int[_width, _height];
            for (int i = 0; i < layerData.Count; i++)
            {
                var row = layerData[i];
                for (int j = 0; j < _width - 1; j++)
                {
                    layerContent[j, i] = row[j];
                }
            }
            layer.Contents = layerContent;
            Layers.Add(layer);
        }
    }
}