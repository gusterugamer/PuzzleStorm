using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PuzzleStorm/PuzzleGenerators/WordSearchPuzzleGenerator")]
public sealed class WordsSearchLevelGenerator : LevelGenerator
{
    public override BaseLevel Generate()
    {
        return new WordsSearchLevel();
    }

    public override void Init()
    {
       
    }

    public override void ParseLevel(string path)
    {
        
    }
}
