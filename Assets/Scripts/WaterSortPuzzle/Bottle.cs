using GusteruStudio.PuzzleStorm;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class Bottle : MonoBehaviour, PuzzlePiece
{
    [SerializeField] private List<Color> _colors;

    public List<Color> colors => _colors;

    private int _maxColors = 0;

    private void Awake()
    {
        WaterSortPuzzle.onPuzzleLoaded += InitializeColors;
    }

    private void OnDestroy()
    {
        WaterSortPuzzle.onPuzzleLoaded -= InitializeColors;
    }

    private void InitializeColors(List<List<List<string>>> levels)
    {
        _maxColors = levels[2][0].Count;
        _colors = new List<Color>(_maxColors);
        for (int i = 0; i < _colors.Capacity; i++)
        {
            string color = levels[2][0][i];
            if (color == "Red")
                _colors.Add(Color.red);
            if (color == "Blue")
                _colors.Add(Color.blue);
            if (color == "Green")
                _colors.Add(Color.green);
            if (color == "Yellow")
                _colors.Add(Color.yellow);
            if (color == "Purple")
                _colors.Add(Color.cyan);
            if (color == "Orange")
                _colors.Add(new Color(37, 150, 120));
            if (color == "Pink")
                _colors.Add(Color.magenta);
            if (color == "Gray")
                _colors.Add(Color.gray);
            if (color == "White")
                _colors.Add(Color.white);
        }
    }

    public void Fill(Bottle bottleFiller)
    {
        //Check if there is enough space
        if (_colors.Count >= _maxColors)
        {
            Debug.Log("CANNOT FILL BOTTLE: " + name + " BECAUSE IS FULL!");
            return;
        }

        Color colorFiller = bottleFiller.colors[0];

        //Check to see if the bottle is empty or it has the first color from TOP to BOTTOM the same as fillColor
        if (_colors.Count == 0 || colorFiller == _colors[0])
        {
            Debug.Log("BOTTLE: " + name + " WAS FILLED BY BOTTLE " + bottleFiller.name + " WITH COLOR: " + colorFiller.ToString());
            bottleFiller.Spill();
            _colors.Insert(0,colorFiller);
        }
    }

    private void Spill()
    {
        if (_colors.Count > 0)
        {
            _colors.RemoveAt(0);
            Debug.Log(name + " WAS SPILLED");
        }
    }
}
