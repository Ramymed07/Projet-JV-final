using UnityEngine;
using System.Collections.Generic;

public class DoorInteraction : MonoBehaviour
{
    [Header("Interaction Settings")]
    public float interactionRange = 3f;
    public KeyCode interactKey = KeyCode.E;
    public Camera playerCamera;

    [Header("Door Settings")]
    public float openAngle = 90f;
    public float doorSpeed = 3f;

    private Transform targetDoor;
    private bool isMoving = false;

    // Tracks each door's state independently
    private Dictionary<Transform, bool> doorIsOpen = new Dictionary<Transform, bool>();
    private Dictionary<Transform, Quaternion> doorClosedRotation = new Dictionary<Transform, Quaternion>();
    private Dictionary<Transform, Quaternion> doorOpenRotation = new Dictionary<Transform, Quaternion>();

    void Update()
    {
        if (Input.GetKeyDown(interactKey))
            TryInteract();

        if (isMoving && targetDoor != null)
            AnimateDoor();
    }

    void TryInteract()
    {
        Ray ray = playerCamera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, interactionRange))
        {
            bool isDoor = hit.collider.CompareTag("door");
            bool isReverseDoor = hit.collider.CompareTag("reverse door");

            if (!isDoor && !isReverseDoor) return;

            Transform door = hit.collider.transform;
            float angle = isReverseDoor ? -openAngle : openAngle;

            // First time interacting with this door: save its closed rotation
            if (!doorClosedRotation.ContainsKey(door))
            {
                doorClosedRotation[door] = door.rotation;
                doorOpenRotation[door] = door.rotation * Quaternion.Euler(0, angle, 0);
                doorIsOpen[door] = false;
            }

            targetDoor = door;
            doorIsOpen[door] = !doorIsOpen[door]; // toggle
            isMoving = true;
        }
    }

    void AnimateDoor()
    {
        Quaternion target = doorIsOpen[targetDoor] ? doorOpenRotation[targetDoor] : doorClosedRotation[targetDoor];
        targetDoor.rotation = Quaternion.Lerp(targetDoor.rotation, target, Time.deltaTime * doorSpeed);

        if (Quaternion.Angle(targetDoor.rotation, target) < 0.1f)
        {
            targetDoor.rotation = target;
            isMoving = false;
        }
    }
}