using UnityEngine;
using UnityEngine.UI;

// ─────────────────────────────────────────────────────────────────────────────
// PanelToggle.cs
//
// HOW TO SET UP:
//  1. Create a Canvas (GameObject > UI > Canvas). Set Render Mode to
//     "Screen Space - Overlay".
//  2. Inside the Canvas, create a Panel (GameObject > UI > Panel).
//     Anchor it however you like (e.g. centre, or full-screen).
//  3. Inside the Panel, add a Scroll View (GameObject > UI > Scroll View).
//     Resize it to fill the panel.
//  4. Inside the Scroll View > Viewport > Content, add a Text (Legacy) object
//     OR a TextMeshPro - Text object and paste your long text there.
//     Set the Content's height to "Auto" via a Content Size Fitter component
//     (Vertical Fit = Preferred Size) so it grows with the text.
//  5. Attach THIS script to ANY GameObject in the scene (e.g. an empty
//     "GameManager" object, or the Canvas itself).
//  6. In the Inspector, drag:
//       • your Panel  → Panel To Toggle
//       • (optional) your scrollbar → Scroll Rect  so it resets to top on open
// ─────────────────────────────────────────────────────────────────────────────

public class PanelToggle : MonoBehaviour
{
    [Header("References")]
    [Tooltip("The UI Panel (or any root GameObject) to show/hide.")]
    public GameObject panelToToggle;

    [Tooltip("(Optional) The ScrollRect inside the panel. " +
             "If assigned, the scroll position resets to the top each time the panel opens.")]
    public ScrollRect scrollRect;

    [Header("Settings")]
    [Tooltip("Key that toggles the panel.")]
    public KeyCode toggleKey = KeyCode.P;

    [Tooltip("If true the panel starts hidden; if false it starts visible.")]
    public bool hiddenOnStart = true;

    // -------------------------------------------------------------------------

    void Start()
    {
        if (panelToToggle == null)
        {
            Debug.LogWarning("[PanelToggle] 'Panel To Toggle' is not assigned in the Inspector.");
            return;
        }

        panelToToggle.SetActive(!hiddenOnStart);
    }

    void Update()
    {
        if (panelToToggle == null) return;

        if (Input.GetKeyDown(toggleKey))
        {
            bool nowActive = !panelToToggle.activeSelf;
            panelToToggle.SetActive(nowActive);

            // Reset scroll to top whenever the panel is opened
            if (nowActive && scrollRect != null)
            {
                // verticalNormalizedPosition: 1 = top, 0 = bottom
                scrollRect.verticalNormalizedPosition = 1f;
            }
        }
    }
}