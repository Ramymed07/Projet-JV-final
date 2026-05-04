using UnityEngine;
using UnityEngine.SceneManagement;
using StarterAssets;

public class ReadablePaper : MonoBehaviour
{
    [Header("Main UI")]
    public GameObject mainUI;

    [Header("Paper UI")]
    public GameObject readPanel;
    public GameObject interactPrompt;

    [Header("Player Controller")]
    public FirstPersonController firstPersonController;

    [Header("Ending Settings")]
    public bool isFinalPaper = false;
    public GameObject endingScreen;

    private bool playerInRange = false;
    private bool isReading = false;

    private static ReadablePaper activePaper;

    void Start()
    {
        if (readPanel != null)
            readPanel.SetActive(false);

        if (interactPrompt != null)
            interactPrompt.SetActive(false);

        if (endingScreen != null)
            endingScreen.SetActive(false);
    }

    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E) && !isReading && activePaper == null)
        {
            OpenPaper();
        }

        if (activePaper == this && isReading && Input.GetKeyDown(KeyCode.Escape))
        {
            ClosePaper();
        }
    }

    void OpenPaper()
    {
        activePaper = this;
        isReading = true;

        if (readPanel != null)
        {
            readPanel.SetActive(true);
            readPanel.transform.SetAsLastSibling();
        }

        if (interactPrompt != null)
            interactPrompt.SetActive(false);

        if (mainUI != null)
            mainUI.SetActive(false);

        if (firstPersonController != null)
            firstPersonController.enabled = false;

        UnlockCursor();
    }

    void ClosePaper()
    {
        isReading = false;

        if (readPanel != null)
            readPanel.SetActive(false);

        activePaper = null;

        if (isFinalPaper)
        {
            ShowEndingScreen();
            return;
        }

        if (mainUI != null)
            mainUI.SetActive(true);

        if (firstPersonController != null)
            firstPersonController.enabled = true;

        if (playerInRange && interactPrompt != null)
            interactPrompt.SetActive(true);

        LockCursor();
    }

    void ShowEndingScreen()
    {
        if (endingScreen != null)
        {
            endingScreen.SetActive(true);
            endingScreen.transform.SetAsLastSibling();
        }

        if (mainUI != null)
            mainUI.SetActive(false);

        if (interactPrompt != null)
            interactPrompt.SetActive(false);

        if (firstPersonController != null)
            firstPersonController.enabled = false;

        UnlockCursor();

        Time.timeScale = 0f;
    }

    private void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void UnlockCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void ExitGame()
    {
        Time.timeScale = 1f;
        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Something entered trigger: " + other.name);

        if (other.CompareTag("Player"))
        {
            Debug.Log("Player entered paper trigger");
            playerInRange = true;

            if (interactPrompt != null && !isReading && activePaper == null)
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

            if (isReading && activePaper == this)
                ClosePaper();
        }
    }
}