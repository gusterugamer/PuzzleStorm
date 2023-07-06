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

    private const float _SPACE_BETWEEN_ROWS = 1.5f;

    private const float _SPACE_BETWEEN_BOTTLES = 0.05f;

    private int _maxNumOfBottlesPerRow = 0;

    private void Awake()
    {
        _waterSortGenerator = (WaterSortLevelGenerator)_levelGenerator;
    }

    [Button]
    public override void Init()
    {
        _waterSortGenerator.Init();
        CalculateMaxBottlesNumberPerRow();
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

        List<Bottle> createdBottles = new List<Bottle>(waterSortLevel.numColors);

        int numOfRows = Mathf.CeilToInt(waterSortLevel.numColors / _maxNumOfBottlesPerRow);

        //NOTE : Why is space between bottles preserved, but space between rows is not

        int currentColorIndex = 0;
        for (int j = 0; j <= numOfRows; j++)
        {
            List<Bottle> bottlesRow = new List<Bottle>(_maxNumOfBottlesPerRow);
            for (int i = 0; i < _maxNumOfBottlesPerRow && currentColorIndex < waterSortLevel.numColors; i++)
            {
                Bottle newBottle = Instantiate(_bottlePrefab);
                newBottle.transform.position = new Vector3((_SPACE_BETWEEN_BOTTLES + _bottlePrefab.GetBottleSize().x) * i, j* -_SPACE_BETWEEN_ROWS * DeviceAspectScaler.rationBetweenAspects,0.0f);
                newBottle.SetColors(waterSortLevel.color[currentColorIndex++].ToArray());
                createdBottles.Add(newBottle);
                bottlesRow.Add(newBottle);
            }
            CenterRow(bottlesRow);
        }

        CenterBottles(createdBottles);
    }

    private float GetViewWidth()
    {
        Camera mainCamera = Camera.main;

        float aspectRatio = mainCamera.aspect;
        float ortoSize = mainCamera.orthographicSize;

        return aspectRatio * ortoSize * 2f;
    }

    private void CalculateMaxBottlesNumberPerRow()
    {
        float viewWidth = GetViewWidth();
        float bottleWidth = _bottlePrefab.GetBottleSize().x;

        //Subtracting the minimum padding from the front and the back of the row
        float length = viewWidth - 2f* _MIN_ROW_PADDING;

        //Calculating how many bottles would fit without spacing
        int nBottlesNoSpace = (int)(length / bottleWidth);

        //Calculating the length of all the space between bottles except last one
        //because the last one has padding after it
        float rowSpace = (nBottlesNoSpace - 1) * _SPACE_BETWEEN_BOTTLES;
        //removing that space from the length to account for bottles spacing
        float effectiveLength = length - rowSpace;
        
        _maxNumOfBottlesPerRow = (int)(effectiveLength/bottleWidth);
    }

    private void CenterBottles(in List<Bottle> bottles)
    {
        //Calculateing the center of the array of bottles that was created

        Vector3 centerOfBottles = Vector3.zero;

        foreach (Bottle bottle in bottles)
        {
            centerOfBottles += bottle.transform.position;
        }

        centerOfBottles /= bottles.Count;

        Vector3 offSetToCameraCenter = Camera.main.transform.position - centerOfBottles;
        offSetToCameraCenter.z = 0f; //we care to offset the bottle only on X and Y

        //Apply offset to each bottle to be centered
        foreach (Bottle bottle in bottles)
        {
            bottle.transform.position += offSetToCameraCenter;
        }
    }

    private void CenterRow(in List<Bottle> bottlesRow)
    {
        //Center bottles on X
        float rowCenterX = 0f;

        foreach (Bottle bottle in bottlesRow)
        {
            rowCenterX += bottle.transform.position.x;
        }
        rowCenterX /= bottlesRow.Count;

        float xOffsetToCameraCenter = Camera.main.transform.position.x - rowCenterX;

        foreach (Bottle bottle in bottlesRow)
        {
            Vector3 currentPositon = bottle.transform.position;
            currentPositon.x += xOffsetToCameraCenter;
            bottle.transform.position = currentPositon;
        }
    }
}
