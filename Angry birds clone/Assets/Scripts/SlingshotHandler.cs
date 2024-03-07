using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using DG.Tweening;

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
    [SerializeField] private Transform elasticTransform;

    [Header("Slingshot Tweaks")]
    [SerializeField] private float maxDistance = 3.5f;
    [SerializeField] private float shotForce;
    [SerializeField] private float timeBetweenBirdRespawn = 2f;
    [SerializeField] private float elasticDivider = 1.2f;
    [SerializeField] private AnimationCurve elasticCurve;

    [Header("Bird")]
    [SerializeField] private Angrybird angryBirdPrefab;
    [SerializeField] private float birdOffset;

    [Header("Directions")]
    private Vector2 slingshotLinesPosition;

    private Vector2 direction;
    private Vector2 directionNormalized;

    private Angrybird spawnedBird;

    private SlingshotArea slingshotArea;

    private bool clickedWithinArea;
    private bool birdOnSlingShot;

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
        if (InputManager.wasLeftMouseButtonPressed && slingshotArea.IsWithinSlingshotArea())
        {
            clickedWithinArea = true;
        }

        if (InputManager.isLeftMouseButtonPressed && clickedWithinArea && birdOnSlingShot)
        {
            DrawLines();
            PositionAndRotateBird();
        }

        if (InputManager.wasLeftMouseButtonReleased && birdOnSlingShot && clickedWithinArea)
        {
            if (GameManager.instance.HasEnoughShots())
            {
                clickedWithinArea = false;
                birdOnSlingShot = false;

                spawnedBird.LauchBird(direction, shotForce);
                GameManager.instance.UseShot();
                AnimateSlingshot();

                if(GameManager.instance.HasEnoughShots())
                {
                    StartCoroutine(SpawnAngryBirdAfterTime());
                }
            }
        }
    }

    #region Slingshot Methods

    private void DrawLines()
    {
        // Gets the screen position of the mouse
        Vector3 touchPosition = Camera.main.ScreenToWorldPoint(InputManager.mousePosition);

        // ClampMagnitude doesn't let the vector pass certain lenght
        slingshotLinesPosition = centerPosition.position + Vector3.ClampMagnitude(touchPosition - centerPosition.position, maxDistance);
        
        SetLines(slingshotLinesPosition);

        // Casting the Vector2 before centralPosition.position to transform the Vector3 into Vector2
        direction = (Vector2)centerPosition.position - slingshotLinesPosition;
        directionNormalized = direction.normalized;
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

        Vector2 dir = (centerPosition.position - idlePosition.position).normalized;
        Vector2 spawnPosition = (Vector2)idlePosition.position + dir * birdOffset;

        spawnedBird = Instantiate(angryBirdPrefab, spawnPosition, Quaternion.identity);
        spawnedBird.transform.right = dir;

        birdOnSlingShot = true;
    }

    private void PositionAndRotateBird()
    {
        spawnedBird.transform.position = slingshotLinesPosition + directionNormalized * birdOffset;
        spawnedBird.transform.right = directionNormalized;
    }

    private IEnumerator SpawnAngryBirdAfterTime()
    {
        yield return new WaitForSeconds(timeBetweenBirdRespawn);

        SpawnAngryBirds();
    }

    #endregion

    #region Animate Slingshot

    private void AnimateSlingshot()
    {
        elasticTransform.position = leftLineRenderer.GetPosition(0);

        float dist = Vector2.Distance(elasticTransform.position, centerPosition.position);

        float time = dist / elasticDivider;

        elasticTransform.DOMove(centerPosition.position, time).SetEase(elasticCurve);
        StartCoroutine(AnimateSlingShotLines(elasticTransform, time));
    }

    private IEnumerator AnimateSlingShotLines(Transform trans, float time)
    {
        float elapsedTime = 0f;
        while (elapsedTime < time)
        {
            elapsedTime += Time.deltaTime;

            SetLines(trans.position);

            yield return null;
        }
    }

    #endregion
}
