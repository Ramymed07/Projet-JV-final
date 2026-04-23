using System.Collections.Generic;
using UnityEngine;

public class PlayerKeyInteractor : MonoBehaviour
{
    [Header("Input")]
    public KeyCode interactKey = KeyCode.E;

    [Header("Raycast")]
    public float interactRange = 5f;
    public Camera playerCamera;

    [Header("UI Slots")]
    public GameObject[] keySlots;

    [Header("Puzzle")]
    [Tooltip("Assign the PuzzleManager so key interaction is blocked while a puzzle is open.")]
    public PuzzleManager puzzleManager;

    private Dictionary<string, string> collectedKeys = new Dictionary<string, string>();
    private int keysCollectedCount = 0;

    void Update()
    {
        // Do not process any interaction while a puzzle panel is open
        if (puzzleManager != null && puzzleManager.gameObject.activeSelf) return;

        if (Input.GetKeyDown(interactKey))
        {
            TryInteract();
        }
    }

    void TryInteract()
    {
        if (playerCamera == null)
        {
            Debug.LogWarning("Player Camera is not assigned on PlayerKeyInteractor.");
            return;
        }

        Ray ray = playerCamera.ScreenPointToRay(
            new Vector3(Screen.width / 2f, Screen.height / 2f, 0f)
        );
        Debug.DrawRay(ray.origin, ray.direction * interactRange, Color.red, 2f);

        RaycastHit hit;
        if (!Physics.Raycast(ray, out hit, interactRange))
        {
            Debug.Log("Raycast hit nothing.");
            return;
        }

        Debug.Log("Hit: " + hit.collider.name);

        DoorKeyData keyData = hit.collider.GetComponentInParent<DoorKeyData>();
        if (keyData != null)
        {
            Debug.Log("Found DoorKeyData on: " + keyData.name);
            PickupKey(keyData);
            return;
        }

        Debug.Log("No DoorKeyData found on hit object.");
        TryUnlockDoorSet(hit.collider.gameObject);
    }

    void PickupKey(DoorKeyData keyData)
    {
        if (keyData.doorPairs == null || keyData.doorPairs.Length == 0)
        {
            Debug.LogWarning($"Key '{keyData.name}' has no door pairs assigned.");
            return;
        }

        foreach (DoorTagPair pair in keyData.doorPairs)
        {
            if (pair == null) continue;
            if (string.IsNullOrEmpty(pair.lockedDoorTag) || string.IsNullOrEmpty(pair.unlockedDoorTag))
            {
                Debug.LogWarning($"Key '{keyData.name}' has an incomplete door pair.");
                continue;
            }
            collectedKeys[pair.lockedDoorTag] = pair.unlockedDoorTag;
        }

        ShowNextKeySlot();
        Debug.Log($"Picked up {keyData.keyId}.");
        Destroy(keyData.gameObject);
    }

    void TryUnlockDoorSet(GameObject hitObject)
    {
        string hitTag = hitObject.tag;
        if (!collectedKeys.ContainsKey(hitTag))
        {
            Debug.Log($"No matching key for {hitObject.name} with tag '{hitTag}'.");
            return;
        }

        foreach (KeyValuePair<string, string> key in collectedKeys)
        {
            GameObject[] matchingDoors = GameObject.FindGameObjectsWithTag(key.Key);
            if (matchingDoors.Length > 0)
            {
                foreach (GameObject door in matchingDoors)
                {
                    door.tag = key.Value;
                }
            }
        }

        Debug.Log($"Unlocked door set triggered by '{hitTag}'.");
    }

    void ShowNextKeySlot()
    {
        if (keySlots == null || keySlots.Length == 0)
        {
            Debug.LogWarning("No key UI slots assigned.");
            return;
        }

        if (keysCollectedCount < keySlots.Length)
        {
            if (keySlots[keysCollectedCount] != null)
                keySlots[keysCollectedCount].SetActive(true);
            keysCollectedCount++;
        }
        else
        {
            Debug.LogWarning("No more UI slots available for additional keys.");
        }
    }
}