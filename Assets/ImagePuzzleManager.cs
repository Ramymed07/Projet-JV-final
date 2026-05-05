using UnityEngine;

public class ImagePuzzleManager : MonoBehaviour
{
    [Header("Camera")]
    public Camera playerCamera;
    public float rayDistance = 5f;

    [Header("Raycast Layer")]
    public LayerMask puzzleLayer;

    [Header("Puzzle Pieces")]
    public PuzzlePiece[] pieces;

    [Header("Selection")]
    public float selectedScaleMultiplier = 1.15f;

    [Header("Reward Key")]
    public GameObject keyObject;
    public Light keyGlowLight;

    [Header("Solved Sound")]
    public AudioSource audioSource;
    public AudioClip solvedSound;

    private PuzzlePiece firstSelected;
    private bool puzzleSolved = false;

    void Start()
    {
        foreach (PuzzlePiece piece in pieces)
        {
            if (piece != null)
                piece.SaveStartState();
        }

        ResetPuzzle();
    }

    void Update()
    {
        if (puzzleSolved) return;

        if (Input.GetMouseButtonDown(0))
        {
            TrySelectPiece();
        }
    }

    void TrySelectPiece()
    {
        if (playerCamera == null)
        {
            Debug.LogWarning("Player Camera is not assigned on ImagePuzzleManager.");
            return;
        }

        Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);

        // Debug raycast: checks what the mouse is hitting, regardless of layer.
        if (Physics.Raycast(ray, out RaycastHit debugHit, rayDistance))
        {
            Debug.Log("Hit: " + debugHit.collider.name +
                      " | Layer: " + LayerMask.LayerToName(debugHit.collider.gameObject.layer));
        }
        else
        {
            Debug.Log("Raycast hit nothing at all.");
        }

        // Real puzzle raycast: only detects objects on this manager's Puzzle Layer.
        if (Physics.Raycast(ray, out RaycastHit hit, rayDistance, puzzleLayer))
        {
            PuzzlePiece piece = hit.collider.GetComponent<PuzzlePiece>();

            if (piece == null)
                piece = hit.collider.GetComponentInParent<PuzzlePiece>();

            if (piece != null)
            {
                SelectPiece(piece);
            }
            else
            {
                Debug.Log("Hit object is on the puzzle layer but has no PuzzlePiece: " + hit.collider.name);
            }
        }
        else
        {
            Debug.Log("Raycast hit nothing on this manager's puzzleLayer.");
        }
    }

    public void SelectPiece(PuzzlePiece piece)
    {
        Debug.Log("Selected: " + piece.name);

        if (puzzleSolved) return;

        if (firstSelected == null)
        {
            Debug.Log("First piece selected");
            firstSelected = piece;
            firstSelected.Select(selectedScaleMultiplier);
            return;
        }

        if (firstSelected == piece)
        {
            Debug.Log("Same piece clicked, deselecting");
            firstSelected.Deselect();
            firstSelected = null;
            return;
        }

        Debug.Log("Swapping " + firstSelected.name + " with " + piece.name);

        SwapPieces(firstSelected, piece);

        firstSelected.Deselect();
        firstSelected = null;

        CheckIfSolved();
    }

    void SwapPieces(PuzzlePiece a, PuzzlePiece b)
    {
        Vector3 tempPos = a.transform.position;
        a.transform.position = b.transform.position;
        b.transform.position = tempPos;

        int tempIndex = a.currentIndex;
        a.currentIndex = b.currentIndex;
        b.currentIndex = tempIndex;
    }

    void CheckIfSolved()
    {
        foreach (PuzzlePiece piece in pieces)
        {
            if (piece == null) continue;

            Debug.Log(piece.name +
                " | currentIndex: " + piece.currentIndex +
                " | correctIndex: " + piece.correctIndex);

            if (piece.currentIndex != piece.correctIndex)
            {
                Debug.Log("Puzzle not solved because of: " + piece.name);
                return;
            }
        }

        puzzleSolved = true;
        LockPuzzle();
        RevealKey();
    }

    void LockPuzzle()
    {
        firstSelected = null;

        foreach (PuzzlePiece piece in pieces)
        {
            if (piece != null)
                piece.Deselect();
        }

        Debug.Log("Puzzle locked. Pieces can no longer be swapped.");
    }

    void RevealKey()
    {
        Debug.Log("Puzzle solved! Key revealed.");

        if (audioSource != null && solvedSound != null)
        {
            audioSource.PlayOneShot(solvedSound);
        }

        if (keyObject != null)
            keyObject.SetActive(true);

        if (keyGlowLight != null)
            keyGlowLight.enabled = true;
    }

    public void ResetPuzzle()
    {
        puzzleSolved = false;
        firstSelected = null;

        foreach (PuzzlePiece piece in pieces)
        {
            if (piece != null)
                piece.ResetState();
        }

        if (keyObject != null)
            keyObject.SetActive(false);

        if (keyGlowLight != null)
            keyGlowLight.enabled = false;

        Debug.Log("Puzzle reset");
    }
}