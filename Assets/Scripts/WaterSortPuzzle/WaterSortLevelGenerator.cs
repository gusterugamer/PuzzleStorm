using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Assertions;
using System;
using System.Linq;
using System.Drawing;
using UnityEngine.U2D.IK;
using Sirenix.OdinInspector;

[CreateAssetMenu(menuName = "PuzzleStorm/PuzzleGenerators/WaterSortGenerator")]

public sealed class WaterSortLevelGenerator : LevelGenerator
{
    [BoxGroup("COLORS")][SerializeField] private WaterSortColors _colors;

    public override void Init()
    {

    }

    [Button]
    public override BaseLevel Generate()
    {
        WaterSortLevel level = new WaterSortLevel();
        level.color = WaterSortSolver.GenerateLevel(_colors.colors.ToList(), 12, 0.5f);

        return level;
    }

    public override void ParseLevel(string path)
    {
        Assert.IsTrue(path.Contains("WaterSort"), "THE LEVEL YOU ATTEMPTED TO LOAD AT PATHL: <<< " + path + " >>> IS NOT A WATERSORT PUZZLE LEVEL");
    }
}