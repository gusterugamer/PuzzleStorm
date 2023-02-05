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
    public List<Color> colors => _colors;

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
            _waterColors.SetFillAmount(_fillAmount, 1f);
        }
    }
    private WaterColors _waterColors;
    [ShowInInspector] private List<Color> _colors = new List<Color>(_MAX_NUMBER_OF_COLORS);
    private int clearColorsCount = 0; //clear colors = bottle is not filled


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
        // TO DO: CHANGE THE LIST TO ARRRY!!!!
        _colors = color;

        //Color at index 0 is the bottom color
        for (int i = 0; i < _MAX_NUMBER_OF_COLORS; i++)
        {
            if (color[i] == Color.clear)
                clearColorsCount++;
            ChangeColor(i, color[i]);
        }
        UpdateFillAmount();
    }

    private void AddColor(Color color)
    {
        clearColorsCount = Mathf.Clamp(--clearColorsCount, 0, _MAX_NUMBER_OF_COLORS);
        ChangeColor(_MAX_NUMBER_OF_COLORS - clearColorsCount - 1 , color);
        UpdateFillAmount();
    }

    private void RemoveColor(int index)
    {
        ChangeColor(index, Color.clear);
        clearColorsCount = Mathf.Clamp(++clearColorsCount, 0, _MAX_NUMBER_OF_COLORS);
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

        //Check if the bottles top colors match
        Color topColor = colors[_MAX_NUMBER_OF_COLORS - clearColorsCount - 1];
        Color secondBottle_TopColor = secondBottle.colors[_MAX_NUMBER_OF_COLORS - secondBottle.clearColorsCount - 1];

        if (topColor != secondBottle_TopColor)
        {
            Debug.Log("Colors do not match " + "<color=" + topColor.ToRGBHex() + ">" + "FIRST_BOTTLE_COLOR" + "</color>" +
                     " ---- " + "<color=" + secondBottle_TopColor.ToRGBHex() + ">" + "SECOND_BOTTLE_COLOR" + "</color>" +
                     " <<<" + secondBottle.name + ">>>" + " cannot be filled!", gameObject);
            return;
        }

        //Calculate how much volum can be transfered to the second bottle

        int lastColorVolume = GetLastColorVolume();
        //Number of clear colors also tells the amount of volum second bottle can recieve
        int transferableVolume = lastColorVolume <= secondBottle.clearColorsCount-1 ? lastColorVolume : secondBottle.clearColorsCount-1;
        //Transfer colors

        int lastClearColorIndex = _MAX_NUMBER_OF_COLORS - clearColorsCount - 1;

        for (int i  = lastClearColorIndex; i>= lastClearColorIndex - transferableVolume; i--)
        {
            secondBottle.AddColor(topColor);
            RemoveColor(i);
        }
    }

    private int GetLastColorVolume()
    {
        //Count how many times last color repets CONSECUTIVE to the bottom
        int nSegmentsWithLastColor = 0;
        int lastColor = _MAX_NUMBER_OF_COLORS - clearColorsCount - 1;
        for (int i = lastColor; i>0;i--)
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
        fillAmount = (float)(_MAX_NUMBER_OF_COLORS - clearColorsCount) / _MAX_NUMBER_OF_COLORS;
    }
}
