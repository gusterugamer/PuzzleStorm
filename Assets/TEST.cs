using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TEST : MonoBehaviour
{
    public Bottle _bottle;
    public Bottle _bottle2;

    private void Start()
    {
        _bottle.SetColors(new Color[] { Color.red, Color.blue, Color.cyan, Color.cyan }) ;
        _bottle2.SetColors(new Color[] { Color.green, Color.cyan, Color.clear, Color.clear }) ;
    }
}