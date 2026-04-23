using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PuzzleManager : MonoBehaviour
{
    [Header("Puzzle Configuration")]
    public string secretCode = "1234";

    [Header("UI References")]
    public TMP_Text inputDisplay;
    public TMP_Text feedbackText;
    public Button clearButton;
    public Button submitButton;
    public Button[] digitButtons;

    [Header("Feedback Messages")]
    public string wrongCodeMessage = "Incorrect. Try again.";
    public string correctCodeMessage = "Unlocked!";

    private string _currentInput = "";
    private InteractableObject _caller;
    private int _openedOnFrame = -1;

    private void Awake()
    {
        for (int i = 0; i < digitButtons.Length; i++)
        {
            int digit = i;
            digitButtons[i].onClick.AddListener(() => AppendDigit(digit.ToString()));
        }

        if (clearButton  != null) clearButton.onClick.AddListener(ClearInput);
        if (submitButton != null) submitButton.onClick.AddListener(TrySubmit);

        gameObject.SetActive(false);
    }

    public void OpenPuzzle(InteractableObject caller)
    {
        _caller = caller;
        _currentInput = "";
        UpdateDisplay();
        SetFeedback("");
        _openedOnFrame = Time.frameCount;
        gameObject.SetActive(true);

        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void ClosePuzzle()
    {
        gameObject.SetActive(false);
        ResumeCursor();
    }

    public void SolvePuzzle()
    {
        SetFeedback(correctCodeMessage);
        gameObject.SetActive(false);
        ResumeCursor();
        _caller?.OnPuzzleSolved();
    }

    public void AppendDigit(string digit)
    {
        if (_currentInput.Length >= secretCode.Length * 2) return;
        _currentInput += digit;
        UpdateDisplay();
        SetFeedback("");
    }

    private void ClearInput()
    {
        _currentInput = "";
        UpdateDisplay();
        SetFeedback("");
    }

    private void TrySubmit()
    {
        if (_currentInput == secretCode)
        {
            SolvePuzzle();
        }
        else
        {
            SetFeedback(wrongCodeMessage);
            _currentInput = "";
            UpdateDisplay();
        }
    }

    private void UpdateDisplay()
    {
        if (inputDisplay != null)
            inputDisplay.text = _currentInput;
    }

    private void SetFeedback(string message)
    {
        if (feedbackText != null)
            feedbackText.text = message;
    }

    private void ResumeCursor()
    {
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        if (Time.frameCount == _openedOnFrame) return;

        if (Input.GetKeyDown(KeyCode.Escape))
            ClosePuzzle();
    }
}