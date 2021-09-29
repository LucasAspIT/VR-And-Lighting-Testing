using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class HandPresence : MonoBehaviour
{
    private InputDevice targetDevice;

    // Start is called before the first frame update
    void Start()
    {
        Invoke("DeviceStartup", 1);
    }

    private void Update()
    {
        if (targetDevice.TryGetFeatureValue(CommonUsages.primaryButton, out bool primaryButtonValue) && primaryButtonValue)
        {
            Debug.Log("Pressing primary button.");
        }

        if (targetDevice.TryGetFeatureValue(CommonUsages.trigger, out float triggerValue) && triggerValue > 0.1f)
        {
            Debug.Log("Trigger pressed " + triggerValue);
        }

        if (targetDevice.TryGetFeatureValue(CommonUsages.primary2DAxis, out Vector2 primary2DAxisValue) && primary2DAxisValue != Vector2.zero)
        {
            Debug.Log("Primary touchpad " + primary2DAxisValue);
        }
    }

    void DeviceStartup()
    {
        List<InputDevice> devices = new List<InputDevice>();
        InputDeviceCharacteristics rightControllerCharacteristics = InputDeviceCharacteristics.Right | InputDeviceCharacteristics.Controller;
        InputDevices.GetDevicesWithCharacteristics(rightControllerCharacteristics, devices);
        Debug.Log("Loaded devices: " + devices.Count);

        foreach (var item in devices)
        {
            int count = 1;

            Debug.Log(item.name + item.characteristics);
            Debug.Log($"The foreach loop finished running {count} time(s).");

            count++;
        }

        if (devices.Count > 0)
        {
            targetDevice = devices[0];
        }
        Debug.Log("The Start() method finished running.");
    }
}
