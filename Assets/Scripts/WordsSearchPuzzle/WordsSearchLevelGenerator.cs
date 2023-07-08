using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PuzzleStorm/PuzzleGenerators/WordSearchPuzzleGenerator")]
public sealed class WordsSearchLevelGenerator : LevelGenerator
{
    public override BaseLevel Generate()
    {
        WordsSearchLevel level = new WordsSearchLevel();
        level.grid = new List<List<char>>();
        for(int i=0;i<10;i++)
        {
            level.grid.Add(new List<char>());
            for (int j =0;j<10;j++)
            {
                int start = 'A';
                int end = 'Z';
                level.grid[i].Add((char)Random.Range(start, end));
            }
        }

        return level;
    }

    public override void Init()
    {
       
    }

    public override void ParseLevel(string path)
    {
        
    }
}
