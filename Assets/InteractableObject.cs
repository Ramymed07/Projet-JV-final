using UnityEngine;

/// <summary>
/// Attach to any scene object that should open a puzzle when the player interacts with it.
/// On puzzle completion, all GameObjects sharing <see cref="tagToDestroy"/> are destroyed.
/// </summary>
public class InteractableObject : MonoBehaviour
{
    [Header("Puzzle Settings")]
    [Tooltip("The PuzzleManager that will be shown when this object is interacted with.")]
    public PuzzleManager puzzleManager;

    [Tooltip("All GameObjects with this tag will be destroyed when the puzzle is solved.")]
    public string tagToDestroy = "Obstacle";

    [Header("Interaction Settings")]
    [Tooltip("Maximum distance at which the player can trigger the interaction.")]
    public float interactionRange = 3f;

    [Tooltip("Key the player presses to interact.")]
    public KeyCode interactKey = KeyCode.E;

    [Header("Optional")]
    [Tooltip("Assign the player Transform for range-checking. Auto-found via 'Player' tag if left empty.")]
    public Transform playerTransform;

    private bool _puzzleSolved = false;

    private void Start()
    {
        // Auto-find the player if not assigned
        if (playerTransform == null)
        {
            GameObject player = GameObject.FindWithTag("Player");
            if (player != null)
                playerTransform = player.transform;
            else
                Debug.LogWarning($"[InteractableObject] No player Transform assigned and no GameObject tagged 'Player' found.", this);
        }

        if (puzzleManager == null)
            Debug.LogWarning($"[InteractableObject] No PuzzleManager assigned on '{gameObject.name}'.", this);
    }

    private void Update()
    {
        if (_puzzleSolved) return;

        if (Input.GetKeyDown(interactKey) && IsPlayerInRange())
            OpenPuzzle();
    }

    private bool IsPlayerInRange()
    {
        if (playerTransform == null) return false;
        return Vector3.Distance(transform.position, playerTransform.position) <= interactionRange;
    }

    private void OpenPuzzle()
    {
        if (puzzleManager == null) return;
        puzzleManager.OpenPuzzle(this);
    }

    /// <summary>
    /// Called by PuzzleManager when the puzzle has been completed successfully.
    /// Destroys all scene objects matching <see cref="tagToDestroy"/>.
    /// </summary>
    public void OnPuzzleSolved()
    {
        if (_puzzleSolved) return;
        _puzzleSolved = true;

        if (string.IsNullOrEmpty(tagToDestroy))
        {
            Debug.LogWarning($"[InteractableObject] 'tagToDestroy' is empty on '{gameObject.name}'. Nothing will be destroyed.", this);
            return;
        }

        GameObject[] targets = GameObject.FindGameObjectsWithTag(tagToDestroy);

        if (targets.Length == 0)
        {
            Debug.Log($"[InteractableObject] No GameObjects found with tag '{tagToDestroy}'.");
            return;
        }

        foreach (GameObject target in targets)
        {
            Debug.Log($"[InteractableObject] Destroying '{target.name}' (tag: '{tagToDestroy}').");
            Destroy(target);
        }
    }

    // Optional: visualise the interaction range in the Scene view
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, interactionRange);
    }
}