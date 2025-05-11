using UnityEngine;
using System.Collections.Generic;
using System;

public class Aimbot : MonoBehaviour
{
    [Header("Aim Assist Settings")]
    [SerializeField] private float assistStrength = 5f; // Higher values = faster aim correction
    [SerializeField] private LayerMask targetLayers; // Set to layers containing enemies
    public Vector3 targetOffset = new(0, 1.5f, 0); // Aim at head level
    [SerializeField] private bool debugMode = false;

    [Header("Screen Raycast Settings")]
    public float screenScanRadius = 0.3f; // Radius around screen center to scan (0-1 range)
    [SerializeField] private int scanResolution = 9; // Number of rays to cast (higher = more accurate but more expensive)
    [SerializeField] private float maxRayDistance = 100f; // Maximum distance for raycasts

    [Header("References")]
    [SerializeField] private Camera mainCamera; // Reference to the main camera

    private ThirdPersonShooterController shooterController;
    private ThirdPersonController playerController;
    private PlayerInputs playerInputs;
    public List<Transform> potentialTargets = new();
    private Transform currentTarget;
    private MeleeAttack meleeAttack;

    void Start()
    {
        if (mainCamera == null)
            mainCamera = Camera.main;

        shooterController = GetComponent<ThirdPersonShooterController>();
        playerController = GetComponent<ThirdPersonController>();
        playerInputs = GetComponent<PlayerInputs>();
        meleeAttack = GetComponent<MeleeAttack>();
    }

    void Update()
    {
        // Reset the aimbot assist values at the beginning of each frame
        if (playerController != null)
        {
            playerController.AimbotAssistX = 0f;
            playerController.AimbotAssistY = 0f;
        }

        // Only activate aim assist when the player is aiming
        if ((playerInputs.isUsingController && shooterController.isAiming) || meleeAttack.IsDrilling)
        {
            FindPotentialTargets();

            // Find the most suitable target
            Transform bestTarget = GetNearestTargetInSight();

            // Apply aim assist if we have a valid target
            if (bestTarget != null)
            {
                ApplyAimAssist(bestTarget);
            }

            // Debug visualization
            if (debugMode && currentTarget != null)
            {
                Debug.DrawLine(mainCamera.transform.position, currentTarget.position + targetOffset, Color.red);
            }
        }
    }

    private void FindPotentialTargets()
    {
        potentialTargets.Clear();

        if (mainCamera == null) return;

        // Cast rays in a grid pattern around the center of the screen
        for (int x = 0; x < scanResolution; x++)
        {
            for (int y = 0; y < scanResolution; y++)
            {
                // Calculate screen position for this ray
                float screenX = 0.5f + screenScanRadius * ((float)x / (scanResolution - 1) - 0.5f) * 2f;
                float screenY = 0.5f + screenScanRadius * ((float)y / (scanResolution - 1) - 0.5f) * 2f;

                Vector3 screenPoint = new Vector3(screenX, screenY, 0);
                Ray ray = mainCamera.ViewportPointToRay(screenPoint);

                // Perform the raycast
                if (Physics.Raycast(ray, out RaycastHit hit, maxRayDistance, targetLayers))
                {
                    if (hit.collider.CompareTag("Enemy"))
                    {
                        // Check if we already have this target
                        Transform hitTransform = hit.collider.transform;
                        if (!potentialTargets.Contains(hitTransform))
                        {
                            potentialTargets.Add(hitTransform);

                            if (debugMode)
                            {
                                Debug.DrawLine(ray.origin, hit.point, Color.yellow, 0.1f);
                            }
                        }
                    }
                }
                else if (debugMode)
                {
                    // Draw debug ray when no hit
                    Debug.DrawRay(ray.origin, ray.direction * maxRayDistance, Color.grey, 0.1f);
                }
            }
        }
    }

