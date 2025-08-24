using UnityEngine;

public class FlashLightToggle : MonoBehaviour
{
    private UnityEngine.Rendering.Universal.Light2D light2D;
    private bool lightOn = true;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        light2D = GetComponent<UnityEngine.Rendering.Universal.Light2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            SwitchFlashLight();
        }
    }

    void SwitchFlashLight()
    {
        if (!lightOn)
        {
            light2D.intensity = 0.4f;
        }
        else
        {
            light2D.intensity = 0f;
        }
        lightOn = !lightOn;
    }
}
