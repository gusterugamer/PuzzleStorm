using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "PuzzleStorm/Puzzles/WaterSortPuzzle")]
public class WaterSortPuzzle : Puzzle
{
    // Define a list to store the generated levels
    public List<List<List<string>>> levels = new List<List<List<string>>>();

    public static event UnityAction<List<List<List<string>>>> onPuzzleLoaded;

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
        GenerateLevel();
        onPuzzleLoaded?.Invoke(levels);
    }

    protected override void ClearLevel()
    {
       
    }

    protected override void GenerateLevel()
    {
        // Define the list of colors to use in the levels
        List<string> colors = new List<string> { "Red", "Blue", "Green", "Yellow", "Purple", "Orange", "Pink", "Gray", "White" };

        // Define the number of bottles and colors to use in the levels
        int numBottles = 8;
        int numColors = 8;
        int numLevels = 3;

        // Generate the levels
        for (int i = 0; i < numLevels; i++)
        {
            // Create a new level
            List<List<string>> level = new List<List<string>>();

            // Create the bottles for the level
            for (int j = 0; j < numBottles; j++)
            {
                // Create a new bottle for the level
                List<string> bottle = new List<string>();

                // Determine the difficulty of the level
                float difficulty = (i + 1) / (float)numLevels;

                // Fill the bottle with colors using a random sampling algorithm
                bottle.AddRange(colors.OrderBy(c => Random.Range(0, numColors)).Take((int)(numColors * difficulty)));

                // Shuffle the colors in the bottle to increase the difficulty of the level
                for (int m = 0; m < (int)(bottle.Count * difficulty); m++)
                {
                    int index1 = Random.Range(0, bottle.Count);
                    int index2 = Random.Range(0, bottle.Count);
                    string temp = bottle[index1];
                    bottle[index1] = bottle[index2];
                    bottle[index2] = temp;
                }

                // Add the bottle to the level
                level.Add(bottle);
            }

            // Add the level to the list of levels
            levels.Add(level);
        }

        // Output the generated levels
        foreach (List<List<string>> level in levels)
        {
            Debug.Log("Level:");
            foreach (List<string> bottle in level)
            {
                Debug.Log("Bottle: " + string.Join(", ", bottle));
            }
        }
    }
}
