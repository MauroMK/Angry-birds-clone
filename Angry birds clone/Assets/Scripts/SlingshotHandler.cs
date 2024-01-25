using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SlingshotHandler : MonoBehaviour
{
    [SerializeField] private LineRenderer leftLineRenderer;
    [SerializeField] private LineRenderer rightLineRenderer;

    [SerializeField] private Transform leftStartPosition;
    [SerializeField] private Transform rightStartPosition;
    [SerializeField] private Transform centerPosition;
    [SerializeField] private Transform idlePosition;

    [SerializeField] private float maxDistance = 3.5f;

    private Vector2 slingshotLinesPosition;

    void Update()
    {
        if (Mouse.current.leftButton.isPressed)
        {
            DrawLines();
        }
    }

    private void DrawLines()
    {
        // Gets the screen position of the mouse
        Vector3 touchPosition = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());

        // ClampMagnitude doesn't let the vector pass certain lenght
        slingshotLinesPosition = centerPosition.position + Vector3.ClampMagnitude(touchPosition - centerPosition.position, maxDistance);
        
        SetLines(slingshotLinesPosition);
    }

    private void SetLines(Vector2 position)
    {
        leftLineRenderer.SetPosition(0, position);
        leftLineRenderer.SetPosition(1, leftStartPosition.position);

        rightLineRenderer.SetPosition(0, position);
        rightLineRenderer.SetPosition(1, rightStartPosition.position);
    }
}
