using GusteruStudio.PuzzleStorm;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SelectionMode : ScriptableObject
{
    [Button]
    public abstract void Set();

    public abstract void Exit();

    public abstract void Enter();

    protected abstract void MakeSelection(PuzzlePiece pp);
}
