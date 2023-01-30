using GusteruStudio.PuzzleStorm;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public sealed class Bottle : MonoBehaviour, PuzzlePiece
{

    public const int _MAX_NUMBER_OF_COLORS = 4;

    [Range(0f, 1f)]
    public float _fillAmount;

    public float _rotationMultiplier;

    private WaterColors _waterColors;

    public List<Color> colors => _colors;

    private List<Color> _colors;

    private void Awake()
    {
        //All bottles require colors
        _waterColors = GetComponentInChildren<WaterColors>();
        Assert.IsNull(_waterColors, "BOTTLE: " + name + " doesn't have the colors as child!");
    }

    public void SetColors(List<Color> color)
    {
        _colors = color;
        for (int i=0;i< _MAX_NUMBER_OF_COLORS;i++)
        {
            _waterColors.SetColor(i, _colors[i]);
        }
    }
}
