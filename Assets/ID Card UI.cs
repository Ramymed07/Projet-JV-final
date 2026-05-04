using UnityEngine;

public class InventoryToggle : MonoBehaviour
{
    public GameObject idCardPanel;

    [Header("Hide while ID card is open")]
    public GameObject mainUI;
    public GameObject[] otherCanvasesOrPanels;

    private bool mainUIWasActive;
    private bool[] wasActive;

    void Start()
    {
        wasActive = new bool[otherCanvasesOrPanels.Length];

        if (idCardPanel != null)
            idCardPanel.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
            ToggleIDCard();
    }

    void ToggleIDCard()
    {
        bool opening = !idCardPanel.activeSelf;

        if (opening)
        {
            mainUIWasActive = mainUI.activeSelf;
            mainUI.SetActive(false);

            for (int i = 0; i < otherCanvasesOrPanels.Length; i++)
            {
                if (otherCanvasesOrPanels[i] != null)
                {
                    wasActive[i] = otherCanvasesOrPanels[i].activeSelf;
                    otherCanvasesOrPanels[i].SetActive(false);
                }
            }

            idCardPanel.SetActive(true);

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = false;
        }
        else
        {
            idCardPanel.SetActive(false);

            if (mainUI != null)
                mainUI.SetActive(mainUIWasActive);

            for (int i = 0; i < otherCanvasesOrPanels.Length; i++)
            {
                if (otherCanvasesOrPanels[i] != null)
                    otherCanvasesOrPanels[i].SetActive(wasActive[i]);
            }

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }
}
