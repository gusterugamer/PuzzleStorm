using GusteruStudio.Extensions;
using GusteruStudio.PuzzleStorm;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using System.Collections;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
#endif

using UnityEngine;
using UnityEngine.Assertions;

public sealed class Bottle : MonoBehaviour, PuzzlePiece
{
    [Range(0f, 1f)]
    [SerializeField] private float _fillAmount;

    private float fillAmount
    {
        get 
        {
            return _fillAmount; 
        }
        set
        {
            _fillAmount = value;
            _waterColors.SetFillAmount(_fillAmount, 1f);
        }
    }

    public float _rotationMultiplier;
    public List<Color> colors => _colors;

    private WaterColors _waterColors;

    [ShowInInspector] private List<Color> _colors;

    public const int _MAX_NUMBER_OF_COLORS = 4;

    private void OnValidate()
    {
        if (EditorApplication.isPlaying && Time.time > 1f)
        {
            _waterColors.SetFillAmount(_fillAmount, 1f);
        }
    }

    private void Awake()
    {
        //All bottles require colors
        _waterColors = GetComponentInChildren<WaterColors>();
        Assert.IsNotNull(_waterColors, "BOTTLE: " + name + " doesn't have the colors as child!");
    }

    public void SetColors(List<Color> color)
    {
        //Color at index 0 is the bottom color

        _colors = color;
        int nClearColors = 0;

        for (int i = 0; i < _MAX_NUMBER_OF_COLORS; i++)
        {
            if (color[i] == Color.clear)
                nClearColors++;

            _waterColors.SetColor(i, _colors[i]);
        }
        //Fill the bottle up only with visible colors
        _fillAmount = (float)(_MAX_NUMBER_OF_COLORS - nClearColors) / _MAX_NUMBER_OF_COLORS;
        _waterColors.SetFillAmount(_fillAmount, 1f);
    }

    private void AddColor(Color color)
    {
        int lastColor = 0;
        Color currentColor = colors[0];

        while (currentColor != Color.clear && lastColor < _MAX_NUMBER_OF_COLORS)
        {
            currentColor = colors[++lastColor];
        }
        colors[lastColor] = color;
        _waterColors.SetColor(lastColor, color);
    }

    public void AttemptToFill(Bottle secondBottle)
    {
        //Check if the bottle is not full
        if (secondBottle.fillAmount == 1f)
        {
            Debug.Log("Bottle: " + secondBottle.name + " is full and cannot be filled!", gameObject);
            return;
        }

        //Check if the second bottle top color (last color in list ) matches this color

        int lastColor = 0;

        while (lastColor < _MAX_NUMBER_OF_COLORS && secondBottle.colors[lastColor] != Color.clear)
        {
            lastColor++;
        }

        if (secondBottle.colors[lastColor - 1] == colors[colors.Count - 1])
        {
            fillAmount -= 0.25f;
            secondBottle.fillAmount += 0.25f;
            secondBottle.AddColor(secondBottle.colors[lastColor - 1]);
        }
        else
        {
            Debug.Log("First selected bottle color is: " + "<color="+ _colors[colors.Count - 1].ToRGBHex() + ">" + "FIRST_BOTTLE_COLOR" + "</color>" +
                      " Second selected bottle color is: " + "<color=" + secondBottle.colors[lastColor - 1].ToRGBHex() + ">" + "SECOND_BOTTLE_COLOR" + "</color>" + 
                      " <<<" + secondBottle.name + ">>>" + " cannot be filled!", gameObject);
        }
    }
}
