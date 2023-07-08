using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PuzzleStorm/Puzzles/WordsSearchPuzzle")]
public sealed class WordsSearchPuzzle : Puzzle
{
    [BoxGroup("Grid")][SerializeField] private WordsGridBounds _gridBounds;
    [BoxGroup("Grid")][SerializeField] private Letter _letterPrefab;
    private WordsSearchLevelGenerator _wordsSearchGenerator = null;

    private List<List<Letter>> _currentGrid = new List<List<Letter>>();

    public override void Init()
    {
        _wordsSearchGenerator = (WordsSearchLevelGenerator)_levelGenerator;
        _wordsSearchGenerator.Init();
    }

    public override void Enter()
    {
        InstantiateLevel();
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
        WordsSearchLevel level = _wordsSearchGenerator.Generate() as WordsSearchLevel;

        _currentGrid = new List<List<Letter>>();

        Vector2 letterSize = _letterPrefab.GetSize();
        Vector2 boundsSize = _gridBounds.GetSize();

        //Check if we can fit the number of letter at current scale
        int numLettersOnRow = Mathf.FloorToInt(boundsSize.x / letterSize.x);
        int numLettersOnColoumn = Mathf.FloorToInt(boundsSize.y / letterSize.y);

        float scaleFactor = 1f;
        if (numLettersOnRow < level.grid[0].Count || numLettersOnColoumn < level.grid.Count)
        {
            //Since letters are squares they have to have the same scale both on X and Y
            //therefor we need to see if the coloumn require more space or the row

            if (level.grid[0].Count - numLettersOnRow > level.grid.Count - numLettersOnColoumn)
                scaleFactor = CalculateLetterScaleFactor(level.grid[0].Count * _letterPrefab.GetSize().x, boundsSize.x);
            else
                scaleFactor = CalculateLetterScaleFactor(level.grid.Count * _letterPrefab.GetSize().y, boundsSize.y);
        }

        for (int i = 0; i < level.grid.Count; i++)
        {
            _currentGrid.Add(new List<Letter>());
            for (int j = 0; j < level.grid[i].Count; j++)
            {
                Letter newLetter = Instantiate(_letterPrefab);
                newLetter.transform.localScale *= scaleFactor;
                newLetter.transform.position = new Vector3(newLetter.transform.localScale.x / 2f + _gridBounds.TopLeft.x + i * newLetter.GetSize().x,
                                                          -newLetter.transform.localScale.y / 2f + _gridBounds.TopLeft.y - j * newLetter.GetSize().y, 0.0f);
                newLetter.SetSymbol(level.grid[i][j]);
                newLetter.SetGridPosition(new Vector2Int(i, j));
                newLetter.SetGrid(_currentGrid);
                _currentGrid[i].Add(newLetter);
            }
        }
    }

    protected override void LoadLevel()
    {

    }

    private float CalculateLetterScaleFactor(float rowSize, float boundsSize)
    {
        return boundsSize / rowSize;
    }
}
