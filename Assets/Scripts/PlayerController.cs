using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float _walkSpeed = 5f;
    [SerializeField] private float _runSpeed = 8f;
    private Rigidbody _body;

    public Rigidbody Body { get { return _body; } }


    [Header("View")]
    [SerializeField] private Transform _camHolder;
    [SerializeField] private float _mouseSensitivity = 1f;
    private float _camPitch = 0f;


    [Header("Flashlight")]
    [SerializeField] private Flashlight _flash;
    private Transform _flashTr;


    [Header("Flashlight Sway")]
    [SerializeField] private float _swayMultiplier = 2f;
    [SerializeField] private float _swaySmooth = 8f;


    [Header("Stamina")]
    [SerializeField] private float _staminaDrainRate = 0.5f;
    [SerializeField] private float _staminRechargeRate = 0.5f;
    [SerializeField] private float _staminaRechargeDelay = 1.5f; // how many seconds of not running it takes for stamina to recharge
    private float _stamina = 100f;
    private float _lastRunTime; // last point in time where player ran.


    [Header("Temp SHIT")]
    [SerializeField] private Text _staminaText;

    private void SetStaminaText() { _staminaText.text = $"Stamina: {_stamina:.00}"; }


    private void Start()
    {
        _body = GetComponent<Rigidbody>();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        _lastRunTime = Time.time;

        _flashTr = _flash.transform;

        SetStaminaText();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            _flash.Toggle();
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            _flash.HAX(); // TEMP
        }

        float mouseX = Input.GetAxisRaw("Mouse X"); // horizontal look, rotate entire body along the y axis
        float mouseY = Input.GetAxisRaw("Mouse Y"); // vertical look, rotate only the cameraHolder along the x axis

        transform.Rotate(Vector3.up * mouseX * _mouseSensitivity);

        _camPitch = Mathf.Clamp(_camPitch - mouseY * _mouseSensitivity, -90f, 90f);
        _camHolder.localEulerAngles = Vector3.right * _camPitch; 


        // Flashlight Sway
        Quaternion swayRotation = Quaternion.AngleAxis(mouseX * _swayMultiplier, Vector3.up) * Quaternion.AngleAxis(-mouseY * _swayMultiplier, Vector3.right);
        _flashTr.localRotation = Quaternion.Slerp(_flashTr.localRotation, swayRotation, _swaySmooth * Time.deltaTime); 
    }

    private void FixedUpdate() // move
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveZ = Input.GetAxisRaw("Vertical");
        float speed;

        bool standstill = moveX == 0f && moveZ == 0f;

        if (Input.GetKey(KeyCode.LeftShift) && !standstill) 
        {
            if (_stamina > 0f)
            {
                speed = _runSpeed;
                _stamina -= _staminaDrainRate;
            }
            else
            {
                speed = _walkSpeed;
            }

            _lastRunTime = Time.time; // Have to completely let go of shift (if moving) for stamina recharge
        }
        else 
        {
            if (_stamina < 100f && Time.time >= _lastRunTime + _staminaRechargeDelay)
            {
                _stamina += _staminRechargeRate;
            }

            speed = _walkSpeed;
        }

        _stamina = Mathf.Clamp(_stamina, 0f, 100f);
        SetStaminaText();

        if (!standstill)
        {   
            Vector3 moveVec = 
                (transform.forward * moveZ + transform.right * moveX).normalized * Time.fixedDeltaTime * speed;
            _body.MovePosition(_body.position + moveVec);
        }
    }


    public void Die()
    {
        _body.useGravity = true;
        _body.constraints = RigidbodyConstraints.None;
        enabled = false;
    }
}
