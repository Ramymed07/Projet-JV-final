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

    void Update()
    {
        if (Input.GetKeyDown(pickupKey))
            TryPickup();
    }

    void TryPickup()
    {
        Ray ray = playerCamera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, pickupRange))
        {
            if (!hit.collider.CompareTag("key")) return;

            UnlockDoors();
            Destroy(hit.collider.gameObject);
        }
    }

    void UnlockDoors()
    {
        foreach (GameObject door in GameObject.FindGameObjectsWithTag(lockedDoorTag1))
            door.tag = unlockedDoorTag1;

        foreach (GameObject door in GameObject.FindGameObjectsWithTag(lockedDoorTag2))
            door.tag = unlockedDoorTag2;
    }
}