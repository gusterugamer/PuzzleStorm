using GusteruStudio.Extensions;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

[RequireComponent(typeof(SpriteRenderer))]
public sealed class LetterSymbol : MonoBehaviour
{
    [BoxGroup("CharacterMap")][SerializeField] private WordsSearchCharactersMap _charactersMap;
    private SpriteRenderer _spriteRenderer = null;
    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void Set(char letter)
    {
        Assert.IsFalse(letter.IsLowerCase(), "THE LETTER SET IS LOWER CASE !!!");
        _spriteRenderer.sprite = _charactersMap.Get(letter);
    }
}
