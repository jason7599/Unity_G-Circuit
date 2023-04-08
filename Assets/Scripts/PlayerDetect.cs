using UnityEngine;

public class PlayerDetect : MonoBehaviour
{   
    [Header("Detection Options")]
    [SerializeField] private float _detectDistance = 4f;
    [SerializeField] private float _detectAngle = 60f; 
    [SerializeField] private int _detectInterval = 10; // Interval in frames to check detection
    [SerializeField] private LayerMask _obstructionlayer;
    private float _detectDistanceSquared;

    public delegate void DetectEvent();

    private DetectEvent OnPlayerDetectEnter;
    private DetectEvent OnPlayerDetectExit;
    public bool detected { get; private set; }

    private void Start()
    {
        _detectDistanceSquared = Mathf.Pow(_detectDistance, 2); // in hopes of basic optimization
    }

    // Register callback methods
    public void Register(DetectEvent enter, DetectEvent exit)
    {
        OnPlayerDetectEnter = enter;
        OnPlayerDetectExit = exit;
    }

    private void Update()
    {
        if (Time.frameCount % _detectInterval == 0) 
        {
            if (PlayerInSight())
            {
                if (!detected) // detect enter
                {
                    OnPlayerDetectEnter?.Invoke();
                    detected = true;
                }
            }
            else
            {
                if (detected) // detect exit
                {
                    OnPlayerDetectExit?.Invoke();
                    detected = false;
                }
            }
        }
    }

    public bool PlayerInSight()
    {
        Vector3 dirToPlayer = Player.Position - transform.position;

        if (dirToPlayer.sqrMagnitude > _detectDistanceSquared) return false;

        if (Vector3.Angle(transform.forward, dirToPlayer) > _detectAngle) return false;

        // TODO: more rays
        if (Physics.Raycast(transform.position + transform.up, dirToPlayer, _detectDistance, _obstructionlayer)) return false;

        return true;
    }
}
