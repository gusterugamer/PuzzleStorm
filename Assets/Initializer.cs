using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Initializer : MonoBehaviour
{
    [BoxGroup("Puzzle")][SerializeField] private Puzzle _puzzle;

    private void Start()
    {
        //The order of these calls matters
        InitCameraPosition();
        _puzzle.Set();
    }

    //Calculate the X position of the camera so the camera frustum is always on the positive side of the X axis
    //to make instantiation of objects easier
    private void InitCameraPosition()
    {
        Camera mainCamera = Camera.main;

        float aspectRatio = mainCamera.aspect;
        float cameraX = aspectRatio * mainCamera.orthographicSize;

        mainCamera.transform.position = new Vector3(cameraX, mainCamera.transform.position.y, -1.0f); // -1.0f to make sure objects are inside the frustum
    }
}
