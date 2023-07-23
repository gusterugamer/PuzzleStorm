using GusteruStudio.PuzzleStorm;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

[RequireComponent(typeof(BoxCollider2D))]
public sealed class Letter : MonoBehaviour, PuzzlePiece
{
    [ShowInInspector, ReadOnly] private char _letter = '0';

    private LetterSymbol _letterSymbol;

    [ShowInInspector, ReadOnly]private Vector2Int _gridPosition;

    private List<List<Letter>> _grid = null;

    public List<List<Letter>> Grid => _grid;

    public Vector2Int GridPosition => _gridPosition;

    public char Character => _letter;

    private void Awake()
    {
        _letterSymbol = GetComponentInChildren<LetterSymbol>();
        Assert.IsNotNull(_letterSymbol, "LETTER: " + name + " doesn't have a Letter Symbol component as child");
    }

    public void SetSymbol(in char letter)
    {
        _letter = letter;
        _letterSymbol.Set(letter);
    }

    public Vector2 GetSize()
    {
        return GetComponent<BoxCollider2D>().size * transform.localScale.x;
    }

    public void SetGridPosition(in Vector2Int position)
    {
        _gridPosition = position;
    }

    public void SetGrid(in List<List<Letter>> grid) 
    {
        _grid = grid;
    }
}
