using UnityEngine;

public class HintMenuCursor : MonoBehaviour
{
    [Tooltip("If true, the cursor will be shown and unlocked while this panel is active.")]
    public bool showCursorWhenActive = true;

    [Tooltip("If true, the cursor will restore its previous lock/visibility state when the panel closes.")]
    public bool restoreCursorOnClose = true;

    [Tooltip("The UI element in the top-left that should be hidden when the hint menu opens.")]
    public GameObject topLeftUI;

    [Tooltip("The UI element in the top-right that should be hidden when the hint menu opens.")]
    public GameObject topRightUI;

    private bool previousCursorVisible;
    private CursorLockMode previousCursorLockState;
    private bool hasSavedCursorState;

    private GameObject[] previousTopLeftObjects;
    private bool[] previousTopLeftStates;
    private GameObject[] previousTopRightObjects;
    private bool[] previousTopRightStates;
    private bool hasSavedTopUIState;

    void OnEnable()
    {
        if (showCursorWhenActive)
        {
            SaveCursorState();
            SetCursorVisible(true);
        }

        SaveTopUIState();
        HideTopUI();
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
        if (restoreCursorOnClose && hasSavedCursorState)
        {
            RestoreCursorState();
        }

        if (hasSavedTopUIState)
        {
            RestoreTopUIState();
        }
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

    private void SaveTopUIState()
    {
        if (topLeftUI != null)
            SaveHierarchyState(topLeftUI, out previousTopLeftObjects, out previousTopLeftStates);

        if (topRightUI != null)
            SaveHierarchyState(topRightUI, out previousTopRightObjects, out previousTopRightStates);

        hasSavedTopUIState = true;
    }

    private void HideTopUI()
    {
        if (topLeftUI != null)
            topLeftUI.SetActive(false);

        if (topRightUI != null)
            topRightUI.SetActive(false);
    }

    private void RestoreTopUIState()
    {
        if (previousTopLeftObjects != null && previousTopLeftStates != null)
            RestoreHierarchyState(previousTopLeftObjects, previousTopLeftStates);

        if (previousTopRightObjects != null && previousTopRightStates != null)
            RestoreHierarchyState(previousTopRightObjects, previousTopRightStates);

        hasSavedTopUIState = false;
    }

    private void SaveHierarchyState(GameObject root, out GameObject[] objects, out bool[] states)
    {
        var allObjects = root.GetComponentsInChildren<Transform>(true);
        objects = new GameObject[allObjects.Length];
        states = new bool[allObjects.Length];

        for (int i = 0; i < allObjects.Length; i++)
        {
            objects[i] = allObjects[i].gameObject;
            states[i] = allObjects[i].gameObject.activeSelf;
        }
    }

    private void RestoreHierarchyState(GameObject[] objects, bool[] states)
    {
        int count = Mathf.Min(objects.Length, states.Length);
        for (int i = 0; i < count; i++)
        {
            if (objects[i] != null)
                objects[i].SetActive(states[i]);
        }
    }
}
