using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SlingshotArea : MonoBehaviour
{
    [SerializeField] private LayerMask slingshotAreaMask;

    public bool IsWithinSlingshotArea()
    {
        // Gets the screen position of the mouse
        Vector2 worldPosition = Camera.main.ScreenToWorldPoint(InputManager.mousePosition);

        // Checks if the physics are touching some collider
        if (Physics2D.OverlapPoint(worldPosition, slingshotAreaMask))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
