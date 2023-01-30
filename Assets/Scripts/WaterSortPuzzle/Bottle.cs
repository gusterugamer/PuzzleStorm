using GusteruStudio.PuzzleStorm;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public sealed class Bottle : MonoBehaviour, PuzzlePiece
{
    [Range(0f, 1f)]
    public float _fillAmount;

    public float _rotationMultiplier;

    private WaterColors _waterColors;

    public List<Color> colors => _colors;

    private List<Color> _colors;

    private void Awake()
    {
        _waterColors = GetComponentInChildren<WaterColors>();
        Assert.IsNull(_waterColors, "BOTTLE: " + name + " doesn't have the colors as child!");
    }

    private void OnValidate()
    {
        _waterColors = GetComponentInChildren<WaterColors>();
        _waterColors.SetFillAmount(_fillAmount, _rotationMultiplier);
    }

    private void OnDestroy()
    {

    }

    public void SetColors(List<Color> color)
    {
        _colors = color;
        Init();
    }

    private void Init()
    {
        //Spawn the sprite renderers

    }

    
}
