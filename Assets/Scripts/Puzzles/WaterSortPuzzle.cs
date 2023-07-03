using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "PuzzleStorm/Puzzles/WaterSortPuzzle")]
public sealed class WaterSortPuzzle : Puzzle
{
    [BoxGroup("Prefab")][SerializeField] private Bottle _bottlePrefab;

    private WaterSortLevelGenerator _waterSortGenerator;

    private const float _MIN_ROW_PADDING = 0.1f;

    private const float _SPACE_BETWEEN_BOTTLES = 0.05f;

    private const float _DEFAULT_ASPECT_RATIO = 0.5625f;

    private float _ratioBetweenAspects = 1f;

    private void Awake()
    {
        _waterSortGenerator = (WaterSortLevelGenerator)_levelGenerator;
    }

    [Button]
    public override void Init()
    {
        _waterSortGenerator.Init();
        CalculateRatioBetweenAspects();
        UpdatePrefabScaleForNewAspectRatio();
    }

    public override void Set()
    {
        PuzzleManager.Set(this);
        _selectionMode.Set();
    }

    public override void Enter()
    {
        LoadLevel();
    }

    public override void Exit()
    {
        ClearLevel();
    }

    protected override void LoadLevel()
    {
        
    }

    protected override void ClearLevel()
    {

    }
    
    protected override void InstantiateLevel()
    {
        WaterSortLevel waterSortLevel = _waterSortGenerator.Generate() as WaterSortLevel;

        Vector3 cameraPos = Camera.main.transform.position;

        float viewWidth = GetViewWidth();

        float bottleWidth = _bottlePrefab.GetBottleSize().x;

        Vector3 startPos = new Vector3(bottleWidth * 0.5f + _MIN_ROW_PADDING, cameraPos.y, _bottlePrefab.transform.position.z);

        float length = viewWidth - 2f *_MIN_ROW_PADDING;
        float step = bottleWidth + _SPACE_BETWEEN_BOTTLES;

        //In loc sa le pozitionez de la inceput la capat , pot sa le instantiez pe toate spatiat apoi doar sa le centrez si padding-ul se va face automat

        int i = 0;
        for(float currentX = startPos.x; currentX < length; currentX+=step)
        {
            Vector3 currentPos = new Vector3(currentX,cameraPos.y,_bottlePrefab.transform.position.z);
            Bottle newBottle = Instantiate(_bottlePrefab);
            newBottle.transform.position = currentPos;
            newBottle.SetColors(waterSortLevel.color[i].ToArray());
            i++;
        }
    }

    private void CalculateRatioBetweenAspects()
    {
        _ratioBetweenAspects = Camera.main.aspect / _DEFAULT_ASPECT_RATIO;
    }

    private void UpdatePrefabScaleForNewAspectRatio()
    {
        _bottlePrefab.transform.localScale *= _ratioBetweenAspects;
    }

    private float GetViewWidth()
    {
        Camera mainCamera = Camera.main;

        float aspectRatio = mainCamera.aspect;
        float ortoSize = mainCamera.orthographicSize;

        return aspectRatio * ortoSize * 2f;
    }
}
