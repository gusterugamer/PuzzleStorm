using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Assertions;

/// <summary>
/// This object is responsable to manage and switch the current puzzle that is currently running
/// </summary>

public abstract class PuzzleManager : ScriptableObject
{
    private static Puzzle _currentPuzzle;

    public static void Set(Puzzle newPuzzle)
    {
        Assert.IsNotNull(newPuzzle, "PUZZLE MANAGER TRIED TO A PUZZLE TO A NULL REFERENCE!");
        _currentPuzzle?.Exit();
        _currentPuzzle = newPuzzle;
        _currentPuzzle.Enter();
    }
}
