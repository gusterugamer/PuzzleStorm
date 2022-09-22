using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PuzzleStorm/SelectionManager")]
public class SelectionManager : ScriptableObject
{
    private static SelectionMode _currentMode = null;

    public static void SetMode(SelectionMode mode)
    {
        _currentMode?.Exit();
        _currentMode = mode;
        _currentMode.Enter();
    }
}
