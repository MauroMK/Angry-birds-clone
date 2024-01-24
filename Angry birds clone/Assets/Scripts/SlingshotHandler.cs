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
        SetLines(touchPosition);
    }

    private void SetLines(Vector2 position)
    {
        leftLineRenderer.SetPosition(0, position);
        leftLineRenderer.SetPosition(1, leftStartPosition.position);

        rightLineRenderer.SetPosition(0, position);
        rightLineRenderer.SetPosition(1, rightStartPosition.position);
    }
}
