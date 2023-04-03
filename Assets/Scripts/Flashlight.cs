using UnityEngine;
using System.Collections;

using UnityEngine.UI;

public class Flashlight : MonoBehaviour
{
    // TEMP
    [SerializeField] private Text _batteryLeftText;

    [SerializeField] private Light _light;
    [SerializeField] private float _batteryDrainRate = 0.25f;

    private float _batteryLeft = 100f;
    private float _initialLightIntensity;
    private Coroutine _batteryDrainRoutine;
    private bool _isOn;

    // TEMP
    private void SetBatteryText()
    {
        _batteryLeftText.text = $"Battery Left: {_batteryLeft:.00}";
    }

    // TEMP
    public void HAX()
    {
        _batteryLeft = 100f;
        SetBatteryText();
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

        _batteryDrainRoutine = StartCoroutine(BatteryDegrade());
    }
    

    private int _beamCollideLayer = (1 << (int)Layer.Wall) | (1 << (int)Layer.Enemy);

    private void Update() // TODO: maybe move this into battery degrade routine?
    {
        if (!_isOn) return;

        // Debug.DrawRay()

        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, 10f, _beamCollideLayer, QueryTriggerInteraction.Collide))
        {
            GameObject hitObject = hit.collider.gameObject;

            if (hitObject.layer == (int)Layer.Wall)
            {
                return;
            }
            
            if (hitObject.TryGetComponent(out GhostTest gt))
            {
                gt.Hurt();
            }
        }
    }


    private void ToggleOff()
    {
        _light.enabled = false;
        _isOn = false;

        StopCoroutine(_batteryDrainRoutine);
    }

    private IEnumerator BatteryDegrade()
    {
        while (_batteryLeft > 0f)
        {
            float interpolation = -Mathf.Pow(((_batteryLeft / 100) - 1), 4) + 1;
            float noise = Mathf.Pow(Random.value * (1 - interpolation), 2); // gets more noisy as battery runs out

            _light.intensity = _initialLightIntensity * interpolation + noise;

            _batteryLeft -= _batteryDrainRate;

            SetBatteryText();

            yield return null;
        }

        _batteryLeft = 0f;

        SetBatteryText();

        ToggleOff();
    }
}
