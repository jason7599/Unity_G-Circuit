using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    [Header("General")]
    [SerializeField] private float _walkSpeed = 4f;
    [SerializeField] private float _runSpeed = 8f;
    [SerializeField] private float _jumpForce = 200f;

    [Header("Stamina")]
    [SerializeField] private float _staminaDrainRate = 1/8f;
    [SerializeField] private float _staminaRechargeRate = 1/2f;
    [SerializeField] private float _staminaRechargeDelay = 1.5f; // How many seconds of not running it takes for stamina to recharge
    [SerializeField] private float _jumpStaminaCost = 5f;

    [Header("TEMP SHIT")]
    public Text staminaText;

    void UpdateText() => staminaText.text = $"Stamina: {_stamina:.00}";

    private Rigidbody _body;
    private Vector3 _moveVec;
    private float _stamina = 100f;
    private float _nextStaminaRecharge; // Have to surpass this time for stamina to recharge

    private void Start()
    {
        _body = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveZ = Input.GetAxisRaw("Vertical");
        float speed;

        if (Input.GetKey(KeyCode.LeftShift) && (moveX != 0f || moveZ != 0f))
        {
            if (_stamina > 0f)
            {
                speed = _runSpeed;
                _stamina -= _staminaDrainRate * Time.deltaTime;
            }
            else
            {
                speed = _walkSpeed;
            }

            // Have to completely let go of shift (if moving) for stamina recharge
            _nextStaminaRecharge = Time.time + _staminaRechargeDelay;
        }
        else
        {
            if (_stamina < 100f && Time.time >= _nextStaminaRecharge)
            {
                _stamina += _staminaRechargeRate * Time.deltaTime;
            }

            speed = _walkSpeed;
        }

        _stamina = Mathf.Clamp(_stamina, 0f, 100f);
        _moveVec = (transform.forward * moveZ + transform.right * moveX).normalized * speed;

        // TODO: Ground checking
        if (Input.GetKeyDown(KeyCode.Space) && _stamina >= _jumpStaminaCost)
        {
            _body.AddForce(Vector3.up * _jumpForce);
            _stamina -= _jumpStaminaCost;
            _nextStaminaRecharge = Time.time + _staminaRechargeDelay;
        }

        UpdateText();
    }

    private void FixedUpdate()
    {
        _body.MovePosition(_body.position + _moveVec * Time.fixedDeltaTime);
    }
}
