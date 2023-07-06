using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

//Game is created with 1080X1920 in mind so for different aspect ratio puzzle pices might 
//go off scream or partially rendered therefor we need to rescale the puzzle pieces based
//on the change of the aspect ratio

[CreateAssetMenu(menuName = "PuzzleStorm/Utility/DeviceAspectScaler")]

public sealed class DeviceAspectScaler : ScriptableObject
{
    public List<GameObject> _prefabs;

    private const float _DEFAULT_ASPECT_RATIO = 0.5625f;
    private float _ratioBetweenAspects = 1f;

    public static float rationBetweenAspects = 0.0f;


    public void UpdatePrefabs()
    {
        CalculateRatioBetweenAspects();

        foreach (var prefab in _prefabs) 
        {
            prefab.transform.localScale *= _ratioBetweenAspects;
        }
    }

    private void CalculateRatioBetweenAspects()
    {
        _ratioBetweenAspects = Camera.main.aspect / _DEFAULT_ASPECT_RATIO;
        rationBetweenAspects = _ratioBetweenAspects;
    }


    //Reset scale of prefabs affected by the aspect scaler in editor 
#if UNITY_EDITOR

    private void OnEnable()
    {
        EditorApplication.playModeStateChanged += OnPlayModeStateChange;
    }

    private void OnDisable()
    {
        EditorApplication.playModeStateChanged -= OnPlayModeStateChange;
    }

    private void OnPlayModeStateChange(PlayModeStateChange state)
    {
        if (state == PlayModeStateChange.ExitingPlayMode)
        {
            foreach (var prefab in _prefabs)
            {
                prefab.transform.localScale = Vector3.one;
            }
        }
    }
#endif
}
