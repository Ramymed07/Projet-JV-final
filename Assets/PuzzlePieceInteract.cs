using UnityEngine;

public class PuzzlePieceInteract : MonoBehaviour
{
    public ImagePuzzleManager puzzleManager;
    private PuzzlePiece puzzlePiece;

    void Start()
    {
        puzzlePiece = GetComponent<PuzzlePiece>();
    }

    void OnMouseDown()
    {
        if (puzzleManager != null && puzzlePiece != null)
        {
            puzzleManager.SelectPiece(puzzlePiece);
        }
    }
}
