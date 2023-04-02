using UnityEngine;
using System.Collections;

using UnityEngine.UI;

public class Flashlight : MonoBehaviour
{
    // TEMP
    [SerializeField] private Text _batteryLeftText;

    [SerializeField] private Light _light;
    [SerializeField] private float _batteryDegradeRate = 0.25f;

    private float _batteryLeft = 100f;
    private float _initialLightIntensity;
    private Coroutine _batteryDegradeRoutine;
    private bool _isOn;

    // TEMP
    private void SetBatteryText()
    {
        _batteryLeftText.text = $"Battery Left: {_batteryLeft:.00}";
    }

    private void Start()
    {
        _initialLightIntensity = _light.intensity;
        _light.enabled = false;
        
        SetBatteryText();
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
            float interpolation = -Mathf.Pow(((_batteryLeft / 100) - 1), 4) + 1;
            float noise = Mathf.Pow(Random.value * (1 - interpolation), 2); // gets more noisy as battery runs out

            _light.intensity = _initialLightIntensity * interpolation + noise;

            _batteryLeft -= _batteryDegradeRate;

            SetBatteryText();

            yield return null;
        }

        _batteryLeft = 0f;

        SetBatteryText();

        ToggleOff();
    }
}
