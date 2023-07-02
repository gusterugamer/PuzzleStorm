using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "PuzzleStorm/Puzzles/WaterSortPuzzle")]
public sealed class WaterSortPuzzle : Puzzle
{
    private WaterSortLevelGenerator _waterSortGenerator;

    private void Awake()
    {
        _waterSortGenerator = (WaterSortLevelGenerator)_levelGenerator;
    }

    public override void Init()
    {
        _waterSortGenerator.Init();
    }

    public override void Set()
    {
        PuzzleManager.Set(this);
        _selectionMode.Set();
    }

    public override void Enter()
    {
        LoadLevel();
    }

    public override void Exit()
    {
        ClearLevel();
    }

    protected override void LoadLevel()
    {
        WaterSortLevel waterSortLevel = _waterSortGenerator.Generate() as WaterSortLevel;
    }

    protected override void ClearLevel()
    {

    }

    protected override void InstantiateLevel()
    {
        
    }


}
