using UnityEngine;
using TMPro;

public class PuzzleManager : MonoBehaviour
{
    public GameObject puzzlePanel;
    public TMP_Text inputCodeText;
    public string correctCode = "1234";
    public GameObject objectToDisappear;

    private string currentInput = "";
    private InteractableObject currentInteractable;

    void Start()
    {
        puzzlePanel.SetActive(false);
        LockCursor();
    }

    void Update()
    {
        if (puzzlePanel.activeSelf && Input.GetKeyDown(KeyCode.Escape))
        {
            ClosePuzzle();
        }
    }

    public void OpenPuzzle(InteractableObject interactable)
    {
        currentInteractable = interactable;

        currentInput = "";
        UpdateDisplay();

        StartCoroutine(OpenPuzzleAfterFrame());
    }

    private System.Collections.IEnumerator OpenPuzzleAfterFrame()
    {
        yield return null;

        puzzlePanel.SetActive(true);
        puzzlePanel.transform.SetAsLastSibling();

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void PressNumber(string number)
    {
        currentInput += number;
        UpdateDisplay();
    }

    public void ClearInput()
    {
        currentInput = "";
        UpdateDisplay();
    }

    public void PressEnter()
    {
        if (currentInput == correctCode)
        {
            SolvePuzzle();
        }
        else
        {
            ClearInput();
        }
    }

    public void ClosePuzzle()
    {
        puzzlePanel.SetActive(false);

        currentInput = "";
        UpdateDisplay();

        LockCursor();
    }

    private void SolvePuzzle()
    {
        if (objectToDisappear != null)
            objectToDisappear.SetActive(false);

        if (currentInteractable != null)
            currentInteractable.MarkPuzzleSolved();

        ClosePuzzle();
    }

    private void UpdateDisplay()
    {
        if (inputCodeText != null)
            inputCodeText.text = currentInput;
    }

    private void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}