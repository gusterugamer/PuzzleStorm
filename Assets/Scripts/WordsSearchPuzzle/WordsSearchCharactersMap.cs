using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PuzzleStorm/WSCharacterMap")]
public sealed class WordsSearchCharactersMap : SerializedScriptableObject
{
    [SerializeField] private Dictionary<char,Sprite> _charMap;

    public Sprite Get(char letter)
    {
        return _charMap[letter];
    }
}
