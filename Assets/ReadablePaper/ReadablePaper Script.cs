using UnityEngine;
using StarterAssets;

public class ReadablePaper : MonoBehaviour
{
    public GameObject readPanel;
    public GameObject interactPrompt;
    public FirstPersonController firstPersonController;

    private bool playerInRange = false;
    private bool isReading = false;

    void Start()
    {
        if (readPanel != null)
            readPanel.SetActive(false);

        if (interactPrompt != null)
            interactPrompt.SetActive(false);
    }

    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E) && !isReading)
        {
            OpenPaper();
        }

        if (isReading && Input.GetKeyDown(KeyCode.Escape))
        {
            ClosePaper();
        }
    }

    void OpenPaper()
    {
        isReading = true;

        if (readPanel != null)
            readPanel.SetActive(true);

        if (interactPrompt != null)
            interactPrompt.SetActive(false);

        if (firstPersonController != null)
            firstPersonController.enabled = false;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    void ClosePaper()
    {
        isReading = false;

        if (readPanel != null)
            readPanel.SetActive(false);

        if (firstPersonController != null)
            firstPersonController.enabled = true;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        if (playerInRange && interactPrompt != null)
            interactPrompt.SetActive(true);
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Something entered trigger: " + other.name);

        if (other.CompareTag("Player"))
        {
            Debug.Log("Player entered paper trigger");
            playerInRange = true;

            if (interactPrompt != null)
                interactPrompt.SetActive(true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        Debug.Log("Something exited trigger: " + other.name);

        if (other.CompareTag("Player"))
        {
            Debug.Log("Player exited paper trigger");
            playerInRange = false;

            if (interactPrompt != null)
                interactPrompt.SetActive(false);

            if (isReading)
                ClosePaper();
        }
    }
}
    
