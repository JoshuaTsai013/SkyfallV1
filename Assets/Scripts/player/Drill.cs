using UnityEngine;

public class Drill : MonoBehaviour
{
    public float DrillSpeed = 2000;
    private bool _isDrilling;
    private void Start()
    {
        // Set the drill to stop
        DrillStop();
    }
    public void DrillStart()
    {
        Debug.Log("DrillRotate!!");
        _isDrilling = true;
        // Rotate the drill
    }

    public void DrillStop()
    {
        Debug.Log("DrillStop!!");
        _isDrilling = false;
    }

    private void Update()
    {
        if (!_isDrilling)
        {
            return;
        }

        transform.Rotate(2000 * Time.deltaTime * Vector3.right);
    }
}
