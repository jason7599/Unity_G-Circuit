using UnityEngine;
using System.Collections;

[RequireComponent(typeof(PlayerDetect))]
public class LOSTest : MonoBehaviour
{
    [SerializeField] private float _speed = 1f;
    
    private enum EnemyState { Idle, Alert, Chase }
    private EnemyState _state = EnemyState.Idle;
    private Vector3 _destPos;

    private Coroutine _alertRoutine;
    
    private void Start()
    {
        GetComponent<PlayerDetect>().Register(OnPlayerDetect, OnPlayerLost);
    }

    private void Update()
    {
        switch (_state)
        {
            // move to random pos
            // case EnemyState.Idle:
            //     break;

            // move to player's last seen pos
            // and look around randomly
            case EnemyState.Alert:

                break;

            // move to player
            case EnemyState.Chase:
                _destPos = Player.Position;
                break;
        }
    }


    // transition to Chase
    private void OnPlayerDetect()
    {
        if (_alertRoutine != null)
            StopCoroutine(_alertRoutine);

        _state = EnemyState.Chase;
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
