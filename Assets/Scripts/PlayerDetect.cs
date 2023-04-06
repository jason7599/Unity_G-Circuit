using UnityEngine;

public abstract class PlayerDetect : MonoBehaviour
{
    private Collider _collider;

    protected virtual void Awake()
    {
        _collider = GetComponent<Collider>();
    }

    private void OnTriggerEnter(Collider collider)
    {
        _collider.enabled = false;

        OnDetect();
    }

    protected abstract void OnDetect();
}
