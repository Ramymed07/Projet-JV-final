using UnityEngine;

public class PuzzlePiece : MonoBehaviour
{
    public int correctIndex;
    public int currentIndex;

    [Header("Outline")]
    public GameObject outlineObject;

    private Vector3 startPosition;
    private int startIndex;
    private Vector3 originalScale;

    void Awake()
    {
        originalScale = transform.localScale;

        if (outlineObject != null)
            outlineObject.SetActive(false);
    }

    public void SaveStartState()
    {
        startPosition = transform.position;
        startIndex = currentIndex;
    }

    public void ResetState()
    {
        transform.position = startPosition;
        currentIndex = startIndex;
        transform.localScale = originalScale;
        Deselect();
    }

    public void Select(float scaleMultiplier)
    {
        transform.localScale = originalScale * scaleMultiplier;

        if (outlineObject != null)
            outlineObject.SetActive(true);
    }

    public void Deselect()
    {
        transform.localScale = originalScale;

        if (outlineObject != null)
            outlineObject.SetActive(false);
    }
}