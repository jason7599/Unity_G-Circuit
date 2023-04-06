using UnityEngine;

public class GhostTest : MonoBehaviour
{
    private PlayerController _player;
    
    [SerializeField] private float _speed = 8f;
    [SerializeField] private float _attackRangeSqrd = 4f;

    private int _health = 100;

    private void Start()
    {
        _player = FindObjectOfType<PlayerController>();
    }
    
    private void Update()
    {
        Vector3 dirToPlayer = _player.transform.position - transform.position;
        transform.rotation = Quaternion.LookRotation(dirToPlayer);

        if (dirToPlayer.sqrMagnitude <= _attackRangeSqrd)
        {
            _player.Die();
            _player.Body.AddForce(dirToPlayer * 100f);
            enabled = false;
        }

        transform.position += dirToPlayer.normalized * Time.deltaTime * _speed;
    }

    public void Hurt()
    {
        print("Ow");
        if (--_health <= 0f)
        {
            Flee();
        }
    }

    private void Flee()
    {
        _health = 100;
        _speed += 2f;
        
        Vector3 randomPos = Random.onUnitSphere;
        randomPos.y = 0;
        randomPos = randomPos.normalized * 50f;

        transform.position = randomPos;
    }
}