    private Transform GetNearestTargetInSight()
    {
        Transform nearestTarget = null;
        float nearestDistance = float.MaxValue;

        // Screen center point
        Vector3 screenCenter = new(0.5f, 0.5f, 0);

        foreach (var target in potentialTargets)
        {
            // Convert target position to screen position
            Vector3 targetScreenPos = mainCamera.WorldToViewportPoint(target.position + targetOffset);

            // Skip if behind camera
            if (targetScreenPos.z <= 0) continue;

            // Calculate distance from screen center
            float screenDistance = Vector2.Distance(targetScreenPos, screenCenter);

            // Check if target is within assist angle threshold (now in screen space)
            if (screenDistance <= screenScanRadius)
            {
                float distanceToCamera = Vector3.Distance(mainCamera.transform.position, target.position);

                if (distanceToCamera < nearestDistance)
                {
                    nearestTarget = target;
                    nearestDistance = distanceToCamera;
                }
            }
        }

        currentTarget = nearestTarget;
        return nearestTarget;
    }

    private void ApplyAimAssist(Transform target)
    {
        if (playerController == null) return;

        // Calculate target position with offset
        Vector3 targetPos = target.position + targetOffset;

        // Get the direction to the target from the camera
        Vector3 cameraToTarget = targetPos - mainCamera.transform.position;

        // Convert the world direction to a rotation
        Quaternion targetRotation = Quaternion.LookRotation(cameraToTarget);

        // Calculate the difference between current camera rotation and target rotation
        float deltaYaw = Mathf.DeltaAngle(mainCamera.transform.eulerAngles.y, targetRotation.eulerAngles.y);
        float deltaPitch = Mathf.DeltaAngle(mainCamera.transform.eulerAngles.x, targetRotation.eulerAngles.x);

        // Apply a constant rate of adjustment instead of lerping
        float adjustX = Mathf.Sign(deltaYaw) * Mathf.Min(Math.Abs(deltaYaw) * 0.1f, 0.5f) * assistStrength;
        float adjustY = Mathf.Sign(deltaPitch) * Mathf.Min(Math.Abs(deltaPitch) * 0.1f, 0.5f) * assistStrength;

        // Set this frame's adjustment values
        playerController.AimbotAssistX = adjustX;
        playerController.AimbotAssistY = adjustY;

        // If in debug mode, show the aim direction
        if (debugMode)
        {
            Debug.DrawLine(mainCamera.transform.position, targetPos, Color.yellow);
        }
    }

    private void OnDrawGizmos()
    {
        if (!debugMode) return;

        // Visualize scan area on screen
        if (mainCamera != null && Application.isPlaying)
        {
            Gizmos.color = Color.cyan;
            Vector3 screenCenter = mainCamera.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 10));

            // Draw a cross at screen center
            float crossSize = 0.1f;
            Gizmos.DrawLine(
                screenCenter + mainCamera.transform.right * crossSize,
                screenCenter - mainCamera.transform.right * crossSize
            );
            Gizmos.DrawLine(
                screenCenter + mainCamera.transform.up * crossSize,
                screenCenter - mainCamera.transform.up * crossSize
            );

            // Draw outline of scan area instead of all points
            float screenRadius = screenScanRadius * 1.5f;  // Make it slightly larger for visibility
            Vector3[] corners = new Vector3[4];
            corners[0] = mainCamera.ViewportToWorldPoint(new Vector3(0.5f - screenRadius, 0.5f - screenRadius, 10));
            corners[1] = mainCamera.ViewportToWorldPoint(new Vector3(0.5f + screenRadius, 0.5f - screenRadius, 10));
            corners[2] = mainCamera.ViewportToWorldPoint(new Vector3(0.5f + screenRadius, 0.5f + screenRadius, 10));
            corners[3] = mainCamera.ViewportToWorldPoint(new Vector3(0.5f - screenRadius, 0.5f + screenRadius, 10));

            // Draw the outline square
            for (int i = 0; i < 4; i++)
            {
                Gizmos.DrawLine(corners[i], corners[(i + 1) % 4]);
            }
        }
    }
}
