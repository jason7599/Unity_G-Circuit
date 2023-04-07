using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[RequireComponent(typeof(PlayerMovement))]
[RequireComponent(typeof(PlayerLook))]
public class Player : MonoBehaviour
{
    #region Singleton Fields

    private static Player _instance;
    public static Player Instance 
    {
        get
        {
            if (_instance == null)
            {
                // This will only happen if another script tries to access
                // the instance before this script's awake is called
                _instance = FindObjectOfType<Player>();
                _instance._move = _instance.GetComponent<PlayerMovement>();
                _instance._look = _instance.GetComponent<PlayerLook>();
            }

            return _instance;
        }
    }

    private PlayerMovement _move;
    private PlayerLook _look;

    public static PlayerMovement Movement { get { return Instance._move; } }
    public static PlayerLook Look { get { return Instance._look; } }

    #endregion


    [SerializeField] private Flashlight _flash;


    private int _health = 100;

    // TEMP SHIT
    [SerializeField] private Text _healthText;
    void SetHealthText() => _healthText.text = $"Health: {_health}";

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;

            _move = GetComponent<PlayerMovement>();
            _look = GetComponent<PlayerLook>();
        }
        else if (_instance != this)
        {
            Debug.LogError("Multiple player instances detected.");
            Destroy(gameObject);
            return;
        }
    }

    private void Update()
    {
        HandleInput();
    }

    // Hanlde inputs regarding items and stuff
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
        // if (Input.GetKeyDown(KeyCode.Q))
        // {
        //     Stun(5f);
        // }
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

    // TODO:
    // public void TakeDamage(Damage damage)
    // {

    // }

    public void Stun(float duration)
    {
        StartCoroutine(StunRoutine(duration));
    }

    private IEnumerator StunRoutine(float duration)
    {
        Vector3 pos = transform.position;
        Quaternion rot = transform.rotation;
        RigidbodyConstraints constraints = _move.body.constraints;

        _move.body.constraints = RigidbodyConstraints.None;
        _move.body.AddForce(Random.onUnitSphere * 100f);
        _look.enabled = false;

        _move.Bind(duration);
        yield return new WaitForSeconds(duration);

        _look.enabled = true;
        _move.body.constraints = constraints;
        transform.rotation = rot;
        transform.position = pos;
    }
}
