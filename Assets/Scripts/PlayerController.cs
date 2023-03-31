using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody _body;

    [SerializeField] private float _speed = 5f;

    [SerializeField] private float _runSpeed = 10f;

    [SerializeField] private Transform _camHolder;
    [SerializeField] private float _mouseSensitivity = 1f;

    private float _camPitch = 0f;

    private void Start()
    {
        _body = GetComponent<Rigidbody>();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void LateUpdate() // look
    {
        float mouseX = Input.GetAxis("Mouse X") * _mouseSensitivity; // horizontal look, rotate entire body along the y axis
        float mouseY = Input.GetAxis("Mouse Y") * _mouseSensitivity; // vertical look, rotate only the cameraHolder along the x axis

        transform.Rotate(Vector3.up * mouseX);

        _camPitch = Mathf.Clamp(_camPitch - mouseY, -90f, 90f);
        _camHolder.localEulerAngles = Vector3.right * _camPitch; 
    }

    private void FixedUpdate() // move
    {
        Vector3 moveVec = 
            (transform.forward * Input.GetAxisRaw("Vertical") + transform.right * Input.GetAxisRaw("Horizontal"))
            .normalized * Time.fixedDeltaTime * _speed;
        
        _body.MovePosition(_body.position + moveVec);
    }
}
