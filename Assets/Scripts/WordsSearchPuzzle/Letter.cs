using GusteruStudio.PuzzleStorm;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

[RequireComponent(typeof(BoxCollider2D))]
public sealed class Letter : MonoBehaviour, PuzzlePiece
{
    [ShowInInspector] private char _letter = '0';

    private LetterSymbol _letterSymbol;

    private void Awake()
    {
        _letterSymbol = GetComponentInChildren<LetterSymbol>();
        Assert.IsNotNull(_letterSymbol, "LETTER: " + name + " doesn't have a Letter Symbol component as child");
    }

    public void Set(char letter)
    {
        _letter = letter;
        _letterSymbol.Set(letter);
    }

    public char Get()
    {
        return _letter;
    }
}
