using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Zoom Settings")]
    public float minZoom = 5f; // Minimum orthographic size (maximum zoom-in)
    public float maxZoom = 20f; // Maximum orthographic size (maximum zoom-out)
    public float zoomSpeed = 5f; // Speed of zooming

    [Header("Camera Movement Settings")]
    public float mouseMoveSpeed = 10f; // Speed of camera movement using mouse
    public float keyboardMoveSpeed = 20f; // Speed of camera movement using keyboard

    private Camera mainCamera;
    private Vector3 previousMousePosition;

    private void Awake()
    {
        mainCamera = Camera.main;
    }

    private void Update()
    {
        HandleZoom();
        HandleCameraMovement();
    }

    private void HandleZoom()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        float newSize = mainCamera.orthographicSize - scroll * zoomSpeed;

        // Clamp the zoom level to minZoom and maxZoom
        mainCamera.orthographicSize = Mathf.Clamp(newSize, minZoom, maxZoom);
    }

    private void HandleCameraMovement()
    {
        // Check if the right mouse button is held down
        if (Input.GetMouseButtonDown(1))
        {
            previousMousePosition = Input.mousePosition;
        }
        else if (Input.GetMouseButton(1))
        {
            Vector3 mouseDelta = Input.mousePosition - previousMousePosition;
            previousMousePosition = Input.mousePosition;

            // Move the camera based on mouse movement
            Vector3 moveVector = new Vector3(-mouseDelta.x, 0f, -mouseDelta.y);
            moveVector *= mouseMoveSpeed * Time.deltaTime;

            // Adjust movement speed based on the current zoom level
            moveVector *= mainCamera.orthographicSize / maxZoom;

            // Apply the movement vector
            transform.Translate(moveVector, Space.World);
        }
        else
        {
            // Handle camera movement using WASD or arrow keys
            float horizontalInput = Input.GetAxis("Horizontal");
            float verticalInput = Input.GetAxis("Vertical");

            Vector3 moveDirection = new Vector3(horizontalInput, 0f, verticalInput);
            moveDirection *= keyboardMoveSpeed * Time.deltaTime;

            // Adjust movement speed based on the current zoom level
            moveDirection *= mainCamera.orthographicSize / maxZoom;

            // Apply the movement vector
            transform.Translate(moveDirection, Space.World);
        }
    }
}
