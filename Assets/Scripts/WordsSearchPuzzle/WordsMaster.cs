using GusteruStudio.ReactiveVariables;
using GusteruStudio.Selection;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "PuzzleStorm/WordsMaster")]
public sealed class WordsMaster : ScriptableObject
{
    [BoxGroup("Event")][SerializeField] private LetterSelectionEvent _onLettersSelected;
    //Build a word given positions of start and end letters in words matrix

    private void OnEnable()
    {
        _onLettersSelected.Unsubscribe(BuildWord);
        _onLettersSelected.Subscribe(BuildWord);
    }

    private void OnDisable()
    {
        _onLettersSelected.Unsubscribe(BuildWord);
    }

    public void BuildWord(LetterSelection selection)
    {
        Vector2Int firstLetterPosition = selection.firstLetter.GridPosition;
        Vector2Int secondLetterPosition = selection.secondLetter.GridPosition;

        bool isReversed = firstLetterPosition.x > secondLetterPosition.x ||
                          firstLetterPosition.y > secondLetterPosition.y;

        if (isReversed)
        {
            Vector2Int temp = firstLetterPosition;
            firstLetterPosition = secondLetterPosition;
            secondLetterPosition = temp;
        }
       
        List<List<Letter>> grid = selection.firstLetter.Grid;
        string currentWord = ReadCharsInGrind(firstLetterPosition,secondLetterPosition, grid);

        if (isReversed)
            currentWord = new string(currentWord.Reverse().ToArray());

        ValidateWord(currentWord);
    }

    private string ReadCharsInGrind(Vector2Int firstLetterPosition, Vector2Int secondLetterPosition, in List<List<Letter>> grid)
    {
        int rowDistance = Mathf.Abs(firstLetterPosition.x - secondLetterPosition.x) + 1;
        int colDistance = Mathf.Abs(firstLetterPosition.y - secondLetterPosition.y) + 1;

        //Calculates the distance in grid between letters
        //the max value is required in case the 2 letters are on the same row or on the same coloumn
        //(for diag the distance is the same)
        int totalDistance = Mathf.Max(rowDistance, colDistance);

        //result of the calculation is either 0 or 1 depending if the selection is done 
        //on diagonal, vertical or horizontal
        int rowStep = rowDistance / totalDistance;
        int colStep = colDistance / totalDistance;

        string currentWord = "";
        for (int i = 0; i < totalDistance; i++)
        {
            currentWord += grid[firstLetterPosition.x + i * rowStep][firstLetterPosition.y + i * colStep].Character;
        }
        return currentWord;
    }

    //Check if the word is one of the picked words
    private void ValidateWord(string word)
    {

    }
}
