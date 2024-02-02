using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] float _mouseSensitivity;
    [SerializeField] private float _smoothTime;
    [SerializeField] float _rotationXMinMax;
    [SerializeField] Transform PlayerRot;

    private float _rotationY;
    private float _rotationX;
    private float _Horizontal;
    private float _Vertical;
    private Vector3 _currentRotation;
    private Vector3 _smoothVelocity = Vector3.zero;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    void CameraRotation()
    {
        _rotationY += Input.GetAxis("Mouse X") * _mouseSensitivity;
        _rotationX -= Input.GetAxis("Mouse Y") * _mouseSensitivity;
        _rotationX = Mathf.Clamp(_rotationX, -_rotationXMinMax, _rotationXMinMax);
        Vector3 nextRotation = new Vector3(_rotationX, _rotationY);
        _currentRotation = Vector3.SmoothDamp(_currentRotation, nextRotation, ref _smoothVelocity, _smoothTime);
        transform.localEulerAngles = _currentRotation;
        PlayerRot.rotation = Quaternion.Euler(0, _currentRotation.x, 0);
    }
    // Update is called once per frame
    void Update()
    {
        CameraRotation();
    }
}
