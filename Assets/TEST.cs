using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TEST : MonoBehaviour
{
    public Bottle _bottle;

    private void Start()
    {
        _bottle.SetColors(new List<Color> { Color.red, Color.blue, Color.green, Color.white }) ;
    }
}
