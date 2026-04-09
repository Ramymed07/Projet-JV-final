using UnityEngine;

public class InventoryToggle : MonoBehaviour
{
    [Header("UI Settings")]
    public GameObject inventoryImage;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
            ToggleInventory();
    }

    void ToggleInventory()
    {
        if (inventoryImage != null)
            inventoryImage.SetActive(!inventoryImage.activeSelf);
        else
            Debug.LogWarning("Inventory Image UI element is not assigned in the Inspector.");
    }
}