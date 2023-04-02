using UnityEngine;
using UnityEditor;
using System.Collections;

public class LOSTest : MonoBehaviour
{
    [SerializeField] private Transform _playerTr;

    [SerializeField] private float _detectAngle = 50f;
    [SerializeField] private float _detectDistance = 16f;
    [SerializeField] private int _visionCheckInterval = 10;

    private int _obstacleLayer = (1 << (int)Layer.Wall);

    private void Update()
    {
        if (Time.frameCount % _visionCheckInterval == 0)
        {
            if (PlayerInSight())
            {
                transform.LookAt(_playerTr);
            }
        }
    }

    private bool PlayerInSight()
    {
        Vector3 dirToPlayer = _playerTr.position - transform.position;

        if (dirToPlayer.sqrMagnitude > Mathf.Pow(_detectDistance, 2)) return false;

        if (Vector3.Angle(transform.forward, dirToPlayer) > _detectAngle) return false;

        if (Physics.Raycast(transform.position + Vector3.up * 1.5f, dirToPlayer, _detectDistance, _obstacleLayer)) return false;

        return true;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _detectDistance);

        if (PlayerInSight())
        {
            Vector3 dirToPlayer = _playerTr.position - transform.position;

            Gizmos.color = Color.green;
            Gizmos.DrawRay(transform.position + Vector3.up * 1.5f, dirToPlayer);
        }
    }
}
