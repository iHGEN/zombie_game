using UnityEngine;

public class _Player_Move : MonoBehaviour
{
    Rigidbody rb;
    [SerializeField] float player_speed;
    [SerializeField] float player_speed_run;
    [SerializeField] float _mouseSensitivity;
    [SerializeField] private float _smoothTime;
    [SerializeField] float _rotationXMinMax;
    float result_speed;
    private float _rotationY;
    private float _rotationX;
    private float _Horizontal;
    private float _Vertical;
    private Vector3 _currentRotation;
    private Vector3 _smoothVelocity = Vector3.zero;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked;
    }
    void movement()
    {
        _Horizontal = Input.GetAxis("Horizontal");
        _Vertical = Input.GetAxis("Vertical");
        _rotationY += Input.GetAxis("Mouse X") * _mouseSensitivity;
        _rotationX -= Input.GetAxis("Mouse Y") * _mouseSensitivity;
        if (_Horizontal != 0 || _Vertical != 0 || _rotationX != 0 || _rotationY != 0)
        {
            _rotationX = Mathf.Clamp(_rotationX, -_rotationXMinMax, _rotationXMinMax);
            Vector3 nextRotation = new Vector3(_rotationX, _rotationY);
            _currentRotation = Vector3.SmoothDamp(_currentRotation, nextRotation, ref _smoothVelocity, _smoothTime);
            Camera.main.transform.localEulerAngles = _currentRotation;
            result_speed = Input.GetKey(KeyCode.LeftShift) ? player_speed_run : player_speed;
            var move = Quaternion.Euler(0, Camera.main.transform.eulerAngles.y, 0) * new Vector3(_Horizontal, -0.1f, _Vertical) * result_speed * Time.deltaTime;
            rb.MovePosition(transform.position + move);
        }
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        movement();
    }
}
