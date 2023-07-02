using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using UnityEngine;

public sealed class WaterSortSolver
{
    private static System.Random random = new System.Random();

    private const int _MIN_NUM_OF_SOLUTIONS = 10;

    private const float _SECONDS_TO_WAIT_FOR_SOLUTIONS = 5f;

    // Function to generate a random level
    public static List<List<Color>> GenerateLevel(in List<Color> _bottleColors, int numTubes, float difficulty, int numEmptyTubes = 2)
    {
        if (_bottleColors.Count == 0)
        {
            Debug.LogError("THE COLORS LIST OF THE SOLVER IS EMTPY!!!");
            return null;
        }

        int numColors = numTubes - numEmptyTubes;
        List<List<Color>> level = new List<List<Color>>(numTubes);
        Dictionary<Color, int> colorPieces = new Dictionary<Color, int>(); // color,num. pieces left

        for (int i = 0; i < numTubes; i++)
        {
            var temp = new List<Color>(Bottle.MAX_NUMBER_OF_COLORS);
            level.Add(temp);
            for (int j = 0; j < Bottle.MAX_NUMBER_OF_COLORS; j++)
            {
                temp.Add(Color.clear);
            }
        }

        // Generate random colors for the tubes
        for (int i = 0; i < numColors; i++)
        {
            colorPieces.Add(_bottleColors[i], Bottle.MAX_NUMBER_OF_COLORS);
        }

        for (int i = 0; i < numTubes; i++)
        {
            for (int j = 0; j < Bottle.MAX_NUMBER_OF_COLORS; j++)
            {
                if (colorPieces.Count > 0)
                {
                    int randomIndex = random.Next(0, colorPieces.Count);

                    Color pickedColor = colorPieces.ElementAt(randomIndex).Key;

                    level[i][j] = pickedColor;
                    colorPieces[pickedColor]--;

                    if (colorPieces[pickedColor] <= 0)
                        colorPieces.Remove(pickedColor);
                }
                else
                {
                    level[i][j] = Color.clear; // Empty tube
                }
            }
        }

        if (Solve(level)) return level;

        return new List<List<Color>>();
    }

    private static int ColorsCount(List<Color> c1)
    {
        HashSet<Color> result = new HashSet<Color>();
        foreach (Color i in c1)
        {
            if (i != Color.clear)
                result.Add(i);
        }
        return result.Count;
    }

    private static bool CanDoMove(List<Color> c1, List<Color> c2, int bottle1, int bottle2)
    {
        int topColorIndexC1 = GetTopColorIndex(c1);
        int topColorIndexC2 = GetTopColorIndex(c2);

        if (bottle1 == bottle2)
            return false;

        if (ColorsCount(c1) == 0)
            return false;

        if (c2[0] != Color.clear)
            return false;

        //MISCARE REDUNDANTA: mutare o singura culoare dintr-o sticla goala in alta
        if (c2[topColorIndexC2] == Color.clear && topColorIndexC2 == c2.Count - 1 && ColorsCount(c1) < 2)
            return false;

        if (c1[topColorIndexC1] != c2[topColorIndexC2] && c2[topColorIndexC2] != Color.clear)
            return false;

        return true;
    }

    private static int GetTopColorIndex(List<Color> c1)
    {
        for (int i = 0; i < c1.Count; i++)
        {
            if (c1[i] != Color.clear)
                return i;
        }
        return c1.Count - 1;
    }

    private static void DoMove(List<Color> c1, List<Color> c2)
    {
        int topColorIndex = GetTopColorIndex(c1);
        Color topColor = c1[topColorIndex];

        int i = topColorIndex;
        int j = GetTopColorIndex(c2);

        if (c2[j] != Color.clear)
            j--;

        while (i < 4 && j >= 0 && c1[i] == topColor)
        {
            c2[j] = topColor;
            j--;
            c1[i] = Color.clear;
            i++;
        }
    }

    private static bool Solved(List<List<Color>> currentConfiguration)
    {
        for (int i = 0; i < currentConfiguration.Count; i++)
        {
            for (int j = 1; j < currentConfiguration[i].Count; j++)
            {
                if (currentConfiguration[i][0] != currentConfiguration[i][j])
                    return false;
            }
        }

        return true;
    }

    private static List<List<Color>> DeepCopyList(List<List<Color>> list)
    {
        List<List<Color>> newList = new List<List<Color>>();

        for (int i = 0; i < list.Count; i++)
        {
            for (int j = 0; j < list[i].Count; j++)
            {
                if (newList.Count < i + 1)
                    newList.Add(new List<Color>());
                newList[i].Add(list[i][j]);
            }
        }

        return newList;
    }

    private static bool Solve(List<List<Color>> currentConfiguration)
    {
        float startTime = Time.time;

        Stack<Flow> configurations = new Stack<Flow>();

        HashSet<List<List<Color>>> visitedPaths = new HashSet<List<List<Color>>>(new MyEqualityComparer());

        List<List<KeyValuePair<int, int>>> validPaths = new List<List<KeyValuePair<int, int>>>();

        Flow currFlow = new Flow();
        currFlow.currentConfiguration = DeepCopyList(currentConfiguration);
        currFlow.path = new List<KeyValuePair<int, int>>();

        configurations.Push(currFlow);

        while (configurations.Count != 0)
        {
            if (Time.time - startTime >= _SECONDS_TO_WAIT_FOR_SOLUTIONS)
                return false;

            var currentConfig = configurations.Pop();

            if (visitedPaths.Contains(currentConfig.currentConfiguration))
                continue;

            visitedPaths.Add(currentConfig.currentConfiguration);

            if (Solved(currentConfig.currentConfiguration))
            {
                validPaths.Add(currentConfig.path);
                if (validPaths.Count > _MIN_NUM_OF_SOLUTIONS) return true;
                continue;
            }

            for (int i = 0; i < currentConfig.currentConfiguration.Count; i++)
            {
                for (int j = 0; j < currentConfig.currentConfiguration.Count; j++)
                {
                    if (CanDoMove(currentConfig.currentConfiguration[i], currentConfig.currentConfiguration[j], i, j))
                    {
                        KeyValuePair<int, int> newMove = new KeyValuePair<int, int>(i, j);

                        var newConfig = DeepCopyList(currentConfig.currentConfiguration);
                        var newPath = currentConfig.path.ToList();
                        newPath.Add(newMove);

                        DoMove(newConfig[i], newConfig[j]);

                        Flow newFlow = new Flow();
                        newFlow.currentConfiguration = newConfig;
                        newFlow.path = newPath;

                        configurations.Push(newFlow);
                    }
                }
            }
        }
        return false;
    }

    private struct Flow
    {
        public List<List<Color>> currentConfiguration;
        public List<KeyValuePair<int, int>> path;
    }

    private class MyEqualityComparer : IEqualityComparer<List<List<Color>>>
    {
        public bool Equals(List<List<Color>> x, List<List<Color>> y)
        {
            if (x == null || y == null)
                return false;

            if (x.Count != y.Count)
            {
                return false;
            }
            for (int i = 0; i < x.Count; i++)
            {
                for (int j = 0; j < x[0].Count; j++)
                {
                    if (x[i][j] != y[i][j])
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        public int GetHashCode([DisallowNull] List<List<Color>> obj)
        {
            int result = 17;
            for (int i = 0; i < obj.Count; i++)
            {
                unchecked
                {
                    result = result * 23 + obj[i][0].GetHashCode();
                }
            }
            return result;
        }
    }
}
