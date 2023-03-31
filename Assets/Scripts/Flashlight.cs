using UnityEngine;
using System.Collections;

public class Flashlight : MonoBehaviour
{
    [SerializeField] private Light _light;
    [SerializeField] private float _batteryDegradeRate = 0.25f;

    private float _batteryLeft = 100f;
    private float _initialLightIntensity;
    private Coroutine _batteryDegradeRoutine;
    private bool _isOn;

    private void Start()
    {
        _initialLightIntensity = _light.intensity;
        _light.enabled = false;
    }

    public void Toggle()
    {
        if (_isOn)
        {
            ToggleOff();
        }
        else if (_batteryLeft > 0f)
        {
            ToggleOn();
        }   
    }

    private void ToggleOn()
    {
        _light.enabled = true;
        _isOn = true;

        _batteryDegradeRoutine = StartCoroutine(BatteryDegrade());
    }

    private void ToggleOff()
    {
        _light.enabled = false;
        _isOn = false;

        StopCoroutine(_batteryDegradeRoutine);
    }

    private IEnumerator BatteryDegrade()
    {
        while (_batteryLeft > 0f)
        {
            _light.intensity = _initialLightIntensity * (_batteryLeft / 100);

            _batteryLeft -= _batteryDegradeRate;

            yield return null;
        }

        _batteryLeft = 0f;
        ToggleOff();
    }
}
