using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PuzzleStorm/Utility/WordsGridBounds")]

public sealed class WordsGridBounds : ScriptableObject
{
    [BoxGroup("DeviceScaler")][SerializeField] private DeviceAspectScaler _aspectScaler;

    private readonly Vector2 _TOP_LEFT_AS_PERCENT_OF_CAMERA_ORTHO_SIZE = new Vector2(0f, 0.823f);
    private readonly Vector2 _BOTTOM_RIGHT_AS_PERCENT_OF_CAMERA_ORTHO_SIZE = new Vector2(1f,0.153f);

    private const float _GRID_PADDING = 0.15f;

    private Vector2 topLeft = Vector2.zero;
    private Vector2 bottomRight = Vector2.zero;

    public Vector2 TopLeft => topLeft;
    public Vector2 BottomRight => bottomRight;

    public void Init()
    {
        float viewWidth = _aspectScaler.GetViewWidth();
        float viewHeight = 2f * Camera.main.orthographicSize;

        topLeft = new Vector2(viewWidth * _TOP_LEFT_AS_PERCENT_OF_CAMERA_ORTHO_SIZE.x, 
                              viewHeight * _TOP_LEFT_AS_PERCENT_OF_CAMERA_ORTHO_SIZE.y);
        topLeft.x += _GRID_PADDING;
        topLeft.y -= _GRID_PADDING;

        bottomRight = new Vector2(viewWidth * _BOTTOM_RIGHT_AS_PERCENT_OF_CAMERA_ORTHO_SIZE.x,
                                  viewHeight * _BOTTOM_RIGHT_AS_PERCENT_OF_CAMERA_ORTHO_SIZE.y);

        bottomRight.x -= _GRID_PADDING;
        bottomRight.y += _GRID_PADDING;
    }

    public Vector2 GetSize()
    {
        return new Vector2(bottomRight.x -  topLeft.x, topLeft.y - bottomRight.y);
    }
}
