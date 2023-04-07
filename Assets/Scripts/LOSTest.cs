using UnityEngine;
using System.Collections;

[RequireComponent(typeof(PlayerDetect))]
public class LOSTest : MonoBehaviour
{
    [SerializeField] private float _speed = 1f;
    
    private enum EnemyState { Idle, Alert, Chase }
    private Coroutine _alertRoutine;
    private EnemyState _state = EnemyState.Idle;
    private Vector3 _lastSeenPos;
    
    private void Start()
    {
        GetComponent<PlayerDetect>().Configure(OnPlayerDetect, OnPlayerLost);
    }

    private void Update()
    {
        switch (_state)
        {
            case EnemyState.Idle:

                break;

            case EnemyState.Alert:



                break;

            case EnemyState.Chase:

                Vector3 dirToMove = Player.Instance.transform.position - transform.position;

                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(dirToMove), .5f);
                transform.position += dirToMove.normalized * Time.deltaTime * _speed;

                break;
        }
    }

    private void OnPlayerDetect()
    {
        if (_alertRoutine != null)
            StopCoroutine(_alertRoutine);

        _state = EnemyState.Chase;
        _lastSeenPos = Player.Instance.transform.position;
    }

    private void OnPlayerLost()
    {
        _state = EnemyState.Alert;
    }

    private IEnumerator AlertRoutine(float duration)
    {
        while (duration > 0f)
        {
            Quaternion startRot = transform.rotation;
            Quaternion destRot = Quaternion.LookRotation(new Vector3(Random.Range(-1f, 1f), 0f, Random.Range(-1f, 1f)));

            float randomDuration = Mathf.Min(Random.Range(.25f, .5f), duration);
            float elapsed = 0f;

            while (elapsed < randomDuration)
            {
                transform.rotation = Quaternion.Slerp(startRot, destRot, (elapsed / randomDuration));
                elapsed += Time.deltaTime;

                yield return null;
            }

            duration -= randomDuration;
        }

        _state = EnemyState.Idle;
    }
}
