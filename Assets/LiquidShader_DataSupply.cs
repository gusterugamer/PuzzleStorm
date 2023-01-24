using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public sealed class LiquidShader_DataSupply : MonoBehaviour
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

    public void OnDrawGizmosSelected()
    {
        var r = GetComponent<Renderer>();
        if (r == null)
            return;
        var bounds = r.localBounds;
        Gizmos.matrix = transform.localToWorldMatrix;
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(bounds.center, bounds.extents * 2);
    }


#if UNITY_EDITOR

    //private void OnValidate()
    //{
    //    EditorApplication.update += UpateEditor;
    //}

    //private void UpateEditor()
    //{
    //    //Apply the height while in editor to see the colors properly
    //    SpriteRenderer renderer = GetComponent<SpriteRenderer>();
    //    float height = Mathf.Abs(renderer.bounds.max.y - renderer.bounds.min.y);
    //    Shader.SetGlobalFloat("Bottle_Height", height);
    //}

#endif
}
