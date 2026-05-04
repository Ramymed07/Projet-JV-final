using UnityEngine;

public class HintMenuCursor : MonoBehaviour
{
    [Tooltip("If true, the cursor will be shown and unlocked while this panel is active.")]
    public bool showCursorWhenActive = true;

    [Tooltip("If true, the cursor will restore its previous lock/visibility state when the panel closes.")]
    public bool restoreCursorOnClose = true;

    private bool previousCursorVisible;
    private CursorLockMode previousCursorLockState;
    private bool hasSavedCursorState;

    void OnEnable()
    {
        if (!showCursorWhenActive)
            return;

        SaveCursorState();
        SetCursorVisible(true);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            gameObject.SetActive(false);
        }
    }

    void OnDisable()
    {
        if (!restoreCursorOnClose || !hasSavedCursorState)
            return;

        RestoreCursorState();
    }

    private void SaveCursorState()
    {
        previousCursorVisible = Cursor.visible;
        previousCursorLockState = Cursor.lockState;
        hasSavedCursorState = true;
    }

    private void RestoreCursorState()
    {
        Cursor.visible = previousCursorVisible;
        Cursor.lockState = previousCursorLockState;
        hasSavedCursorState = false;
    }

    private void SetCursorVisible(bool visible)
    {
        Cursor.visible = visible;
        Cursor.lockState = visible ? CursorLockMode.None : CursorLockMode.Locked;
    }
}
