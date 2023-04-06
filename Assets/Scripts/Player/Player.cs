using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[RequireComponent(typeof(PlayerMovement))]
[RequireComponent(typeof(PlayerLook))]
public class Player : MonoBehaviour
{
    private static Player _instance;
    public static Player Instance { get { return _instance; } }

    [SerializeField] private Flashlight _flash;

    private PlayerMovement _move;
    private PlayerLook _look;

    private Coroutine _bindRoutine;

    private int _health = 100;

    // TEMP SHIT
    [SerializeField] private Text _healthText;
    void SetHealthText() => _healthText.text = $"Health: {_health}";

    private void Awake()
    {
        _instance = this;
    }

    private void Start()
    {
        _move = GetComponent<PlayerMovement>();
        _look = GetComponent<PlayerLook>();
    }

    private void Update()
    {
        HandleInput();
    }

    // Hanlde inputs regarding item and stuff
    private void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            _flash.Toggle();
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            _flash.ChargeBattery(20f);
        }
    }

    public void TakeDamage(int damage)
    {
        _health -= damage;
        if (_health <= 0)
        {
            // FUCKING DIE
        }
        SetHealthText();
    }

    public void Bleed(int ammount, int count, float interval = 0.1f) => StartCoroutine(BleedRoutine(ammount, count, interval));
    private IEnumerator BleedRoutine(int ammount, int count, float interval)
    {
        while (count-- > 0)
        {
            yield return new WaitForSeconds(interval);
            
            TakeDamage(ammount);
        }
    }

    public void Bind(float duration, Vector3? constrainPos = null, float smoothMoveTime = 0f)
    {
        if (_bindRoutine != null) StopCoroutine(_bindRoutine);
        _bindRoutine = StartCoroutine(BindRoutine(duration, constrainPos, smoothMoveTime));
    }

    private IEnumerator BindRoutine(float duration, Vector3? constrainPos = null, float smoothMoveTime = 0f)
    {
        _move.enabled = false;

        if (constrainPos != null)
        {
            Vector3 startPos = transform.position;
            Vector3 destPos = (Vector3)constrainPos;

            smoothMoveTime = Mathf.Min(duration, smoothMoveTime);
            float elapsed = 0f;

            while (elapsed < smoothMoveTime)
            {
                elapsed += Time.deltaTime;
                transform.position = Vector3.Lerp(startPos, destPos, (elapsed / smoothMoveTime));

                yield return null;
            }

            duration -= smoothMoveTime;
            transform.position = destPos;
        }
        
        yield return new WaitForSeconds(duration);

        _move.enabled = true;
    }
}
