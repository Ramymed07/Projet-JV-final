using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// A simple, modular combination-lock puzzle.
/// Attach to a UI Canvas/Panel (disabled by default).
/// Wire up the buttons and display in the Inspector.
///
/// HOW TO EXTEND:
///   • Change <see cref="secretCode"/> in the Inspector for each interactable.
///   • Swap out the digit logic in <see cref="AppendDigit"/> for any other
///     puzzle type (pattern, colour sequence, slider, etc.).
///   • Call <see cref="SolvePuzzle"/> from any external script to trigger completion.
/// </summary>
public class PuzzleManager : MonoBehaviour
{
    [Header("Puzzle Configuration")]
    [Tooltip("The correct code the player must enter to solve the puzzle.")]
    public string secretCode = "1234";

    [Header("UI References")]
    [Tooltip("Text element that shows the player's current input.")]
    public TMP_Text inputDisplay;

    [Tooltip("(Optional) Text shown when the player enters a wrong code.")]
    public TMP_Text feedbackText;

    [Tooltip("Button that clears the current input.")]
    public Button clearButton;

    [Tooltip("Button that submits the current input.")]
    public Button submitButton;

    [Tooltip("Digit buttons 0-9. Order in the list determines which digit each fires (index 0 → '0', index 1 → '1', etc.).")]
    public Button[] digitButtons;

    [Header("Feedback Messages")]
    public string wrongCodeMessage = "Incorrect. Try again.";
    public string correctCodeMessage = "Unlocked!";

    // ── Runtime state ──────────────────────────────────────────────────────────
    private string _currentInput = "";
    private InteractableObject _caller;    // the object that opened this puzzle

    // ── Unity lifecycle ────────────────────────────────────────────────────────

    private void Awake()
    {
        // Wire digit buttons automatically
        for (int i = 0; i < digitButtons.Length; i++)
        {
            int digit = i; // captured for the closure
            digitButtons[i].onClick.AddListener(() => AppendDigit(digit.ToString()));
        }

        if (clearButton  != null) clearButton.onClick.AddListener(ClearInput);
        if (submitButton != null) submitButton.onClick.AddListener(TrySubmit);

        gameObject.SetActive(false); // start hidden
    }

    // ── Public API ─────────────────────────────────────────────────────────────

    /// <summary>Opens the puzzle panel and stores a reference to the caller.</summary>
    public void OpenPuzzle(InteractableObject caller)
    {
        _caller = caller;
        _currentInput = "";
        UpdateDisplay();
        SetFeedback("");
        gameObject.SetActive(true);

        // Optional: pause the game while puzzle is open
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    /// <summary>Closes the puzzle panel without solving it.</summary>
    public void ClosePuzzle()
    {
        gameObject.SetActive(false);
        ResumeCursor();
    }

    /// <summary>
    /// Call this to force-complete the puzzle from an external script
    /// (e.g. a custom puzzle mini-game that handles its own win condition).
    /// </summary>
    public void SolvePuzzle()
    {
        SetFeedback(correctCodeMessage);
        gameObject.SetActive(false);
        ResumeCursor();
        _caller?.OnPuzzleSolved();
    }

    // ── Input handling ─────────────────────────────────────────────────────────

    /// <summary>Appends a character to the player's current input and refreshes the display.</summary>
    public void AppendDigit(string digit)
    {
        // Limit input length to prevent infinite typing
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

    // ── Helpers ────────────────────────────────────────────────────────────────

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

    // Allow closing with Escape (works even with timeScale = 0 via unscaled update)
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            ClosePuzzle();
    }
}