using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Initializer : MonoBehaviour
{
    [BoxGroup("Puzzle")][SerializeField] private Puzzle _puzzle;

    private void Start()
    {
        _puzzle.Set();
    }
}
