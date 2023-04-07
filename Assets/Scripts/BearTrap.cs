using UnityEngine;
using System.Collections;

public class BearTrap : PlayerDetect
{
    [SerializeField] private float _trapDuration = 2.5f;
    [SerializeField] private int _trapDamage = 5;

    private Animator _anim;
    private readonly int _disposeTriggerHash = Animator.StringToHash("dispose");

    private void Start()
    {
        _anim = GetComponent<Animator>();
    }

    protected override void OnDetect()
    {
        Player.Instance.TakeDamage(_trapDamage);
        Player.Instance.Bleed(1, 10, 2f);
        Player.Movement.Bind(_trapDuration, transform.position, 0.075f);

        StartCoroutine(ActivateRoutine());
    }

    private IEnumerator ActivateRoutine()
    {
        _anim.enabled = true;

        yield return new WaitForSeconds(_trapDuration);

        _anim.SetTrigger(_disposeTriggerHash);

        yield return new WaitForSeconds(1.5f); // time for animation to complete

        Destroy(gameObject);
    }
}
