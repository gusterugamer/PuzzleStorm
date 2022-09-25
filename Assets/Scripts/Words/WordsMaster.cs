using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using UnityEngine;

[CreateAssetMenu(menuName = "PuzzleStorm/WordsMaster")]
public class WordsMaster : ScriptableObject
{
    //Build a word given positions of start and end letters in words matrix
    public void BuildWord(Letter start, Letter end)
    {
        Vector2Int startIndex = GetIndex(start);
        Vector2Int endIndex = GetIndex(end);

        // Word can be built in 6 ways ( up, down, left, right, first diag, second diag)
        if (startIndex.x == endIndex.x)
        {
            if (startIndex.y > endIndex.y)
            {
                Vector2Int tempIndex = startIndex;
                startIndex = endIndex;
                endIndex = tempIndex;
            }

            for (int i = startIndex.y;i<endIndex.y;i++)
            {
                //TODO: complete implementation when building the puzzle mode
            }
        }
    }

    //Check if the word is valid
    private void ValidateWord(/*Word structure here*/)
    {

    }

    private Vector2Int GetIndex(Letter letter)
    {
        return Vector2Int.zero;
    }
}
