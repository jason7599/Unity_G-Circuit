using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(PlayerDetect))]
public class Hunter : MonoBehaviour
{
    private NavMeshAgent _agent;

    private bool _inSight = false;
    private Vector3 _destPos;

    private void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        _agent.updateRotation = false;
        GetComponent<PlayerDetect>().Register(OnPlayerDetect, OnPlayerLost);
        _destPos = transform.position;
    }

    private void Update()
    {
        if (_inSight)
        {
            transform.LookAt(Player.Transform);
            _destPos = Player.Position;
            _agent.stoppingDistance = 5f;
        }
        else
        {
            _agent.stoppingDistance = 0f;
        }

        _agent.SetDestination(_destPos);
    }

    private void OnPlayerDetect()
    {
        print("Detect!");
        _inSight = true;
    }

    private void OnPlayerLost()
    {
        print("Lost");
        _inSight = false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(_destPos, 0.5f);
    }
}
