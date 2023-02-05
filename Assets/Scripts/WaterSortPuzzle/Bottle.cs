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
    public const int _MAX_NUMBER_OF_COLORS = 4;
    public float _rotationMultiplier;
    public Color[] colors => _colors;

    private float _fillAmount;
    private float fillAmount
    {
        get
        {
            return _fillAmount;
        }
        set
        {
            //Update fill amount in shader
            _fillAmount = value;
            _waterColors.SetFillAmount(_fillAmount);
        }
    }
    private WaterColors _waterColors;
    [ShowInInspector] private Color[] _colors = new Color[_MAX_NUMBER_OF_COLORS];
    private int colorsCount = 0; //clear colors = bottle is not filled

    private void Awake()
    {
        //All bottles require colors
        _waterColors = GetComponentInChildren<WaterColors>();
        Assert.IsNotNull(_waterColors, "BOTTLE: " + name + " doesn't have the colors as child!");
    }

    public void SetColors(in Color[] color)
    {
        //Color at index 0 is the bottom color
        for (int i = 0; i < _MAX_NUMBER_OF_COLORS; i++)
        {
            if (color[i] != Color.clear)
                colorsCount++;
            ChangeColor(i, color[i]);
        }
        UpdateFillAmount();
    }

    private void AddColor(Color color)
    {
        colorsCount = Mathf.Clamp(++colorsCount, 0, _MAX_NUMBER_OF_COLORS);
        ChangeColor(colorsCount - 1, color);
        UpdateFillAmount();
    }

    private void RemoveColor(int index)
    {
        ChangeColor(index, Color.clear);
        colorsCount = Mathf.Clamp(--colorsCount, 0, _MAX_NUMBER_OF_COLORS);
        UpdateFillAmount();
    }

    public void AttemptToFill(Bottle secondBottle)
    {
        //Check if the bottle is not full
        if (secondBottle.fillAmount == 1f)
        {
            Debug.Log("Bottle: " + secondBottle.name + " is full and cannot be filled!", gameObject);
            return;
        }

        //Check if the bottles top colors match or the other bottle is emtpy
        Color topColor = GetTopColor();
        Color secondBottle_TopColor = secondBottle.GetTopColor();

        if (topColor != secondBottle_TopColor && secondBottle_TopColor != Color.clear)
        {
            Debug.Log("Colors do not match " + "<color=" + topColor.ToRGBHex() + ">" + "FIRST_BOTTLE_COLOR" + "</color>" +
                     " ---- " + "<color=" + secondBottle_TopColor.ToRGBHex() + ">" + "SECOND_BOTTLE_COLOR" + "</color>" +
                     " <<<" + secondBottle.name + ">>>" + " cannot be filled!", gameObject);
            return;
        }

        //Calculate how much volum can be transfered to the second bottle

        int lastColorVolume = GetLastColorVolume();
        
        //Calculating transferable volume based on number of existing colors in bottle
        int secondBottle_FreeVolume = _MAX_NUMBER_OF_COLORS - secondBottle.colorsCount;
        int transferableVolume = lastColorVolume <= secondBottle_FreeVolume ? lastColorVolume : secondBottle_FreeVolume;
        int lastClearColorIndex = colorsCount - 1;

        for (int i = 0; i < transferableVolume; i++)
        {
            secondBottle.AddColor(topColor);
            RemoveColor(lastClearColorIndex - i);
        }
    }

    private int GetLastColorVolume()
    {
        //Count how many times last color repets CONSECUTIVE to the bottom
        int nSegmentsWithLastColor = 0;
        int lastColor = colorsCount - 1;
        for (int i = lastColor; i >= 0; i--)
        {
            if (_colors[i] == _colors[lastColor])
                nSegmentsWithLastColor++;
            else
                break;
        }
        return nSegmentsWithLastColor;
    }

    private void ChangeColor(int index, Color color)
    {
        _colors[index] = color;
        _waterColors.SetColor(index, color);
    }

    private void UpdateFillAmount()
    {
        fillAmount = (float)colorsCount / _MAX_NUMBER_OF_COLORS;
    }

    private Color GetTopColor()
    {
        if (colorsCount == 0)
            return Color.clear;
        return _colors[colorsCount - 1];
    }
}
