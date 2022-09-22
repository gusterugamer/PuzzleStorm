using GusteruStudio.PuzzleStorm;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class Bottle : MonoBehaviour, PuzzlePiece
{
    [BoxGroup("Config")]
    [SerializeField]
    private uint _maxNumColors = 0;

    private uint _curNumColors = 0;

    [SerializeField] private Color[] _colors;

    public Color[] colors => _colors;

    private void Awake()
    {
       // _colors = new Color[_maxNumColors];
        _curNumColors = 0;
    }

    public void Fill(Color color)
    {
        //if (_curNumColors == _maxNumColors)
        //    return;

        _colors[0] = color;
    }

    public void Spill()
    {
        _colors[_colors.Length - 1] = new Color(0f,0f,0f,1f);
    }
}
