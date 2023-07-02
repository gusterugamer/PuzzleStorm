using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PuzzleStorm/WaterSortColors")]
public sealed class WaterSortColors : ScriptableObject
{
    [BoxGroup("COLORS")]
    [SerializeField]
    public List<Color> colors;
}
