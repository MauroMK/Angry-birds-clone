using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SlingshotHandler : MonoBehaviour
{
    #region Variables

    [Header("Line Renderers")]
    [SerializeField] private LineRenderer leftLineRenderer;
    [SerializeField] private LineRenderer rightLineRenderer;

    [Header("Transform References")]
    [SerializeField] private Transform leftStartPosition;
    [SerializeField] private Transform rightStartPosition;
    [SerializeField] private Transform centerPosition;
    [SerializeField] private Transform idlePosition;

    [Header("Slingshot Tweaks")]
    [SerializeField] private float maxDistance = 3.5f;

    [Header("Bird")]
    [SerializeField] private GameObject angryBirdPrefab;

    private GameObject spawnedBird;

    private SlingshotArea slingshotArea;

    private Vector2 slingshotLinesPosition;

    private bool clickedWithinArea;

    #endregion

    void Awake()
    {
        leftLineRenderer.enabled = false;
        rightLineRenderer.enabled = false;

        slingshotArea = FindObjectOfType<SlingshotArea>();

        SpawnAngryBirds();
    }

    void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame && slingshotArea.IsWithinSlingshotArea())
        {
            clickedWithinArea = true;
        }

        if (Mouse.current.leftButton.isPressed && clickedWithinArea)
        {
            DrawLines();
            PositionAndRotateBird();
        }

        if (Mouse.current.leftButton.wasReleasedThisFrame)
        {
            clickedWithinArea = false;
        }
    }

    #region Slingshot Methods

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
        if (!leftLineRenderer.enabled && !rightLineRenderer.enabled)
        {
            leftLineRenderer.enabled = true;
            rightLineRenderer.enabled = true;
        }

        leftLineRenderer.SetPosition(0, position);
        leftLineRenderer.SetPosition(1, leftStartPosition.position);

        rightLineRenderer.SetPosition(0, position);
        rightLineRenderer.SetPosition(1, rightStartPosition.position);
    }

    #endregion

    #region Angry Bird Methods

    private void SpawnAngryBirds()
    {
        SetLines(idlePosition.position);

        spawnedBird = Instantiate(angryBirdPrefab, idlePosition.position, Quaternion.identity);
    }

    private void PositionAndRotateBird()
    {
        spawnedBird.transform.position = slingshotLinesPosition;
    }

    #endregion
}
