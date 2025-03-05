using UnityEngine;

public class UILookatCamera : MonoBehaviour
{
    [SerializeField]
    private GameObject _mainCamera;
    private void Awake()
    {
        // get a reference to our main camera
        if (_mainCamera == null)
        {
            _mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        }
    }

    // Update is called once per frame
    void LateUpdate()
    {
        transform.LookAt(_mainCamera.transform);
    }
}
