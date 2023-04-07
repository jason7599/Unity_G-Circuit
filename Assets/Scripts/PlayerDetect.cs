using UnityEngine;

public class PlayerDetect : MonoBehaviour
{   
    [Header("Detection Options")]
    [SerializeField] private float _detectDistance = 4f;
    [SerializeField] private float _detectAngle = 60f;
    [SerializeField] private int _detectInterval = 10;
    [SerializeField] private LayerMask _obstructionlayer;
    private float _detectDistanceSquared;

    public delegate void DetectEvent();

    private DetectEvent OnPlayerDetectEnter;
    private DetectEvent OnPlayerDetectExit;
    public bool detected { get; private set; }

    private void Start()
    {
        _detectDistanceSquared = Mathf.Pow(_detectDistance, 2);
    }

    public void Configure(DetectEvent enter, DetectEvent exit)
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
                if (!detected)
                {
                    detected = true;
                    OnPlayerDetectEnter?.Invoke();
                }
            }
            else
            {
                if (detected)
                {
                    detected = false;
                    OnPlayerDetectExit?.Invoke();
                }
            }
        }
    }

    public bool PlayerInSight()
    {
        Vector3 dirToPlayer = Player.Instance.transform.position - transform.position;

        if (dirToPlayer.sqrMagnitude > _detectDistanceSquared) return false;

        if (Vector3.Angle(transform.forward, dirToPlayer) > _detectAngle) return false;

        if (Physics.Raycast(transform.position + transform.up, dirToPlayer, _detectDistance, _obstructionlayer)) return false;

        return true;
    }
}
