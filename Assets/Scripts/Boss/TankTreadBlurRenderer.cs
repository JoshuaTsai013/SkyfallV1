using UnityEngine;
using System.Collections.Generic;

// Ensures the GameObject has a Renderer component
[RequireComponent(typeof(Renderer))]
public class TankTreadBlurRenderer : MonoBehaviour
{
    [Header("Blur Parameters")]
    [Range(1, 64)] public int blurFrames = 4;     // How many previous positions to track
    [Range(1, 32)] public int samples = 4;        // Number of blur samples to render
    public float blurLength = 1.0f;               // Length of the blur effect
    public float alphaFalloff = 1.1f;             // Controls blur opacity falloff
    public float forwardOffset = 0.5f;            // Offset in the direction of movement
    public float verticalOffset = 0.2f;           // Vertical offset from the ground
    public Material blurMaterial;                 // Material used for rendering blur effect

    public MeshFilter sourceMeshFilter;           // Source mesh to create blur from

    private Queue<Vector3> positionHistory = new Queue<Vector3>();  // Stores recent positions
    private Mesh mesh;                            // Reference to the source mesh
    private Renderer rend;                        // Component reference
    private MaterialPropertyBlock mpb;            // For efficient material property changes

    void Start()
    {
        // Validate required components
        if (sourceMeshFilter == null)
        {
            Debug.LogError("TankTreadBlurRenderer not assign sourceMeshFilter");
            enabled = false;
            return;
        }

        // Cache references for better performance
        mesh = sourceMeshFilter.sharedMesh;
        rend = GetComponent<Renderer>();
        mpb = new MaterialPropertyBlock();
    }

    void Update()
    {
        // Maintain position history, removing oldest when at capacity
        if (positionHistory.Count >= blurFrames)
            positionHistory.Dequeue();

        // Add current position to history
        positionHistory.Enqueue(sourceMeshFilter.transform.position);
    }

    void LateUpdate()
    {
        // Skip rendering if prerequisites aren't met or not enough position data
        if (blurMaterial == null || mesh == null || positionHistory.Count < 2) return;

        // Convert queue to array for easier access
        Vector3[] positions = positionHistory.ToArray();
        Vector3 latest = positions[positions.Length - 1];
        Vector3 earliest = positions[0];

        // Calculate motion vector and speed
        Vector3 motion = latest - earliest;
        float speed = motion.magnitude;
        if (speed < 0.0001f) return;  // Skip if barely moving

        // Calculate rendering parameters
        Vector3 midpoint = (latest + earliest) * 0.5f;
        Vector3 direction = motion.normalized;
        float offsetStrength = Mathf.Clamp01(speed / 0.1f);  // Scale offset based on speed
        Vector3 forwardShift = forwardOffset * offsetStrength * direction;

        Transform src = sourceMeshFilter.transform;

        // Render multiple samples along the motion path
        for (int i = -samples / 2; i <= samples / 2; i++)
        {
            float t = i / (float)samples;
            float fade = Mathf.Pow(1.0f - Mathf.Abs(t), alphaFalloff);  // Calculate opacity

            // Position the blur sample
            Vector3 pos = midpoint + motion * t + forwardShift + src.up * verticalOffset;

            // Create transformation matrix
            Matrix4x4 mat = Matrix4x4.TRS(pos, src.rotation, src.lossyScale);
            mpb.SetColor("_BaseColor", new Color(0.2f, 0.2f, 0.2f, fade));

            // Draw the mesh with the current properties
            Graphics.DrawMesh(mesh, mat, blurMaterial, gameObject.layer, null, 0, mpb);
        }
    }
}
