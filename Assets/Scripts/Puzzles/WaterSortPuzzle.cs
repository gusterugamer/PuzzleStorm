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
    [BoxGroup("DeviceScaler")][SerializeField] private DeviceAspectScaler _aspectScaler;

    private WaterSortLevelGenerator _waterSortGenerator;

    private const float _MIN_ROW_PADDING = 0.1f;

    private const float _SPACE_BETWEEN_ROWS = 0.25f;

    private const float _SPACE_BETWEEN_BOTTLES = 0.05f;

    private int _maxNumOfBottlesPerRow = 0;

    private List<Bottle> _currentLevelBottles = new List<Bottle>();

    public override void Init()
    {
        _waterSortGenerator = (WaterSortLevelGenerator)_levelGenerator;
        _waterSortGenerator.Init();
        CalculateMaxBottlesNumberPerRow();

        Debug.Log("WATER SORT PUZZLE INITIALIZED! ");
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
        InstantiateLevel();
    }

    protected override void ClearLevel()
    {
        foreach (Bottle bottle in _currentLevelBottles) 
        {
            Destroy(bottle.gameObject);
        }
        _currentLevelBottles.Clear();
    }
    
    protected override void InstantiateLevel()
    {
        WaterSortLevel waterSortLevel = _waterSortGenerator.Generate() as WaterSortLevel;

        _currentLevelBottles = new List<Bottle>(waterSortLevel.numColors);

        int numOfRows = Mathf.CeilToInt(waterSortLevel.numColors / _maxNumOfBottlesPerRow);

        Vector2 bottleSize = _bottlePrefab.GetBottleSize();

        int currentColorIndex = 0;
        for (int j = 0; j <= numOfRows; j++)
        {
            List<Bottle> bottlesRow = new List<Bottle>(_maxNumOfBottlesPerRow);
            for (int i = 0; i < _maxNumOfBottlesPerRow && currentColorIndex < waterSortLevel.numColors; i++)
            {
                Bottle newBottle = Instantiate(_bottlePrefab);
                newBottle.transform.position = new Vector3((_SPACE_BETWEEN_BOTTLES + bottleSize.x) * i, -j* (_SPACE_BETWEEN_ROWS + bottleSize.y), 0.0f);
                newBottle.SetColors(waterSortLevel.color[currentColorIndex++].ToArray());
                _currentLevelBottles.Add(newBottle);
                bottlesRow.Add(newBottle);
            }
            CenterRow(bottlesRow);
        }
        CenterBottles(_currentLevelBottles);
    }

    private void CalculateMaxBottlesNumberPerRow()
    {
        float viewWidth = _aspectScaler.GetViewWidth();
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
