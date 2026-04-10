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

        if (mainUI != null)
            mainUI.SetActive(false); // 👈 ADD THIS

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

        if (isFinalPaper)
        {
            ShowEndingScreen();
            return;
        }

        if (mainUI != null)
            mainUI.SetActive(true); // 👈 only for normal papers

        if (firstPersonController != null)
            firstPersonController.enabled = true;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        if (playerInRange && interactPrompt != null)
            interactPrompt.SetActive(true);
    }

    void ShowEndingScreen()
    {
        if (endingScreen != null)
            endingScreen.SetActive(true);

        if (firstPersonController != null)
            firstPersonController.enabled = false;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        Time.timeScale = 0f;
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
        if (other.CompareTag("Player"))
        {
            playerInRange = true;

            if (interactPrompt != null && !isReading)
                interactPrompt.SetActive(true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;

            if (interactPrompt != null)
                interactPrompt.SetActive(false);

            if (isReading)
                ClosePaper();
        }
    }
}