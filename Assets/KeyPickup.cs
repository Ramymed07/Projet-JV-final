using UnityEngine;

public class KeyPickup : MonoBehaviour
{
    [Header("Key Settings")]
    public string lockedDoorTag1 = "locked door A";
    public string unlockedDoorTag1 = "door";
    public string lockedDoorTag2 = "locked door B";
    public string unlockedDoorTag2 = "reverse door";

    [Header("Pickup Settings")]
    public float pickupRange = 2f;
    public KeyCode pickupKey = KeyCode.E;
    public Camera playerCamera;

    [Header("UI Settings")]
    public GameObject Slot1;

    [Header("Sound Settings")]
    public AudioClip pickupSound;

    [Range(0f, 1f)]
    public float pickupSoundVolume = 1f;

    void Update()
    {
        if (Input.GetKeyDown(pickupKey))
        {
            TryPickup();
        }
    }

    void TryPickup()
    {
        if (playerCamera == null)
        {
            Debug.LogWarning("Player Camera is not assigned in the Inspector.");
            return;
        }

        Ray ray = playerCamera.ScreenPointToRay(
            new Vector3(Screen.width / 2f, Screen.height / 2f, 0f)
        );

        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, pickupRange))
        {
            if (!hit.collider.CompareTag("key")) return;

            UnlockDoors();
            ShowSlot1();
            PlayPickupSound(hit.collider.transform.position);

            Destroy(hit.collider.gameObject);
        }
    }

    void UnlockDoors()
    {
        foreach (GameObject door in GameObject.FindGameObjectsWithTag(lockedDoorTag1))
        {
            door.tag = unlockedDoorTag1;
        }

        foreach (GameObject door in GameObject.FindGameObjectsWithTag(lockedDoorTag2))
        {
            door.tag = unlockedDoorTag2;
        }
    }

    void ShowSlot1()
    {
        if (Slot1 != null)
        {
            Slot1.SetActive(true);
        }
        else
        {
            Debug.LogWarning("Slot1 UI element is not assigned in the Inspector.");
        }
    }

    void PlayPickupSound(Vector3 soundPosition)
    {
        if (pickupSound == null)
        {
            Debug.LogWarning("Pickup Sound is not assigned in the Inspector.");
            return;
        }

        GameObject soundObject = new GameObject("Temporary Key Pickup Sound");
        soundObject.transform.position = soundPosition;

        AudioSource audioSource = soundObject.AddComponent<AudioSource>();
        audioSource.clip = pickupSound;
        audioSource.volume = pickupSoundVolume;

        // Makes the volume easier to control because the sound is not distance-based.
        audioSource.spatialBlend = 0f;

        audioSource.playOnAwake = false;
        audioSource.Play();

        Destroy(soundObject, pickupSound.length);
    }
}