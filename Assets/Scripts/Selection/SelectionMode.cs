using GusteruStudio.PuzzleStorm;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SelectionMode : ScriptableObject
{
    public abstract void Set();

    public abstract void Exit();

    public abstract void Enter();

    protected abstract void ProcessSelection(PuzzlePiece pp);
}
