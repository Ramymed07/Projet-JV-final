using UnityEngine;

public class InteractableObject : MonoBehaviour
{
    [Header("Puzzle")]
    public PuzzleManager puzzleManager;

    [Header("Interaction")]
    public float interactionRange = 3f;
    public KeyCode interactKey = KeyCode.E;
    public Transform playerTransform;

    private bool puzzleSolved = false;

    void Start()
    {
        if (playerTransform == null)
        {
            GameObject player = GameObject.FindWithTag("Player");
            if (player != null)
                playerTransform = player.transform;
        }
    }

    void Update()
    {
        if (puzzleSolved) return;

        if (Input.GetKeyDown(interactKey) && IsPlayerInRange())
        {
            OpenPuzzle();
        }
    }

    bool IsPlayerInRange()
    {
        if (playerTransform == null) return false;

        return Vector3.Distance(transform.position, playerTransform.position) <= interactionRange;
    }

    private void OpenPuzzle()
    {
        if (puzzleManager == null)
        {
            Debug.LogWarning("PuzzleManager is not assigned.");
            return;
        }

        Debug.Log("E pressed: opening puzzle now.");
        puzzleManager.OpenPuzzle(this);
    }

    public void MarkPuzzleSolved()
    {
        puzzleSolved = true;
    }
}