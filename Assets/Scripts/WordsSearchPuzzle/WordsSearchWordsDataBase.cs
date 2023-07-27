using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PuzzleStorm/WordsSearchWordsDataBase")]
public sealed class WordsSearchWordsDataBase : ScriptableObject
{
    [ShowInInspector, ReadOnly] private Dictionary<WordsTheme, List<string>> words = new Dictionary<WordsTheme, List<string>>(); 

    public void Init()
    {
        FillDataBase();
    }

    public List<string> GetWordsWithTheme(int numWords, WordsTheme theme)
    {
        return new List<string>();
    }

    private void FillDataBase()
    {

    }
}

public enum WordsTheme
{
    None = 0,
}
