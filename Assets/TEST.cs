using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TEST : MonoBehaviour
{
    public List<Bottle> _bottles;
    public WaterSortLevelGenerator _levelGen;

    private void Start()
    {
        WaterSortLevel level = _levelGen.Generate() as WaterSortLevel;
        for (int i = 0; i < _bottles.Count; i++)
        {
            Bottle bottle = _bottles[i];
            bottle.SetColors(level.color[i].ToArray());
        }
    }
}
