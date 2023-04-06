using UnityEngine;

public class PlayerLook : MonoBehaviour
{
    [SerializeField] private Transform _viewHolder;
    [SerializeField] private float _sensitivity = 1f;
    private float _camPitch;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * _sensitivity; // horizontal look, rotate entire body along the y axis
        float mouseY = Input.GetAxis("Mouse Y") * _sensitivity; // vertical look, rotate only the cameraHolder along the x axis

        transform.Rotate(Vector3.up * mouseX);

        _camPitch = Mathf.Clamp(_camPitch - mouseY, -90f, 90f);
        _viewHolder.localEulerAngles = Vector3.right * _camPitch; 
    }
}
