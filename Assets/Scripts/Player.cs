using UnityEngine;

[RequireComponent(typeof(PlayerController))]
public class Player : MonoBehaviour
{
    [SerializeField] private Flashlight _flash;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            _flash.Toggle();
        }
    }
}
