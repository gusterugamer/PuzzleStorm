using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Puzzle : ScriptableObject
{
    [BoxGroup("SelectionMode")]
    [SerializeField]
    protected SelectionMode _selectionMode;

    [BoxGroup("LevelGenerator")]
    [SerializeField]
    protected LevelGenerator _levelGenerator;

    [Button]
    public abstract void Init();

    [Button]
    public abstract void Set();

    public abstract void Exit();

    public abstract void Enter();

    protected abstract void LoadLevel();
    protected abstract void ClearLevel();
    [Button]
    protected abstract void InstantiateLevel();
}

