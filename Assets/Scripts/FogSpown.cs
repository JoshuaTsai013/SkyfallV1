using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class FogSpown : MonoBehaviour
{
    public float spawnHeight;
    public float heightVariation;
    public float spawnAreaX; // Width of the spawn area
    public float spawnAreaZ; // Depth of the spawn area
    public int fogAmount;
    public GameObject fogObject;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < fogAmount; i++)
        {
            Vector3 spawnPos = new Vector3(
                Random.Range(transform.position.x - spawnAreaX, transform.position.x + spawnAreaX),
                Random.Range(spawnHeight - heightVariation, spawnHeight + heightVariation),
                Random.Range(transform.position.z - spawnAreaZ, transform.position.z + spawnAreaZ)
            );
            Instantiate(fogObject, spawnPos, quaternion.identity, this.transform);
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Vector3 topLeft = new Vector3(transform.position.x - spawnAreaX, transform.position.y, transform.position.z + spawnAreaZ);
        Vector3 topRight = new Vector3(transform.position.x + spawnAreaX, transform.position.y, transform.position.z + spawnAreaZ);
        Vector3 bottomLeft = new Vector3(transform.position.x - spawnAreaX, transform.position.y, transform.position.z - spawnAreaZ);
        Vector3 bottomRight = new Vector3(transform.position.x + spawnAreaX, transform.position.y, transform.position.z - spawnAreaZ);

        Gizmos.DrawLine(topLeft, topRight);
        Gizmos.DrawLine(topRight, bottomRight);
        Gizmos.DrawLine(bottomRight, bottomLeft);
        Gizmos.DrawLine(bottomLeft, topLeft);
    }
}
