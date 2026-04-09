using UnityEngine;
using UnityEngine.InputSystem;

public class ToggleFlashlight : MonoBehaviour
{
    public Light flashlight;

    void Update()
    {
        if (Keyboard.current.fKey.wasPressedThisFrame)
        {
            if (flashlight != null)
            {
                flashlight.enabled = !flashlight.enabled;
                Debug.Log("Flashlight: " + flashlight.enabled);
            }
        }
    }
}