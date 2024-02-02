using UnityEngine;

public class _Player_Move : MonoBehaviour
{
    [SerializeField] float player_speed;
    [SerializeField] float player_speed_run;
    [SerializeField] LayerMask mask;
    [SerializeField] float jump;
    [SerializeField] Camera _camera;

    Ray hit;
    CapsuleCollider capsuleCollider;
    Rigidbody rb;
    float result_speed;
    private float _Horizontal;
    private float _Vertical;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        capsuleCollider = GetComponent<CapsuleCollider>();
        _camera = _camera == null ? Camera.main : _camera;
    }
    void movement()
    {
        _Horizontal = Input.GetAxis("Horizontal");
        _Vertical = Input.GetAxis("Vertical");
        if (_Horizontal != 0 || _Vertical != 0)
        {
            result_speed = Input.GetKey(KeyCode.LeftShift) ? player_speed_run : player_speed;
            var move = Quaternion.Euler(0, _camera.transform.eulerAngles.y, 0) * new Vector3(_Horizontal, -0.1f, _Vertical) * result_speed * Time.deltaTime;
            rb.MovePosition(transform.position +  move);
        }
        Jump();
        if (is_grounded())
        {
            rb.drag = 5;
        }
        else
        {
            rb.drag = 0;
        }
    }
    void Jump()
    {
        if(is_grounded() && Input.GetKeyDown(KeyCode.Space))
        {
            rb.AddForce(Vector3.up * jump,ForceMode.Impulse);
        }
    }
    bool is_grounded()
    {
        return Physics.Raycast(transform.position, Vector3.down, capsuleCollider.height  / 2 + 0.2f , mask);
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        movement();
    }
}
