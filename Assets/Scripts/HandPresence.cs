using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class HandPresence : MonoBehaviour
{
    [SerializeField] private bool showController = false;
    [SerializeField] private InputDeviceCharacteristics controllerCharacteristics;
    [SerializeField] private List<GameObject> controllerPrefabs;
    [SerializeField] private GameObject handmodelPrefab;

    private InputDevice targetDevice;
    private GameObject spawnedController;
    private GameObject spawnedHandModel;
    private Animator handAnimator;

    // Start is called before the first frame update
    void Start()
    {
        DeviceStartup();
    }

    private void Update()
    {
        /*
                // Listens to whether the specific button is pressed or not presented in a bool value (The A button in the case of Valve Index controllers)
                if (targetDevice.TryGetFeatureValue(CommonUsages.primaryButton, out bool primaryButtonValue) && primaryButtonValue)
                {
                    Debug.Log("Pressing primary button.");
                }

                // Listens to whether the trigger is pressed down, and how pressed down it is presented in a float value
                if (targetDevice.TryGetFeatureValue(CommonUsages.trigger, out float triggerValue) && triggerValue > 0.1f)
                {
                    Debug.Log("Trigger pressed " + triggerValue);
                }

                // Listens to whether the primary2DAxis (in the case of Valve Index controllers, it's the thumbstick) is moved about, presented in a Vector2 (like an XY coordinate system)
                if (targetDevice.TryGetFeatureValue(CommonUsages.primary2DAxis, out Vector2 primary2DAxisValue) && primary2DAxisValue != Vector2.zero)
                {
                    Debug.Log("Primary touchpad " + primary2DAxisValue);
                }
        */

        if (!targetDevice.isValid)
        {
            DeviceStartup();
        }
        else
        {
            if (showController)
            {
                spawnedHandModel.SetActive(false);
                spawnedController.SetActive(true);
            }
            else
            {
                spawnedHandModel.SetActive(true);
                spawnedController.SetActive(false);
                UpdateHandAnimation();
            }
        }
    }

    void DeviceStartup()
    {
        // Creates a list InputDevice to contain specified devices later.
        List<InputDevice> devices = new List<InputDevice>();

        // Creates an input device characteristic called 'rightControllerCharacteristics' with the specified characteristics: Assosicated with the right side of the user, is a game controller.
        // InputDeviceCharacteristics rightControllerCharacteristics = InputDeviceCharacteristics.Right | InputDeviceCharacteristics.Controller;

        // Gets the list of active XR input devices that match the specified InputDeviceCharacteristics and inserts them into the InputDevice list ('devices')
        InputDevices.GetDevicesWithCharacteristics(controllerCharacteristics, devices);
        Debug.Log("Loaded devices: " + devices.Count);

        // List out everything in the 'devices' list to the console, with the specified information.
        foreach (var item in devices)
        {
            int count = 1;

            Debug.Log("Name: " + item.name + "\n" + "Characteristics: " + item.characteristics);
            Debug.Log($"The foreach loop finished running {count} time(s).");

            count++;
        }

        if (devices.Count > 0)
        {
            targetDevice = devices[0];

            // Find controller where the name is the same as the targetDevice name (make sure the game object 3D model(s) in Unity are named correctly)
            GameObject prefab = controllerPrefabs.Find(controller => controller.name == targetDevice.name + " " + targetDevice.role);
            if (prefab)
            {
                spawnedController = Instantiate(prefab, transform);
            }
            else // If the controller prefab can't be found
            {
                Debug.Log("Error: Did not find the corresponding controller model.\nSetting model to default.");
                spawnedController = Instantiate(controllerPrefabs[0], transform);
            }

            spawnedHandModel = Instantiate(handmodelPrefab, transform);
            handAnimator = spawnedHandModel.GetComponent<Animator>();
        }
        Debug.Log("The DeviceStartup() method finished running.");
    }

    void UpdateHandAnimation()
    {
        if (targetDevice.TryGetFeatureValue(CommonUsages.trigger, out float triggerValue))
        {
            handAnimator.SetFloat("Trigger", triggerValue);
        }
        else
        {
            handAnimator.SetFloat("Trigger", 0);
        }

        if (targetDevice.TryGetFeatureValue(CommonUsages.grip, out float gripValue))
        {
            handAnimator.SetFloat("Grip", gripValue);
        }
        else
        {
            handAnimator.SetFloat("Grip", 0);
        }
    }
}
