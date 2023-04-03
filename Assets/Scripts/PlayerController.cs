using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    #region Movement
    private Rigidbody _body;
    [SerializeField] private float _walkSpeed = 5f;
    [SerializeField] private float _runSpeed = 8f;
    #endregion


    #region Look
    private float _camPitch = 0f;
    [SerializeField] private Transform _camHolder;
    [SerializeField] private float _mouseSensitivity = 1f;
    #endregion


    [SerializeField] private Flashlight _flash;


    #region Stamina
    private float _stamina = 100f;
    [SerializeField] private float _staminaDrainRate = 0.5f;
    [SerializeField] private float _staminRechargeRate = 0.5f;
    [SerializeField] private float _staminaRechargeDelay = 1.5f; // how many seconds of not running it takes for stamina to recharge
    private float _lastRunTime; // last point in time where player ran.
    #endregion


    // TEMP
    [SerializeField] private Text _staminaText;
    private void SetStaminaText() { _staminaText.text = $"Stamina: {_stamina:0..}"; }

    private void Start()
    {
        _body = GetComponent<Rigidbody>();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        _lastRunTime = Time.time;

        SetStaminaText();
    }

    private void Update() // look
    {
        if (Input.GetMouseButtonDown(0))
        {
            _flash.Toggle();
        }
    
        float mouseX = Input.GetAxis("Mouse X") * _mouseSensitivity; // horizontal look, rotate entire body along the y axis
        float mouseY = Input.GetAxis("Mouse Y") * _mouseSensitivity; // vertical look, rotate only the cameraHolder along the x axis

        transform.Rotate(Vector3.up * mouseX);

        _camPitch = Mathf.Clamp(_camPitch - mouseY, -90f, 90f);
        _camHolder.localEulerAngles = Vector3.right * _camPitch; 
    }

    private void FixedUpdate() // move
    {
        float speed;

        if (Input.GetKey(KeyCode.LeftShift) && _stamina > 0f)
        {
            _stamina -= _staminaDrainRate;
            speed = _runSpeed;

            _lastRunTime = Time.time;
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

        Vector3 moveVec = 
            (transform.forward * Input.GetAxisRaw("Vertical") + transform.right * Input.GetAxisRaw("Horizontal"))
            .normalized * Time.fixedDeltaTime * speed;
        
        _body.MovePosition(_body.position + moveVec);
    }
}
