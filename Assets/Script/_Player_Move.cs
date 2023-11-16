using System.Collections;
using System.Collections.Generic;
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
    private Vector3 _currentRotation;
    private Vector3 _smoothVelocity = Vector3.zero;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    void movement()
    {
        _rotationY += Input.GetAxis("Mouse X") * _mouseSensitivity;
        _rotationX -= Input.GetAxis("Mouse Y") * _mouseSensitivity;
        _rotationX = Mathf.Clamp(_rotationX, -_rotationXMinMax, _rotationXMinMax);
        Vector3 nextRotation = new Vector3(_rotationX, _rotationY);
        _currentRotation = Vector3.SmoothDamp(_currentRotation, nextRotation, ref _smoothVelocity, _smoothTime);
        Camera.main.transform.localEulerAngles = _currentRotation;
        result_speed = Input.GetKey(KeyCode.LeftShift) ? player_speed_run : player_speed;
        var move = Quaternion.Euler(0, Camera.main.transform.eulerAngles.y, 0) * new Vector3(Input.GetAxis("Horizontal"), -0.1f, Input.GetAxis("Vertical")) * result_speed * Time.deltaTime;
        rb.MovePosition(transform.position + move);
    }
    // Update is called once per frame
    void Update()
    {
        movement();
    }
}
