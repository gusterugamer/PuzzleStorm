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

        float scaleFactor;

        //Calculate a scale factor for the letter prefab
        // such that one dimesion fits exactly the grid area
        if (level.grid[0].Count > level.grid.Count)
        {
            float currentScaleOfRow = level.grid[0].Count * letterSize.x;
            scaleFactor = CalculateLetterScaleFactor(currentScaleOfRow, boundsSize.x); //scale by row
        }
        else
        {
            float currentScaleOfColoumn = level.grid.Count * letterSize.y;
            scaleFactor = CalculateLetterScaleFactor(currentScaleOfColoumn, boundsSize.y); //scale by row
        }

        //building the grid 
        for (int i = 0; i < level.grid.Count; i++)
        {
            _currentGrid.Add(new List<Letter>());
            for (int j = 0; j < level.grid[i].Count; j++)
            {
                Letter newLetter = Instantiate(_letterPrefab);
                newLetter.transform.localScale *= scaleFactor;
                newLetter.transform.position = new Vector3(j * newLetter.GetSize().x, -i * newLetter.GetSize().y, 0.0f); //The rows have to go one underneath another
                newLetter.SetSymbol(level.grid[i][j]);
                newLetter.SetGridPosition(new Vector2Int(i, j));
                newLetter.SetGrid(_currentGrid);
                _currentGrid[i].Add(newLetter);
            }
        }

        CenterGrid(_currentGrid);
    }

    protected override void LoadLevel()
    {

    }

    private float CalculateLetterScaleFactor(float rowSize, float boundsSize)
    {
        return boundsSize / rowSize;
    }

    private void CenterGrid(in List<List<Letter>> grid)
    {
        Vector3 center = Vector3.zero;
        for (int i = 0; i < grid.Count; i++)
        {
            for (int j = 0; j < grid[i].Count; j++)
            {
                center += grid[i][j].transform.position;
            }
        }
        center /= (grid.Count * grid[0].Count);

        Vector3 gridCenter = (_gridBounds.TopLeft + _gridBounds.BottomRight) / 2f;

        Vector3 offset = gridCenter - center;
        offset.z = 0.0f;

        for (int i = 0; i < grid.Count; i++)
        {
            for (int j = 0; j < grid[i].Count; j++)
            {
                grid[i][j].transform.position += offset;
            }
        }
    }
}
