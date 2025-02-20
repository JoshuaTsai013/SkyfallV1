using UnityEngine;

public class GunMovement : MonoBehaviour
{
    [SerializeField]
    private GameObject _mainCamera;
    [SerializeField]
    private Transform _gunRoot;
    [SerializeField]
    private PlayerInputs _inputs;
    // Start is called before the first frame update
    private void Awake()
    {
        // get a reference to our main camera
        if (_mainCamera == null)
        {
            _mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        }
    }
    private void Start()
    {
        transform.rotation = _gunRoot.rotation;
    }
    // Update is called once per frame
    private void FixedUpdate()
    {
        transform.rotation = _gunRoot.rotation;
    }
}
