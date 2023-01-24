using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class is responsable to manage and switch the current puzzle that is currently running
/// </summary>

public abstract class PuzzleManager : ScriptableObject
{
    private static Puzzle _currentPuzzle;

    public static void Set(Puzzle newPuzzle)
    {
        _currentPuzzle?.Exit();
        _currentPuzzle = newPuzzle;
        _currentPuzzle.Enter();
    }
}
