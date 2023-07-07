using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PuzzleStorm/Puzzles/WordSearchPuzzle")]
public sealed class WordSearchPuzzle : Puzzle
{
    private WordsSearchLevelGenerator _wordsSearchGenerator = null;

    public override void Init()
    {
        _wordsSearchGenerator = (WordsSearchLevelGenerator)_levelGenerator;
        _wordsSearchGenerator.Init();
    }

    public override void Enter()
    {
        _wordsSearchGenerator.Generate();
    }

    public override void Exit()
    {
       
    }

    public override void Set()
    {
        PuzzleManager.Set(this);
        _selectionMode.Set();
    }

    protected override void ClearLevel()
    {
        
    }

    protected override void InstantiateLevel()
    {
        
    }

    protected override void LoadLevel()
    {
        
    }
}
