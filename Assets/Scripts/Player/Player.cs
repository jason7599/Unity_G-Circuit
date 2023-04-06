using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private Flashlight _flash;

    private void Update()
    {
        HandleInput();
    }

    private void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            _flash.Toggle();
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            _flash.ChargeBattery(20f);
        }
    }
}
