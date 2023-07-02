using Sirenix.OdinInspector;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Common interface for gamemodes level generators to implement
/// </summary>

public abstract class LevelGenerator : ScriptableObject 
{
    public abstract void Init();

    [Button]
    public abstract BaseLevel Generate();


    public abstract void ParseLevel(string path);
}
