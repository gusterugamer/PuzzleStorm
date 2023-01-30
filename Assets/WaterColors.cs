using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public sealed class WaterColors : MonoBehaviour
{
    SpriteRenderer _renderer = null;

    private float _HEIGHT;

    private void Awake()
    {
        _renderer = GetComponent<SpriteRenderer>();

        //calculate sprite height in worldSpace
        _HEIGHT = Mathf.Abs(_renderer.bounds.max.y - _renderer.bounds.min.y);
    }

    private void Update()
    {
        //Send length to shader
        Shader.SetGlobalFloat("Bottle_Height", _HEIGHT);
    }

    public void SetColor(int index, Color color)
    {
        //Sets the color in the shader instance based on the Reference values of color in shadergraph
        _renderer.material.SetColor("_Color" + index, color);
    }

    public void SetFillAmount(float fill, float rotationMultiplier)
    {
        _renderer.material.SetFloat("Fill_Amount",fill);
        _renderer.material.SetFloat("Rotation_Multiplier", rotationMultiplier);
    }
}
